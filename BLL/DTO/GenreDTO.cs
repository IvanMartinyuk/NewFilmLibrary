using DAL.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GenreDTO
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public List<Film> Films { get; set; }
    }
}
