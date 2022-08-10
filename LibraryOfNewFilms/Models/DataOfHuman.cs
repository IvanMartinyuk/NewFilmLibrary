using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfNewFilms.Models
{
    public class DataOfHuman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string SmallInfo { get; set; }
        public ICollection<Film> Films { get; set; }
    }
}
