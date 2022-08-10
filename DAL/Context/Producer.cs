using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class Producer
    {
        [Key]
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public byte[] ProducerImage { get; set; }
        public string SmallInfo { get; set; }
        public ICollection<Film> Films { get; set; }
    }
}
