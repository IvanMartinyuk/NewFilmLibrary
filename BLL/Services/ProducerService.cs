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
    public class ProducerService
    {
        public ProducerRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public ProducerService(IRepository<Producer> repository)
        {
            Repository = repository as ProducerRepository;
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Producer, ProducerDTO>()
                                .ForMember(y => y.Films, y => y.MapFrom(c => c.Films));
                x.CreateMap<ProducerDTO, Producer>();
            });
            Mapper = new Mapper(config);
        }
        public void AddOrUpdate(ProducerDTO value)
        {
            Repository.CreateOrUpdate(Mapper.Map<ProducerDTO, Producer>(value));
            Repository.SaveChanges();
        }
        public void Delete(ProducerDTO value)
        {
            Repository.Delete(Mapper.Map<ProducerDTO, Producer>(value));
            Repository.SaveChanges();
        }

        public ProducerDTO Get(int id)
        {
            Repository.ReadAll();
            return Mapper.Map<Producer, ProducerDTO>(Repository.Get(id));
        }

        public IEnumerable<ProducerDTO> GetAll()
        {
            Repository.ReadAll();
            return Mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerDTO>>(Repository.GetAll());
        }
    }
}
