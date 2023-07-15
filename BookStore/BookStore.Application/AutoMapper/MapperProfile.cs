using AutoMapper;
using BookStore.Domain.Entities;
using RabbitMQ.Client;

namespace BookStore.Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RabbitMqConnectionData, ConnectionFactory>();
        }
    }
}
