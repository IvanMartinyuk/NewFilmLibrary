using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class GenreRepository : GenericRepository<Genre>
    {
        public GenreRepository(DbContext context) : base(context)
        {
            //ReadAll();
        }
        public void ReadAll()
        { 
            try
            {
                Genre g = context.Set<Genre>().Include(c => c.Films).ToList()[0];
            }
            catch (Exception ex)
            {

            }
        }
    }
}
