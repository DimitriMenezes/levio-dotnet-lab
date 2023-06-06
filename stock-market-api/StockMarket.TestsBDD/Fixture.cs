using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using StockMarket.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.TestsBDD
{
    public class Fixture : IDisposable
    {
        public StockMarketContext StockMarketContext { get; set; }        
        //public IRentalSimulationService _simulationService { get; set; }
        //public IReservationService _reservationService { get; set; }
        //public IInspectionService _inspectionService { get; set; }


        public Fixture()
        {
            var options = new DbContextOptionsBuilder<StockMarketContext>()
                    .UseInMemoryDatabase("TestDB")
                    .Options;

            StockMarketContext = new StockMarketContext(options);

            //var vehicleRepository = new VehicleReadOnlyRepository(VehicleContext);
            //var reservationRepository = new ReservationRepository(BusinessContext);
            //var inspectionRepository = new InspectionRepository(BusinessContext);

            //_simulationService = new RentalSimulationService(vehicleRepository);

            //var _mapper = new Mock<IMapper>();

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new ServicesMapperProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();

            //_reservationService = new ReservationService(reservationRepository, vehicleRepository, mapper);

            //_inspectionService = new InspectionService(inspectionRepository, reservationRepository, mapper);
        }

        public void Dispose()
        {
            StockMarketContext.Dispose();
        }
    }
}
