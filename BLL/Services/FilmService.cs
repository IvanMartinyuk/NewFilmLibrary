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
    public class FilmService
    {
        public FilmRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public FilmService(IRepository<Film> repository)
        {
            Repository = (FilmRepository)repository;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Film, FilmDTO>()
                                .ForMember(y => y.Genres, y => y.MapFrom(c => c.Genres))
                                .ForMember(y => y.Producers, y => y.MapFrom(c => c.Producers))
                                .ForMember(y => y.Actors, y => y.MapFrom(c => c.Actors))
                                .ForMember(y => y.Users, y => y.MapFrom(c => c.Users));
                x.CreateMap<FilmDTO, Film>();
            });
            Mapper = new Mapper(config);
        }
        public void AddOrUpdate(FilmDTO value)
        {
            Repository.CreateOrUpdate(Mapper.Map<FilmDTO, Film>(value));
            Repository.SaveChanges();
        }

        public void Delete(FilmDTO value)
        {
            Repository.Delete(Mapper.Map<FilmDTO, Film>(value));
            Repository.SaveChanges();
        }

        public FilmDTO Get(int id)
        {
            Repository.ReadAll();
            return Mapper.Map<Film, FilmDTO>(Repository.Get(id));
        }

        public IEnumerable<FilmDTO> GetAll()
        {
            Repository.ReadAll();
            return Mapper.Map<List<Film>, List<FilmDTO>>(Repository.GetAll().ToList());
        }
    }
}
