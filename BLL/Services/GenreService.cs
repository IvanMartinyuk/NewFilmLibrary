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
    public class GenreService
    {
        public GenreRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public GenreService(IRepository<Genre> repository)
        {
            Repository = repository as GenreRepository;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Genre, GenreDTO>()
                                .ForMember(y => y.Films, y => y.MapFrom(c => c.Films));
                x.CreateMap<GenreDTO, Genre>();
            });
            Mapper = new Mapper(config);
        }
        public void AddOrUpdate(GenreDTO value)
        {
            Repository.CreateOrUpdate(Mapper.Map<GenreDTO, Genre>(value));
            Repository.SaveChanges();
        }
        public void Delete(GenreDTO value)
        {
            Repository.Delete(Mapper.Map<GenreDTO, Genre>(value));
            Repository.SaveChanges();
        }

        public GenreDTO Get(int id)
        {
            Repository.ReadAll();
            return Mapper.Map<Genre, GenreDTO>(Repository.Get(id));
        }

        public IEnumerable<GenreDTO> GetAll()
        {
            Repository.ReadAll();
            return Mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(Repository.GetAll());
        }
    }
}
