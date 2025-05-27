using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using NSubstitute;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Tours;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.Tests.ServicesTests;

    public class TourBookingServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TourBookingService _service;

        public TourBookingServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors.Clear();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork = _fixture.Freeze<IUnitOfWork>();
            _mapper = _fixture.Freeze<IMapper>();
            _service = new TourBookingService(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task GetAllTourBookingsAsync_ReturnsMappedDtos()
        {
            var entities = _fixture.CreateMany<TourBookingEntity>();
            var dtos = _fixture.CreateMany<TourBookingDto>();

            _unitOfWork.TourBookings.GetAllTourBookingsAsync().Returns(entities);
            _mapper.Map<IEnumerable<TourBookingDto>>(entities).Returns(dtos);

            var result = await _service.GetAllTourBookingsAsync();

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetTourBookingByIdAsync_ValidId_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Create<TourBookingEntity>();
            var dto = _fixture.Create<TourBookingDetailsDto>();

            _unitOfWork.TourBookings.GetTourBookingByIdAsync(id).Returns(entity);
            _mapper.Map<TourBookingDetailsDto>(entity).Returns(dto);

            var result = await _service.GetTourBookingByIdAsync(id);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetTourBookingByIdAsync_NotFound_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.TourBookings.GetTourBookingByIdAsync(id).Returns((TourBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetTourBookingByIdAsync(id));
        }

        [Fact]
        public async Task AddTourBookingAsync_ValidDto_ReturnsMappedDto()
        {
            var dto = _fixture.Build<TourBookingDto>()
                .With(x => x.TourId, Guid.NewGuid())
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.Status, Status.Confirmed)
                .Create();

            var entity = _fixture.Create<TourBookingEntity>();
            var savedEntity = _fixture.Create<TourBookingEntity>();
            var resultDto = _fixture.Create<TourBookingDto>();

            _mapper.Map<TourBookingEntity>(dto).Returns(entity);
            _unitOfWork.TourBookings.AddTourBookingAsync(entity).Returns(savedEntity);
            _mapper.Map<TourBookingDto>(savedEntity).Returns(resultDto);

            var result = await _service.AddTourBookingAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Theory]
        [InlineData(null)]
        public async Task AddTourBookingAsync_NullDto_ThrowsArgumentNullException(TourBookingDto dto)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddTourBookingAsync(dto));
        }

        [Fact]
        public async Task AddTourBookingAsync_EmptyTourId_ThrowsBusinessValidationException()
        {
            var dto = _fixture.Build<TourBookingDto>()
                .With(x => x.TourId, Guid.Empty)
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.Status, Status.Confirmed)
                .Create();

            await Assert.ThrowsAsync<BusinessValidationException>(() => _service.AddTourBookingAsync(dto));
        }

        [Fact]
        public async Task AddTourBookingAsync_InvalidStatus_ThrowsBusinessValidationException()
        {
            var dto = _fixture.Build<TourBookingDto>()
                .With(x => x.TourId, Guid.NewGuid())
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.Status, (Status)999)
                .Create();

            await Assert.ThrowsAsync<BusinessValidationException>(() => _service.AddTourBookingAsync(dto));
        }

        [Fact]
        public async Task UpdateTourBookingAsync_ValidDto_ReturnsMappedDto()
        {
            var dto = _fixture.Build<TourBookingDto>()
                .With(x => x.TourId, Guid.NewGuid())
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.Status, Status.Confirmed)
                .Create();

            var entity = _fixture.Create<TourBookingEntity>();
            var updatedEntity = _fixture.Create<TourBookingEntity>();
            var resultDto = _fixture.Create<TourBookingDto>();

            _mapper.Map<TourBookingEntity>(dto).Returns(entity);
            _unitOfWork.TourBookings.UpdateTourBookingAsync(entity).Returns(updatedEntity);
            _mapper.Map<TourBookingDto>(updatedEntity).Returns(resultDto);

            var result = await _service.UpdateTourBookingAsync(dto);

            Assert.Equal(resultDto, result);
        }

        [Fact]
        public async Task UpdateTourBookingAsync_NotFound_ThrowsNotFoundException()
        {
            var dto = _fixture.Build<TourBookingDto>()
                .With(x => x.TourId, Guid.NewGuid())
                .With(x => x.UserId, Guid.NewGuid())
                .With(x => x.Status, Status.Confirmed)
                .Create();

            var entity = _fixture.Create<TourBookingEntity>();
            _mapper.Map<TourBookingEntity>(dto).Returns(entity);
            _unitOfWork.TourBookings.UpdateTourBookingAsync(entity).Returns((TourBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateTourBookingAsync(dto));
        }

        [Fact]
        public async Task DeleteTourBookingAsync_ValidId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Create<TourBookingEntity>();
            _unitOfWork.TourBookings.GetTourBookingByIdAsync(id).Returns(entity);

            var result = await _service.DeleteTourBookingAsync(id);

            Assert.True(result);
            await _unitOfWork.TourBookings.Received(1).DeleteTourBookingAsync(id);
        }

        [Fact]
        public async Task DeleteTourBookingAsync_NotFound_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWork.TourBookings.GetTourBookingByIdAsync(id).Returns((TourBookingEntity)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteTourBookingAsync(id));
        }
    }
