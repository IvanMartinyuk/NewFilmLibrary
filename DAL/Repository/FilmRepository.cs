using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class FilmRepository : GenericRepository<Film>
    {
        public FilmRepository(DbContext context) : base(context)
        {
            //ReadAll();
        }
        public void ReadAll()
        {
            try
            {
                Film g = context.Set<Film>()
                                .Include(c => c.Actors)
                                .Include(c => c.Genres)
                                .Include(c => c.Producers)
                                .Include(c => c.Users).ToList()[0];
            }
            catch(Exception ex)
            {

            }
        }
    }
}
