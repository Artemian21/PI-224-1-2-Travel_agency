
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Transports;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;
using NSubstitute;

namespace Travel_agency.Tests.ServicesTests;

public class TicketBookingServiceTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly TicketBookingService _service;

    public TicketBookingServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = _fixture.Freeze<IUnitOfWork>();
        _mapper = _fixture.Freeze<IMapper>();
        _service = new TicketBookingService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task GetAllTicketBookingsAsync_ReturnsMappedDtos()
    {
        var entities = _fixture.CreateMany<TicketBookingEntity>(3);
        var dtos = _fixture.CreateMany<TicketBookingDto>(3);
        _unitOfWork.TicketBookings.GetAllTicketBookingsAsync().Returns(Task.FromResult(entities));
        _mapper.Map<IEnumerable<TicketBookingDto>>(entities).Returns(dtos);

        var result = await _service.GetAllTicketBookingsAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetTicketBookingByIdAsync_ReturnsDto_WhenExists()
    {
        var entity = _fixture.Create<TicketBookingEntity>();
        var dto = _fixture.Create<TicketBookingDetailsDto>();
        var id = Guid.NewGuid();

        _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(id).Returns(Task.FromResult(entity));
        _mapper.Map<TicketBookingDetailsDto>(entity).Returns(dto);

        var result = await _service.GetTicketBookingByIdAsync(id);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task GetTicketBookingByIdAsync_Throws_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(id).Returns(Task.FromResult<TicketBookingEntity>(null));

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetTicketBookingByIdAsync(id));
        Assert.Equal($"Ticket Booking with ID {id} not found.", ex.Message);
    }

    [Fact]
    public async Task AddTicketBookingAsync_ValidInput_ReturnsMappedDto()
    {
        var dto = _fixture.Build<TicketBookingDto>()
            .With(x => x.TransportId, Guid.NewGuid())
            .With(x => x.UserId, Guid.NewGuid())
            .With(x => x.Status, Status.Confirmed)
            .Create();

        var entity = _fixture.Create<TicketBookingEntity>();
        var addedEntity = _fixture.Create<TicketBookingEntity>();
        var expectedDto = _fixture.Create<TicketBookingDto>();

        _mapper.Map<TicketBookingEntity>(dto).Returns(entity);
        _unitOfWork.TicketBookings.AddTicketBookingAsync(entity).Returns(Task.FromResult(addedEntity));
        _mapper.Map<TicketBookingDto>(addedEntity).Returns(expectedDto);

        var result = await _service.AddTicketBookingAsync(dto);

        Assert.Equal(expectedDto, result);
    }

    [Fact]
    public async Task AddTicketBookingAsync_Throws_WhenInvalid()
    {
        var dto = new TicketBookingDto(); // Invalid dto (empty GUIDs)

        await Assert.ThrowsAsync<BusinessValidationException>(() => _service.AddTicketBookingAsync(dto));
    }
    
    [Fact]
    public async Task UpdateTicketBookingAsync_ValidInput_ReturnsUpdatedDto()
    {
        var dto = _fixture.Build<TicketBookingDto>()
            .With(x => x.TransportId, Guid.NewGuid())
            .With(x => x.UserId, Guid.NewGuid())
            .With(x => x.Status, Status.Confirmed)
            .Create();

        var entity = _fixture.Create<TicketBookingEntity>();
        var updatedEntity = _fixture.Create<TicketBookingEntity>();
        var expectedDto = _fixture.Create<TicketBookingDto>();

        _mapper.Map<TicketBookingEntity>(dto).Returns(entity);
        _unitOfWork.TicketBookings.UpdateTicketBookingAsync(entity).Returns(Task.FromResult(updatedEntity));
        _mapper.Map<TicketBookingDto>(updatedEntity).Returns(expectedDto);

        var result = await _service.UpdateTicketBookingAsync(dto);

        Assert.Equal(expectedDto, result);
    }

    [Fact]
    public async Task UpdateTicketBookingAsync_Throws_WhenNotFound()
    {
        var dto = _fixture.Build<TicketBookingDto>()
            .With(x => x.TransportId, Guid.NewGuid())
            .With(x => x.UserId, Guid.NewGuid())
            .With(x => x.Status, Status.Confirmed)
            .Create();

        var entity = _fixture.Create<TicketBookingEntity>();
        _mapper.Map<TicketBookingEntity>(dto).Returns(entity);
        _unitOfWork.TicketBookings.UpdateTicketBookingAsync(entity).Returns(Task.FromResult<TicketBookingEntity>(null));

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateTicketBookingAsync(dto));
        Assert.Equal($"Ticket Booking with ID {dto.Id} not found.", ex.Message);
    }

    [Fact]
    public async Task DeleteTicketBookingAsync_ReturnsTrue_WhenDeleted()
    {
        var id = Guid.NewGuid();
        var entity = _fixture.Create<TicketBookingEntity>();
        _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(id).Returns(Task.FromResult(entity));

        var result = await _service.DeleteTicketBookingAsync(id);

        await _unitOfWork.TicketBookings.Received(1).DeleteTicketBookingAsync(id);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteTicketBookingAsync_Throws_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(id).Returns(Task.FromResult<TicketBookingEntity>(null));

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteTicketBookingAsync(id));
        Assert.Equal($"Ticket Booking with ID {id} not found.", ex.Message);
    }

    [Fact]
    public async Task AddTicketBookingAsync_Throws_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddTicketBookingAsync(null));
    }

    [Fact]
    public async Task UpdateTicketBookingAsync_Throws_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateTicketBookingAsync(null));
    }
}