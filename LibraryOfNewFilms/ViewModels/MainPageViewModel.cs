using AutoMapper;
using BLL.DTO;
using BLL.Services;
using DAL.Context;
using DAL.Repository;
using LibraryOfNewFilm.Infrastructure;
using LibraryOfNewFilms.Infrastructure;
using LibraryOfNewFilms.Models;
using LibraryOfNewFilms.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryOfNewFilms.ViewModels
{
    public class MainPageViewModel : BaseNotifyPropertyChanged
    {
        IMapper mapper;
        ObservableCollection<GenreDTO> genres;
        GenreService gs;
        FilmService fs;
        ActorService acs;
        ProducerService ps;
        bool isSortByGenre = false;
        bool isSortByActor = false;
        bool isSortByProducer = false;
        bool isUserFilms = false;
        bool isLoadData = false;
        int startLimitOfLoad = 20;
        int maxOfLoad = 0;
        public ICommand ExitCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand StopLoadDataCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand ResetGenresCommand { get; set; }
        public ICommand ResetActorsCommand { get; set; }
        public ICommand ResetProducersCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RestoreFilmsCommand { get; set; }
        public ICommand LoadMoreFilmCommand { get; set; }
        public ObservableCollection<GenreDTO> Genres
        {
            get => genres;
            set
            {
                genres = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<ActorDTO> actors;
        public ObservableCollection<ActorDTO> Actors
        {
            get => actors;
            set
            {
                actors = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<ProducerDTO> producers;
        public ObservableCollection<ProducerDTO> Producers
        {
            get => producers;
            set
            {
                producers = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<FilmDTO> films;
        public ObservableCollection<FilmDTO> Films
        {
            get => films;
            set
            {
                films = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<FilmDTO> sourceFilms;
        public ObservableCollection<FilmDTO> SourceFilms
        {
            get => sourceFilms;
            set
            {
                sourceFilms = value;
                if(!isUserFilms)
                    Films = GetPartFilms();
            }
        }
        GenreDTO selectedGenre;
        public GenreDTO SelectedGenre
        {
            get => selectedGenre;
            set
            {
                selectedGenre = value;
                Sort(value, true);
                isSortByGenre = true;
            }
        }
        ActorDTO selectedActor;
        public ActorDTO SelectedActor
        {
            get => selectedActor;
            set
            {
                selectedActor = value;
                Sort(value, true);
                isSortByActor = true;
            }
        }
        ProducerDTO selectedProducer;
        public ProducerDTO SelectedProducer
        {
            get => selectedProducer;
            set
            {
                selectedProducer = value;
                Sort(value, true);
                isSortByProducer = true;
            }
        }
        FilmDTO selectedFilm;
        public FilmDTO SelectedFilm
        {
            get => selectedFilm;
            set
            {
                selectedFilm = value;
                OpenFilm(value);
            }
        }
        Visibility notificationsVisibility;
        public Visibility NotificationsVisibility
        {
            get => notificationsVisibility;
            set
            {
                notificationsVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility loadButtonVisibility;
        public Visibility LoadButtonVisibility
        {
            get => loadButtonVisibility;
            set
            {
                loadButtonVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility loadVisibility;
        public Visibility LoadVisibility
        {
            get => loadVisibility;
            set
            {
                loadVisibility = value;
                NotifyPropertyChanged();
            }
        }
        int maxHeight;
        public int MaxHeight
        {
            get => maxHeight;
            set
            {
                maxHeight = value;
                NotifyPropertyChanged();
            }
        }
        int maxWidth;
        public int MaxWidth
        {
            get => maxWidth;
            set
            {
                maxWidth = value;
                NotifyPropertyChanged();
            }
        }
        string columnWidth;
        public string ColumnWidth
        {
            get => columnWidth;
            set
            {
                columnWidth = value;
                NotifyPropertyChanged();
            }
        }
        public MainPageViewModel()
        {
            LoadVisibility = Visibility.Visible;
            Films = new ObservableCollection<FilmDTO>();
            maxOfLoad = startLimitOfLoad;
            NotificationsVisibility = Visibility.Collapsed;
            LoadButtonVisibility = Visibility.Visible;
            MaxHeight = 440;
            ColumnWidth = "20*";
            MaxWidth = 600;
            NetworkManager.EndLoad += Reload;
            fs = new FilmService(new FilmRepository(new LibraryContext()));
            gs = new GenreService(new GenreRepository(new LibraryContext()));
            acs = new ActorService(new ActorRepository(new LibraryContext()));
            ps = new ProducerService(new ProducerRepository(new LibraryContext()));
            try
            {
                Task.Run(() =>
                {
                    SourceFilms = new ObservableCollection<FilmDTO>(fs.GetAll());
                    LoadVisibility = Visibility.Collapsed;
                    Genres = new ObservableCollection<GenreDTO>(gs.GetAll());
                    Actors = new ObservableCollection<ActorDTO>(acs.GetAll());
                    Producers = new ObservableCollection<ProducerDTO>(ps.GetAll());
                    if (SourceFilms.Count == 0)
                        CreateAdmin();
                });
            }
            catch (Exception ex)
            {
                File.AppendAllText("Exception.txt", "\nMainPageViewModel\n" + ex.Message);
            }
            InitCommand();
        }

        private void CreateAdmin()
        {           
            UserService us = new UserService(new UserRepository(new LibraryContext()));
            List<UserDTO> users = us.GetAll().ToList();
            foreach (UserDTO usr in users)
                if (usr.UserName == "Admin")
                    return;
            UserDTO user = new UserDTO()
            {
                UserLogin = "Admin",
                UserPassword = "Admin".GetHashCode().ToString(),
                IsAdmin = true,
                UserName = "Admin"
            };
            us.AddOrUpdate(user);
            user = new UserDTO()
            {
                UserLogin = "Admin1",
                UserPassword = "Adminus".GetHashCode().ToString(),
                IsAdmin = true,
                UserName = "Admin1"
            };
            us.AddOrUpdate(user);
        }

        private void InitCommand()
        {
            ExitCommand = new RelayCommand(x =>
            {
                ((MainView)Switcher.Content).Close();
            });
            LoadDataCommand = new RelayCommand(x =>
            {
                if (!isLoadData)
                {
                    isLoadData = true;
                    NetworkManager ia = new NetworkManager();
                    Task.Run(() => ia.SelectInfo());
                }
            });
            ResetGenresCommand = new RelayCommand(x =>
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                isSortByGenre = false;
                if (isSortByActor)
                    Sort(SelectedActor, true);
                if (isSortByProducer)
                    Sort(SelectedProducer, true);
            });
            ResetActorsCommand = new RelayCommand(x =>
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                isSortByActor = false;
                if (isSortByGenre)
                    Sort(SelectedGenre, true);
                if (isSortByProducer)
                    Sort(SelectedProducer, true);
            });
            ResetProducersCommand = new RelayCommand(x =>
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                isSortByProducer = false;
                if (isSortByActor)
                    Sort(SelectedActor, true);
                if (isSortByGenre)
                    Sort(SelectedGenre, true);
            });
            StopLoadDataCommand = new RelayCommand(x =>
            {
                NetworkManager.IsEnd = true;
                isLoadData = false;
            });
            SearchCommand = new RelayCommand(x =>
            {
                ObservableCollection<FilmDTO> searchedFilm = new ObservableCollection<FilmDTO>();
                foreach (FilmDTO film in Films)
                    if (film.FilmName.Contains(x.ToString()))
                        searchedFilm.Add(film);
                Films = searchedFilm;
            });
            RestoreFilmsCommand = new RelayCommand(x =>
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                if (isSortByProducer)
                    Sort(SelectedProducer, true);
                if (isSortByActor)
                    Sort(SelectedActor, true);
                if (isSortByGenre)
                    Sort(SelectedGenre, true);
            });
            LoadMoreFilmCommand = new RelayCommand(x =>
            {
                maxOfLoad += startLimitOfLoad;
                SourceFilms = sourceFilms;
            });
        }
        void Reload()
        {
            Genres = new ObservableCollection<GenreDTO>(gs.GetAll());
            SourceFilms = new ObservableCollection<FilmDTO>(fs.GetAll());
            Actors = new ObservableCollection<ActorDTO>(acs.GetAll());
            Producers = new ObservableCollection<ProducerDTO>(ps.GetAll());
        }
        void OpenFilm(FilmDTO film)
        {
            film = fs.Get(film.Id);
            FilmDetailView filmView = new FilmDetailView();
            FilmDetailViewModel vm = (FilmDetailViewModel)filmView.DataContext;
            vm.SetFilm(film);
            if(LoginAndRegistrationViewModel.IsLogin)
                vm.CheckFilm();
            Switcher.Switch(filmView);
        }
        void Sort(ActorDTO actor, bool notSort)
        {
            if (actor == null)
                return;
            List<FilmDTO> findedFilms = new List<FilmDTO>();
            if (isSortByActor)
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                if (isSortByGenre && notSort)
                    Sort(SelectedGenre, false);
                if (isSortByProducer && notSort)
                    Sort(SelectedProducer, false);
            }
            foreach (FilmDTO film in Films)
                foreach(Actor ac in film.Actors)
                    if (ac.ActorName == actor.ActorName)
                        findedFilms.Add(film);
            Films = new ObservableCollection<FilmDTO>(findedFilms);
        }
        void Sort(GenreDTO genre, bool notSort)
        {
            if (genre == null)
                return;
            List<FilmDTO> findedFilms = new List<FilmDTO>();
            if (isSortByGenre)
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                if (isSortByActor && notSort)
                    Sort(SelectedActor, false);
                if (isSortByProducer && notSort)
                    Sort(SelectedProducer, false);
            }
            foreach (FilmDTO film in Films)
                foreach (Genre g in film.Genres)
                    if (g.GenreName == genre.GenreName)
                        findedFilms.Add(film);
            Films = new ObservableCollection<FilmDTO>(findedFilms);
        }
        void Sort(ProducerDTO producer, bool notSort)
        {
            if (producer == null)
                return;
            List<FilmDTO> findedFilms = new List<FilmDTO>();
            if (isSortByProducer)
            {
                Films = new ObservableCollection<FilmDTO>(fs.GetAll());
                if (isSortByActor && notSort)
                    Sort(SelectedActor, false);
                if (isSortByGenre && notSort)
                    Sort(SelectedGenre, false);
            }
            foreach (FilmDTO film in Films)
                foreach (Producer p in film.Producers)
                    if (p.ProducerName == producer.ProducerName)
                        findedFilms.Add(film);
            Films = new ObservableCollection<FilmDTO>(findedFilms);
        }
        public void SetToFavoriteFilm()
        {
            isUserFilms = true;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Film, FilmDTO>()
                                .ForMember(y => y.Genres, y => y.MapFrom(c => c.Genres))
                                .ForMember(y => y.Producers, y => y.MapFrom(c => c.Producers))
                                .ForMember(y => y.Actors, y => y.MapFrom(c => c.Actors))
                                .ForMember(y => y.Users, y => y.MapFrom(c => c.Users));
            });
            mapper = new Mapper(config);
            Films = new ObservableCollection<FilmDTO>(mapper.Map<List<Film>, List<FilmDTO>>(UserProfileViewModel.UserDto.FavoriteFilms));
            NotificationsVisibility = Visibility.Visible;
            LoadButtonVisibility = Visibility.Collapsed;
            MaxHeight = 400;
            ColumnWidth = "0*";
            MaxWidth = 790;
        }
        ObservableCollection<FilmDTO> GetPartFilms()
        {
            ObservableCollection<FilmDTO> films = new ObservableCollection<FilmDTO>();
            int count = sourceFilms.Count;
            if (count > maxOfLoad)
                count = maxOfLoad;
            for (int i = 0; i < count; i++)
                films.Add(SourceFilms[i]);
            return films;
        }
    }
}