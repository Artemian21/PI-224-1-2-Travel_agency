using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using NSubstitute;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Transports;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.Tests.ServicesTests;

    public class TransportServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TransportService _service;

        public TransportServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork = _fixture.Freeze<IUnitOfWork>();
            _mapper = _fixture.Freeze<IMapper>();
            _service = new TransportService(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task GetAllTransportsAsync_ReturnsMappedTransportDtos()
        {
            // Arrange
            var entities = _fixture.CreateMany<TransportEntity>();
            var dtos = _fixture.CreateMany<TransportDto>();
            _unitOfWork.Transports.GetAllTransportsAsync().Returns(entities);
            _mapper.Map<IEnumerable<TransportDto>>(entities).Returns(dtos);

            // Act
            var result = await _service.GetAllTransportsAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetTransportByIdAsync_ValidId_ReturnsDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = _fixture.Create<TransportEntity>();
            var dto = _fixture.Create<TransportWithBookingsDto>();
            _unitOfWork.Transports.GetTransportByIdAsync(id).Returns(entity);
            _mapper.Map<TransportWithBookingsDto>(entity).Returns(dto);

            // Act
            var result = await _service.GetTransportByIdAsync(id);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetTransportByIdAsync_NotFound_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Transports.GetTransportByIdAsync(id).Returns((TransportEntity)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetTransportByIdAsync(id));
        }

        [Fact]
        public async Task AddTransportAsync_ValidTransport_ReturnsMappedDto()
        {
            var dto = _fixture.Build<TransportDto>()
                .With(x => x.DepartureDate, DateTime.UtcNow.AddDays(1))
                .With(x => x.ArrivalDate, DateTime.UtcNow.AddDays(2))
                .With(x => x.Price, 100)
                .With(x => x.Company, "Company")
                .With(x => x.Type, "Bus")
                .Create();

            var entity = _fixture.Create<TransportEntity>();
            var savedEntity = _fixture.Create<TransportEntity>();
            var resultDto = _fixture.Create<TransportDto>();

            _mapper.Map<TransportEntity>(dto).Returns(entity);
            _unitOfWork.Transports.AddTransportAsync(entity).Returns(savedEntity);
            _mapper.Map<TransportDto>(savedEntity).Returns(resultDto);

            var result = await _service.AddTransportAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Theory]
        [InlineData(-10, "Company", "Bus", true, true)]
        [InlineData(100, "", "Bus", true, true)]
        [InlineData(100, "Company", "", true, true)]
        [InlineData(100, "Company", "Bus", false, true)]
        [InlineData(100, "Company", "Bus", true, false)]
        public async Task AddTransportAsync_InvalidDto_ThrowsBusinessValidationException(decimal price, string company, string type, bool futureDeparture, bool arrivalAfterDeparture)
        {
            var departure = futureDeparture ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddDays(-1);
            var arrival = arrivalAfterDeparture ? departure.AddHours(1) : departure.AddHours(-1);

            var dto = new TransportDto
            {
                Price = price,
                Company = company,
                Type = type,
                DepartureDate = departure,
                ArrivalDate = arrival
            };

            await Assert.ThrowsAsync<BusinessValidationException>(() => _service.AddTransportAsync(dto));
        }

        [Fact]
        public async Task UpdateTransportAsync_ValidTransport_ReturnsMappedDto()
        {
            var dto = _fixture.Build<TransportDto>()
                .With(x => x.DepartureDate, DateTime.UtcNow.AddDays(1))
                .With(x => x.ArrivalDate, DateTime.UtcNow.AddDays(2))
                .With(x => x.Price, 100)
                .With(x => x.Company, "Company")
                .With(x => x.Type, "Bus")
                .Create();

            var entity = _fixture.Create<TransportEntity>();
            var updatedEntity = _fixture.Create<TransportEntity>();
            var resultDto = _fixture.Create<TransportDto>();

            _mapper.Map<TransportEntity>(dto).Returns(entity);
            _unitOfWork.Transports.UpdateTransportAsync(entity).Returns(updatedEntity);
            _mapper.Map<TransportDto>(updatedEntity).Returns(resultDto);

            var result = await _service.UpdateTransportAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Fact]
        public async Task UpdateTransportAsync_TransportNotFound_ThrowsNotFoundException()
        {
            var dto = _fixture.Build<TransportDto>()
                .With(x => x.DepartureDate, DateTime.UtcNow.AddDays(1))
                .With(x => x.ArrivalDate, DateTime.UtcNow.AddDays(2))
                .With(x => x.Price, 100)
                .With(x => x.Company, "Company")
                .With(x => x.Type, "Bus")
                .Create();

            var entity = _fixture.Create<TransportEntity>();
            _mapper.Map<TransportEntity>(dto).Returns(entity);
            _unitOfWork.Transports.UpdateTransportAsync(entity).Returns((TransportEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateTransportAsync(dto));
        }

        [Fact]
        public async Task DeleteTransportAsync_ValidId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Create<TransportEntity>();
            _unitOfWork.Transports.GetTransportByIdAsync(id).Returns(entity);

            var result = await _service.DeleteTransportAsync(id);

            Assert.True(result);
            await _unitOfWork.Transports.Received(1).DeleteTransportAsync(id);
        }

        [Fact]
        public async Task DeleteTransportAsync_NotFound_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Transports.GetTransportByIdAsync(id).Returns((TransportEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteTransportAsync(id));
        }
    }