using UserApi.Services;
using UserApi.Repositories;
using UserApi.DTOs;
using UserApi.Models;
using Moq;

namespace UserApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsCreatedUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
            var createdUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsUpdatedUser_WhenUserExists()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Name = "John Updated", Email = "john.updated@example.com" };
            var existingUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var updatedUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingUser);
            _mockRepository.Setup(r => r.UpdateAsync(1, It.IsAny<User>())).ReturnsAsync(updatedUser);

            // Act
            var result = await _userService.UpdateUserAsync(1, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Updated", result.Name);
            Assert.Equal("john.updated@example.com", result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Name = "John Updated", Email = "john.updated@example.com" };
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.UpdateUserAsync(999, updateUserDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUserAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUserAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}
