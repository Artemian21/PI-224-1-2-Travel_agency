using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using NSubstitute;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.Tests.ServicesTests;
    public class HotelBookingServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HotelBookingService _service;

        public HotelBookingServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork = _fixture.Freeze<IUnitOfWork>();
            _mapper = _fixture.Freeze<IMapper>();
            _service = new HotelBookingService(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task GetAllHotelBookingsAsync_ReturnsMappedDtos()
        {
            var entities = _fixture.CreateMany<HotelBookingEntity>(3).ToList();
            var dtos = _fixture.CreateMany<HotelBookingDto>(3);
            _unitOfWork.HotelBookings.GetAllHotelBookingsAsync().Returns(entities);
            _mapper.Map<IEnumerable<HotelBookingDto>>(entities).Returns(dtos);

            var result = await _service.GetAllHotelBookingsAsync();

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetHotelBookingByIdAsync_ExistingId_ReturnsDetailsDto()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Create<HotelBookingEntity>();
            var dto = _fixture.Create<HotelBookingDetailsDto>();
            _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(id).Returns(entity);
            _mapper.Map<HotelBookingDetailsDto>(entity).Returns(dto);

            var result = await _service.GetHotelBookingByIdAsync(id);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetHotelBookingByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(id).Returns((HotelBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetHotelBookingByIdAsync(id));
        }

        [Fact]
        public async Task AddHotelBookingAsync_ValidDto_ReturnsMappedDto()
        {
            var dto = _fixture.Create<HotelBookingDto>();
            dto.StartDate = DateTime.UtcNow.AddDays(1);
            dto.EndDate = dto.StartDate.AddDays(2);
            dto.HotelRoomId = Guid.NewGuid();
            dto.UserId = Guid.NewGuid();

            var entity = _fixture.Create<HotelBookingEntity>();
            var addedEntity = _fixture.Create<HotelBookingEntity>();
            var resultDto = _fixture.Create<HotelBookingDto>();

            _mapper.Map<HotelBookingEntity>(dto).Returns(entity);
            _unitOfWork.HotelBookings.AddHotelBookingAsync(entity).Returns(addedEntity);
            _mapper.Map<HotelBookingDto>(addedEntity).Returns(resultDto);

            var result = await _service.AddHotelBookingAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Theory]
        [InlineData(null)]
        public async Task AddHotelBookingAsync_NullDto_ThrowsArgumentNullException(HotelBookingDto dto)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddHotelBookingAsync(dto));
        }
        
        [Theory]
        [InlineData("UserId")]
        [InlineData("HotelRoomId")]
        [InlineData("StartDate")]
        [InlineData("EndDate")]
        [InlineData("NumberOfGuests")]
        [InlineData("Status")]
        public async Task AddHotelBookingAsync_InvalidDto_ThrowsBusinessValidationException(string field)
        {
            var dto = _fixture.Build<HotelBookingDto>()
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.HotelRoomId, Guid.NewGuid())
                .With(x => x.StartDate, DateTime.UtcNow.AddDays(1))
                .With(x => x.EndDate, DateTime.UtcNow.AddDays(3))
                .With(x => x.NumberOfGuests, 2)
                .With(x => x.Status, Status.Pending)
                .Create();

            switch (field)
            {
                case "UserId": dto.UserId = Guid.Empty; break;
                case "HotelRoomId": dto.HotelRoomId = Guid.Empty; break;
                case "StartDate": dto.StartDate = DateTime.UtcNow.AddDays(-1); break;
                case "EndDate": dto.EndDate = dto.StartDate; break;
                case "NumberOfGuests": dto.NumberOfGuests = 0; break;
                case "Status": dto.Status = (Status)999; break;
            }

            await Assert.ThrowsAsync<BusinessValidationException>(() => _service.AddHotelBookingAsync(dto));
        }

        [Fact]
        public async Task UpdateHotelBookingAsync_NotFound_ThrowsNotFoundException()
        {
            var dto = _fixture.Create<HotelBookingDto>();
            dto.StartDate = DateTime.UtcNow.AddDays(1);
            dto.EndDate = dto.StartDate.AddDays(2);
            dto.HotelRoomId = Guid.NewGuid();
            dto.UserId = Guid.NewGuid();

            var entity = _fixture.Create<HotelBookingEntity>();
            _mapper.Map<HotelBookingEntity>(dto).Returns(entity);
            _unitOfWork.HotelBookings.UpdateHotelBookingAsync(entity).Returns((HotelBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateHotelBookingAsync(dto));
        }

        [Fact]
        public async Task UpdateHotelBookingAsync_ValidDto_ReturnsMappedDto()
        {
            var dto = _fixture.Create<HotelBookingDto>();
            dto.StartDate = DateTime.UtcNow.AddDays(1);
            dto.EndDate = dto.StartDate.AddDays(2);
            dto.HotelRoomId = Guid.NewGuid();
            dto.UserId = Guid.NewGuid();

            var entity = _fixture.Create<HotelBookingEntity>();
            var updatedEntity = _fixture.Create<HotelBookingEntity>();
            var resultDto = _fixture.Create<HotelBookingDto>();

            _mapper.Map<HotelBookingEntity>(dto).Returns(entity);
            _unitOfWork.HotelBookings.UpdateHotelBookingAsync(entity).Returns(updatedEntity);
            _mapper.Map<HotelBookingDto>(updatedEntity).Returns(resultDto);

            var result = await _service.UpdateHotelBookingAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Fact]
        public async Task DeleteHotelBookingAsync_ExistingId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Create<HotelBookingEntity>();
            _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(id).Returns(entity);

            var result = await _service.DeleteHotelBookingAsync(id);

            Assert.True(result);
            await _unitOfWork.HotelBookings.Received(1).DeleteHotelBookingAsync(id);
        }

        [Fact]
        public async Task DeleteHotelBookingAsync_NonExistingId_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(id).Returns((HotelBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteHotelBookingAsync(id));
        }
    }