using DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ActorDTO
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public byte[] ActorImage { get; set; }
        public string ActorSmallInfo { get; set; }
        public List<Film> Films { get; set; }
    }
}
