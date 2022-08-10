using BLL.DTO;
using BLL.Services;
using DAL.Context;
using DAL.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryOfNewFilms.Models
{
    public class NetworkManager
    {
        public delegate void Loaded();
        public static event Loaded EndLoad;
        public static bool IsEnd = false;
        readonly string startUrlTitle = "https://imdb8.p.rapidapi.com/title/";
        readonly string startUrlActor = "https://imdb8.p.rapidapi.com/actors/";
        public void SelectInfo()
        {
            try
            {
                GetFilms();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (!ex.InnerException.Message.Contains("429"))
                    {
                        GetFilms();
                        File.AppendAllText(DateTime.Now + "\nException.txt", ex.Message);
                    }
                    else
                        MessageBox.Show(ex.InnerException.Message);
                }
                else
                    MessageBox.Show(ex.Message);
            }
        }
        async Task<string> SendRequest(string uri)
        {
            string str;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                    {
                        { "x-rapidapi-key", "7467395e30msh911943b787d67b4p1adf6djsncee5f2744cae" },
                        { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                str = body;
            }
            if (str == "{\"message\":\"The security token included in the request is expired\"}")
                str = SendRequest(uri).Result;
            return str;
        }

        List<string> SelectTitles(FilmRepository fr)
        {
            List<Film> fl = fr.GetAll().ToList();
            List<string> films = fl.Select(x => x.FilmId).ToList();
            string uri = startUrlTitle + "get-coming-soon-movies?homeCountry=US&purchaseCountry=US&currentCountry=US";
            List<string> titles = new List<string>();
            List<string> titleStr = new List<string>(SendRequest(uri).Result.Split(new char[] { ',' }));
            foreach (string ttl in titleStr)
            {
                string title = ttl.Split(new char[] { '/' })[2];
                if(!films.Contains(title))
                    titles.Add(title);
            }
            return titles;
        }
        void GetFilms()
        {
            FilmRepository fr = new FilmRepository(new LibraryContext());
            string uri = "http://www.omdbapi.com/?apikey=7aaeeadb&plot=full&i=";
            List<string> titles = SelectTitles(fr);
            DateTime today = DateTime.Now;
            int day = today.Day;
            if (day == 31)
                day = 30;
            int month = today.Month + 3;
            int year = today.Year;
            if (today.Month + 3 > 12)
            {
                month = today.Month + 3 - 12;
                year = today.Year + 1;
            }
            DateTime time = new DateTime(year, month, day);
            foreach(string ttl in titles)
            {
                Film film = GetFilm(uri, ttl).Result;
                fr.CreateOrUpdate(film);
                fr.SaveChanges();
                fr.ReadAll();
                List<Film> films = fr.GetAll().ToList();
                film = films[films.Count - 1];
                CleaningUpExcess();
                FillIfItsCheked(film);
                EndLoad?.Invoke();
                if (IsEnd)
                    break;
                if (film.DateOfPubliced > time)
                    break;
            }
        }

        private void FillIfItsCheked(Film film)
        {
            GenreRepository gr = new GenreRepository(new LibraryContext());
            ActorRepository ar = new ActorRepository(new LibraryContext());
            ProducerRepository pr = new ProducerRepository(new LibraryContext());
            gr.ReadAll();
            List<Genre> genres = gr.GetAll().ToList();
            List<Genre> filmGenres = film.Genres.ToList();
            for (int i = 0; i < genres.Count; i++)
            {
                for (int j = 0; j < film.Genres.Count; j++)
                    if (filmGenres[j].GenreName == genres[i].GenreName)
                    {
                        try
                        {
                            StartDB.SendCommand($"exec AddRelationshipGenreFilm @filmId = {film.Id}, @genreId = {genres[i].GenreId}");
                        }
                        catch (Exception ex) { }
                    }
            }
            genres.Clear();
            ar.ReadAll();
            List<Actor> actors = ar.GetAll().ToList();
            List<Actor> filmActors = film.Actors.ToList();
            for (int i = 0; i < actors.Count; i++)
            {
                for (int j = 0; j < film.Actors.Count; j++)
                    if (filmActors[j].ActorName == actors[i].ActorName)
                    {
                        try
                        {
                            StartDB.SendCommand($"exec AddRelationshipActorFilm @filmId = {film.Id}, @actorId = {actors[i].ActorId}");
                        }
                        catch (Exception ex) { }
                    }
            }
            actors.Clear();
            pr.ReadAll();
            List<Producer> producers = pr.GetAll().ToList();
            List<Producer> filmProducers = film.Producers.ToList();
            for (int i = 0; i < producers.Count; i++)
                for (int j = 0; j < film.Producers.Count; j++)
                    if (filmProducers[j].ProducerName == producers[i].ProducerName)
                    {
                        try
                        {
                            StartDB.SendCommand($"exec AddRelationshipProducerFilm @filmId = {film.Id}, @producerId = {producers[i].ProducerId}");
                        }
                        catch (Exception ex) { }
                    }
            producers.Clear();
        }

        private void CleaningUpExcess()
        {
            GenreRepository gr = new GenreRepository(new LibraryContext());
            ActorRepository ar = new ActorRepository(new LibraryContext());
            ProducerRepository pr = new ProducerRepository(new LibraryContext());
            List<Genre> genres = gr.GetAll().ToList();
            List<string> unique = new List<string>();
            List<int> id = new List<int>();
            foreach(Genre g in genres)
            {
                if (!unique.Contains(g.GenreName))
                {
                    unique.Add(g.GenreName);
                    id.Add(g.GenreId);
                }
            }
            foreach (Genre g in genres)
            {
                if (!id.Contains(g.GenreId))
                    gr.Delete(g);
            }
            gr.SaveChanges();
            genres.Clear();
            List<Actor> actors = ar.GetAll().ToList();
            unique.Clear();
            id.Clear();
            foreach (Actor a in actors)
            {
                if (!unique.Contains(a.ActorName))
                {
                    unique.Add(a.ActorName);
                    id.Add(a.ActorId);
                }
            }
            foreach (Actor a in actors)
            {
                if (!id.Contains(a.ActorId))
                    ar.Delete(a);
            }
            ar.SaveChanges();
            actors.Clear();
            List<Producer> producers = pr.GetAll().ToList();
            unique.Clear();
            id.Clear(); 
            foreach (Producer p in producers)
            {
                if (!unique.Contains(p.ProducerName))
                {
                    unique.Add(p.ProducerName);
                    id.Add(p.ProducerId);
                }
            }
            foreach (Producer p in producers)
            {
                if (!id.Contains(p.ProducerId))
                    pr.Delete(p);
            }
            pr.SaveChanges();
            producers.Clear();
        }
        async Task<Film> GetFilm(string uri, string ttl)
        {           
            WebClient webClient = new WebClient();
            string metaData = webClient.DownloadString(uri + ttl);
            JObject obj = (JObject)JsonConvert.DeserializeObject(metaData);
            Film film = new Film();
            film.FilmId = ttl;
            film.FilmName = obj["Title"].ToString();
            if (obj["Poster"].ToString() != "N/A")
                film.Poster = RefreshImage(await webClient.DownloadDataTaskAsync(obj["Poster"].ToString()));
            else
                film.Poster = RefreshImage(await webClient.DownloadDataTaskAsync(new Uri("https://reconomica.ru/wp-content/uploads/2020/02/unnamed-11.jpg")));
            film.RunTime = obj["Runtime"].ToString();
            var cultureInfo = new CultureInfo("de-DE");
            if (obj["Released"].ToString() != "N/A")
            {
                film.DateOfPubliced = DateTime.Parse(obj["Released"].ToString(), cultureInfo);
            }
            else
                film.DateOfPubliced = new DateTime(DateTime.Now.Year, 1, 1);
            film.Plot = obj["Plot"].ToString();
            film.Genres = GetGenres(obj);
            film.Producers = GetProducers(ttl).Result;
            film.Actors = GetActors(ttl).Result;
            return film;
        }

        List<Genre> GetGenres(JObject data)
        {
            List<Genre> genres = new List<Genre>();
            string str = data["Genre"].ToString();
            List<string> genre = new List<string>();
            string[] strs = str.Split(new char[] { ',' });
            for (int j = 0; j < strs.Length; j++)
            {
                genre.Add("");
                for (int i = 0; i < strs[j].Length; i++)
                    if (strs[j][i] != ' ')
                        genre[j] += strs[j][i];
            }
            foreach (string g in genre)
                genres.Add(new Genre() { GenreName = g });
            return genres;
        }
        async Task<List<Producer>> GetProducers(string title)
        {
            string uri = startUrlTitle + "get-top-crew?tconst=" + title;
            List<Producer> producers = new List<Producer>();
            JObject obj = (JObject)JsonConvert.DeserializeObject(SendRequest(uri).Result);
            List<string> ids = new List<string>();
            int count = 0;
            if (obj["writers"]?.Count() != null)
                count = (int)obj["writers"]?.Count();
            if (count > 2)
                count = 2;
            for (int i = 0; i < count; i++)
            {
                producers.Add(new Producer()
                {
                    ProducerName = obj["writers"][i]["name"]?.ToString()
                });
                if (obj["writers"]?[i]?["image"]?["url"]?.ToString() != null)
                    producers[producers.Count - 1].ProducerImage = RefreshImage(await new WebClient().DownloadDataTaskAsync(obj["writers"][i]["image"]?["url"]?.ToString()));
                else
                    producers[producers.Count - 1].ProducerImage = RefreshImage(await new WebClient().DownloadDataTaskAsync("https://www.icrewz.com/wp-content/files/producers-chair1-233x244.jpg"));
                ids.Add(obj["writers"]?[i]?["id"].ToString());
            }
            for (int i = 0; i < ids.Count; i++)
            {
                uri = startUrlActor + "get-bio?nconst=" + ids[i].Split(new char[] { '/' })[2];
                obj = (JObject)JsonConvert.DeserializeObject(SendRequest(uri).Result);
                producers[i].SmallInfo = obj["miniBios"]?[0]?["text"]?.ToString();
            }
            return producers;
        }
        async Task<List<Actor>> GetActors(string title)
        {
            string uri = startUrlTitle + "get-top-cast?tconst=" + title;
            List<Actor> actors = new List<Actor>();
            JArray array = (JArray)JsonConvert.DeserializeObject(SendRequest(uri).Result);
            List<string> ids = new List<string>();
            foreach (var val in array)
            {
                ids.Add(val.Value<string>());
            }
            int count = ids.Count;
            if (count > 7)
                count = 7;
            for (int i = 0; i < count; i++)
            {
                uri = startUrlActor + "get-bio?nconst=" + ids[i].Split(new char[] { '/' })[2];
                JObject obj = (JObject)JsonConvert.DeserializeObject(SendRequest(uri).Result);
                actors.Add(new Actor()
                {
                    ActorName = obj["name"]?.ToString(),
                    ActorSmallInfo = obj["miniBios"]?[0]?["text"]?.ToString()
                });
                if (obj["image"]?["url"]?.ToString() != null)
                    actors[actors.Count - 1].ActorImage = RefreshImage(await new WebClient().DownloadDataTaskAsync(obj["image"]?["url"]?.ToString()));
                else
                    actors[actors.Count - 1].ActorImage = RefreshImage(await new WebClient().DownloadDataTaskAsync("https://e7.pngegg.com/pngimages/906/448/png-clipart-silhouette-person-person-with-helmut-animals-logo.png"));
            }
            return actors;
        }
        byte[] RefreshImage(byte[] array)
        {
            Bitmap bitmap = new Bitmap(GetImageFromeByteArray(array), 190, 300);
            return ImageToBytes(bitmap);
        }
        Image GetImageFromeByteArray(byte[] array)
        {
            using (MemoryStream ms = new MemoryStream(array))
            {
                return Image.FromStream(ms);
            }
        }
        byte[] ImageToBytes(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
