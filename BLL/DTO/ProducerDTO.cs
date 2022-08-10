using DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ProducerDTO
    {
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public byte[] ProducerImage { get; set; }
        public string SmallInfo { get; set; }
        public List<Film> Films { get; set; }
    }
}
