using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public string FilmId { get; set; }
        public byte[] Poster { get; set; }
        public string FilmName { get; set; }
        public DateTime DateOfPubliced { get; set; }
        public string RunTime { get; set; }
        public string Treiler { get; set; }
        public string Plot { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Producer> Producers { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
