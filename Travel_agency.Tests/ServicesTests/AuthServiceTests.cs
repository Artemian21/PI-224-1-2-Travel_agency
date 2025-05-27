using System.ComponentModel.DataAnnotations;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Travel_agency.BLL.Abstractions;
using Travel_agency.DataAccess.Abstraction;
using AutoMapper;
using NSubstitute;
using Travel_agency.BLL.Services;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Users;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.Tests.ServicesTests;

public class AuthServiceTests
{
    private readonly IFixture _fixture;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJWTProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _userRepository = _fixture.Freeze<IUserRepository>();
        _passwordHasher = _fixture.Freeze<IPasswordHasher>();
        _jwtProvider = _fixture.Freeze<IJWTProvider>();
        _mapper = _fixture.Freeze<IMapper>();

        _authService = new AuthService(_userRepository, _passwordHasher, _jwtProvider, _mapper);
    }

    [Fact]
    public async Task Register_ThrowsArgumentNullException_WhenInputIsNull()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.Register(null));
    }

    [Fact]
    public async Task Register_ThrowsConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = _fixture.Create<RegisterUserDto>();
        _userRepository.GetUserByEmailAsync(dto.Email).Returns(new UserEntity());

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _authService.Register(dto));
    }

    [Fact]
    public async Task Register_ThrowsValidationException_WhenPasswordIsWeak()
    {
        // Arrange
        var dto = _fixture.Build<RegisterUserDto>()
                          .With(x => x.Password, "weak")
                          .Create();
        _userRepository.GetUserByEmailAsync(dto.Email).Returns((UserEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _authService.Register(dto));
    }

    [Fact]
    public async Task Register_ReturnsUserDto_WhenSuccessful()
    {
        // Arrange
        var dto = _fixture.Build<RegisterUserDto>()
                          .With(x => x.Password, "StrongPass1!")
                          .Create();
        _userRepository.GetUserByEmailAsync(dto.Email).Returns((UserEntity)null);

        var userEntity = _fixture.Build<UserEntity>()
                                 .With(x => x.Email, dto.Email)
                                 .With(x => x.Username, dto.Username)
                                 .Create();

        _passwordHasher.GenerateHash(dto.Password).Returns("hashedPassword");
        _userRepository.AddUserAsync(Arg.Any<UserEntity>()).Returns(userEntity);
        var userDto = _fixture.Create<UserDto>();
        _mapper.Map<UserDto>(userEntity).Returns(userDto);

        // Act
        var result = await _authService.Register(dto);

        // Assert
        Assert.Equal(userDto, result);
    }

    [Fact]
    public async Task Login_ThrowsBusinessValidationException_WhenEmailOrPasswordIsEmpty()
    {
        await Assert.ThrowsAsync<BusinessValidationException>(() => _authService.Login(null, "pass"));
        await Assert.ThrowsAsync<BusinessValidationException>(() => _authService.Login("", "pass"));
        await Assert.ThrowsAsync<BusinessValidationException>(() => _authService.Login("email", null));
        await Assert.ThrowsAsync<BusinessValidationException>(() => _authService.Login("email", ""));
    }

    [Fact]
    public async Task Login_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        _userRepository.GetUserByEmailAsync(Arg.Any<string>()).Returns((UserEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _authService.Login("test@example.com", "Password1!"));
    }

    [Fact]
    public async Task Login_ThrowsUnauthorizedAccessException_WhenPasswordInvalid()
    {
        // Arrange
        var user = _fixture.Create<UserEntity>();
        _userRepository.GetUserByEmailAsync(user.Email).Returns(user);
        _passwordHasher.VerifyHash(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
        
// Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Login(user.Email, "wrongPass"));
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenSuccessful()
    {
        // Arrange
        var user = _fixture.Create<UserEntity>();
        var userDto = _fixture.Create<UserDto>();
        var token = _fixture.Create<string>();

        _userRepository.GetUserByEmailAsync(user.Email).Returns(user);
        _passwordHasher.VerifyHash(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _mapper.Map<UserDto>(user).Returns(userDto);
        _jwtProvider.GenerateToken(userDto).Returns(token);

        // Act
        var result = await _authService.Login(user.Email, "ValidPass1!");

        // Assert
        Assert.Equal(token, result);
    }
}