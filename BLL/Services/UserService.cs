using AutoMapper;
using BLL.DTO;
using DAL.Context;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        public UserRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public UserService(IRepository<User> repository)
        {
            Repository = repository as UserRepository;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<User, UserDTO>()
                            .ForMember(y => y.FavoriteFilms, y => y.MapFrom(c => c.FavoriteFilms));
                x.CreateMap<UserDTO, User>();
            });
            Mapper = new Mapper(config);
        }
        public void AddOrUpdate(UserDTO value)
        {
            Repository.CreateOrUpdate(Mapper.Map<UserDTO, User>(value));
            Repository.SaveChanges();
        }

        public void Delete(UserDTO value)
        {
            Repository.Delete(Mapper.Map<UserDTO, User>(value));
            Repository.SaveChanges();
        }

        public UserDTO Get(int id)
        {
            Repository.ReadAll();
            return Mapper.Map<User, UserDTO>(Repository.Get(id));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            Repository.ReadAll();
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(Repository.GetAll());
        }
    }
}
