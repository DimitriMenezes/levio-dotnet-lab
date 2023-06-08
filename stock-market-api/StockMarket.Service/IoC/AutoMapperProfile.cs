using AutoMapper;
using StockMarket.Domain.Entities;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.IoC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>()
                .ReverseMap()
                .ForMember(i => i.Password, j => j.Ignore());

            CreateMap<EntrepriseModel, Entreprise>()
                .ReverseMap();

            CreateMap<TickerResultModel, HistoricalTicker>()
                .ReverseMap();

            CreateMap<RealTimeTickerResultModel, RealTimeTicker>()
                .ReverseMap();
        }
    }
}
