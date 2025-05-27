using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using NSubstitute;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Users;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.Tests.ServicesTests;

public class UserServiceTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = _fixture.Freeze<IUnitOfWork>();
        _mapper = _fixture.Freeze<IMapper>();

        _service = new UserService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task AssignUserRoleAsync_UserExists_UpdatesRole()
    {
        var userId = Guid.NewGuid();
        var user = _fixture.Build<UserEntity>().With(u => u.Id, userId).Create();

        _unitOfWork.Users.GetUserByIdAsync(userId).Returns(user);

        var result = await _service.AssignUserRoleAsync(userId, UserRole.Administrator);

        Assert.True(result);
        await _unitOfWork.Users.Received().UpdateUserAsync(user);
        Assert.Equal(UserRole.Administrator, user.Role);
    }

    [Fact]
    public async Task AssignUserRoleAsync_UserNotFound_ThrowsNotFound()
    {
        var userId = Guid.NewGuid();
        _unitOfWork.Users.GetUserByIdAsync(userId).Returns((UserEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.AssignUserRoleAsync(userId, UserRole.Administrator));
    }

    [Fact]
    public async Task DeleteUserAsync_UserExists_DeletesUser()
    {
        var userId = Guid.NewGuid();
        var user = _fixture.Build<UserEntity>().With(u => u.Id, userId).Create();

        _unitOfWork.Users.GetUserByIdAsync(userId).Returns(user);

        var result = await _service.DeleteUserAsync(userId);

        Assert.True(result);
        await _unitOfWork.Users.Received().DeleteUserAsync(userId);
    }

    [Fact]
    public async Task DeleteUserAsync_UserNotFound_ThrowsNotFound()
    {
        var userId = Guid.NewGuid();
        _unitOfWork.Users.GetUserByIdAsync(userId).Returns((UserEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteUserAsync(userId));
    }

    [Fact]
    public async Task GetAllUserAsync_ReturnsMappedUsers()
    {
        var users = _fixture.CreateMany<UserEntity>();
        var dtoList = _fixture.CreateMany<UserDto>();
        _unitOfWork.Users.GetAllUsers().Returns(users);
        _mapper.Map<IEnumerable<UserDto>>(users).Returns(dtoList);

        var result = await _service.GetAllUserAsync();

        Assert.Equal(dtoList, result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_EmptyEmail_ThrowsValidation()
    {
        await Assert.ThrowsAsync<BusinessValidationException>(() => _service.GetUserByEmailAsync(" "));
    }

    [Fact]
    public async Task GetUserByEmailAsync_NotFound_ThrowsNotFound()
    {
        _unitOfWork.Users.GetUserByEmailAsync("test@example.com").Returns((UserEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserByEmailAsync("test@example.com"));
    }

    [Fact]
    public async Task GetUserByEmailAsync_Found_ReturnsDto()
    {
        var entity = _fixture.Create<UserEntity>();
        var dto = _fixture.Create<UserWithBookingsDto>();

        _unitOfWork.Users.GetUserByEmailAsync(entity.Email).Returns(entity);
        _mapper.Map<UserWithBookingsDto>(entity).Returns(dto);

        var result = await _service.GetUserByEmailAsync(entity.Email);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task GetUserByIdAsync_NotFound_ThrowsNotFound()
    {
        var userId = Guid.NewGuid();
        _unitOfWork.Users.GetUserByIdAsync(userId).Returns((UserEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserByIdAsync(userId));
    }

    [Fact]
    public async Task GetUserByIdAsync_Found_ReturnsDto()
    {
        var user = _fixture.Create<UserEntity>();
        var dto = _fixture.Create<UserWithBookingsDto>();

        _unitOfWork.Users.GetUserByIdAsync(user.Id).Returns(user);
        _mapper.Map<UserWithBookingsDto>(user).Returns(dto);

        var result = await _service.GetUserByIdAsync(user.Id);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_InvalidDto_ThrowsValidation()
    {
        var invalidDto = _fixture.Build<UserDto>().With(x => x.Username, "").Create();

        await Assert.ThrowsAsync<BusinessValidationException>(() => _service.UpdateUserProfileAsync(invalidDto));
    }

    [Fact]
    public async Task UpdateUserProfileAsync_UserNotFound_ThrowsNotFound()
    {
        var dto = _fixture.Create<UserDto>();
        var entity = _fixture.Build<UserEntity>().With(x => x.Id, dto.Id).Create();

        _unitOfWork.Users.GetUserByIdAsync(dto.Id).Returns(entity);
        _unitOfWork.Users.UpdateUserAsync(entity).Returns((UserEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateUserProfileAsync(dto));
    }

    [Fact]
    public async Task UpdateUserProfileAsync_Success_ReturnsMappedDto()
    {
        var dto = _fixture.Create<UserDto>();
        var entity = _fixture.Build<UserEntity>().With(x => x.Id, dto.Id).Create();
        var updatedEntity = _fixture.Create<UserEntity>();
        var updatedDto = _fixture.Create<UserDto>();

        _unitOfWork.Users.GetUserByIdAsync(dto.Id).Returns(entity);
        _unitOfWork.Users.UpdateUserAsync(entity).Returns(updatedEntity);
        _mapper.Map<UserDto>(updatedEntity).Returns(updatedDto);

        var result = await _service.UpdateUserProfileAsync(dto);

        Assert.Equal(updatedDto, result);
    }
}