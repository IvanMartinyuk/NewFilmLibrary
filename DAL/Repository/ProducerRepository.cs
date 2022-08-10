using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ProducerRepository : GenericRepository<Producer>
    {
        public ProducerRepository(DbContext context) : base(context)
        {
            //ReadAll();
        }
        public void ReadAll()
        {
            try
            {
                Producer g = context.Set<Producer>().Include(c => c.Films).ToList()[0];
            }
            catch (Exception ex)
            {

            }
        }
    }
}
