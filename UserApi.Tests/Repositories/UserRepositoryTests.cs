using UserApi.Models;
using UserApi.Repositories;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace UserApi.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly UserDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new UserDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            await _context.Users.AddRangeAsync(
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = (short)1, Name = "John Doe", Email = "john@example.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedUser()
        {
            // Arrange
            var user = new User { Name = "John Doe", Email = "john@example.com" };

            // Act
            var result = await _repository.CreateAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = (short)1, Name = "John Doe", Email = "john@example.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var updatedUser = new User { Id = (short)1, Name = "John Updated", Email = "john.updated@example.com" };

            // Act
            var result = await _repository.UpdateAsync(1, updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Updated", result.Name);
            Assert.Equal("john.updated@example.com", result.Email);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User { Id = (short)999, Name = "John Doe", Email = "john@example.com" };

            // Act
            var result = await _repository.UpdateAsync(999, user);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = (short)1, Name = "John Doe", Email = "john@example.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            var deletedUser = await _context.Users.FindAsync((short)1);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
