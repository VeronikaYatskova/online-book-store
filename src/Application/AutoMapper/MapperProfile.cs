using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using RabbitMQ.Client;

namespace Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RabbitMqConnectionData, ConnectionFactory>();
        }
    }
}
