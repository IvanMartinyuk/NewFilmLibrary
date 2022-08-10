using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public string FilmId { get; set; }
        public byte[] Poster { get; set; }
        public string FilmName { get; set; }
        public DateTime DateOfPubliced { get; set; }
        public string RunTime { get; set; }
        public string Treiler { get; set; }
        public string Plot { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Producer> Producers { get; set; }
        public List<Actor> Actors { get; set; }
        public List<User> Users { get; set; }
    }
}
