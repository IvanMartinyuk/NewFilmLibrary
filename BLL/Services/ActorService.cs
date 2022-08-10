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
    public class ActorService
    {
        public ActorRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public ActorService(IRepository<Actor> repository)
        {
            Repository = repository as ActorRepository;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Actor, ActorDTO>()
                            .ForMember(y => y.Films, y => y.MapFrom(c => c.Films));
                x.CreateMap<ActorDTO, Actor>();
            });
            Mapper = new Mapper(config);
        }
        public void AddOrUpdate(ActorDTO value)
        {
            Repository.CreateOrUpdate(Mapper.Map<ActorDTO, Actor>(value));
            Repository.SaveChanges();
        }
        public void Delete(ActorDTO value)
        {
            Repository.Delete(Mapper.Map<ActorDTO, Actor>(value));
            Repository.SaveChanges();
        }

        public ActorDTO Get(int id)
        {
            Repository.ReadAll();
            return Mapper.Map<Actor, ActorDTO>(Repository.Get(id));
        }

        public IEnumerable<ActorDTO> GetAll()
        {
            Repository.ReadAll();
            return Mapper.Map<IEnumerable<Actor>, IEnumerable<ActorDTO>>(Repository.GetAll());
        }
    }
}
