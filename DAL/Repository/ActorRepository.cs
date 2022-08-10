using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ActorRepository : GenericRepository<Actor>
    {
        public ActorRepository(DbContext context) : base(context)
        {
            //ReadAll();
        }
        public void ReadAll()
        { 
            try
            {
                Actor g = context.Set<Actor>().Include(c => c.Films).ToList()[0];
            }
            catch (Exception ex)
            {

            }
        }
    }
}
