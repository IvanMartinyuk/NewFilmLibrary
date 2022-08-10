using DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
            //ReadAll();
        }
        public void ReadAll()
        {
            try
            {
                User g = context.Set<User>().Include(c => c.FavoriteFilms).ToList()[0];
            }
            catch (Exception ex)
            {

            }
        }
    }
}
