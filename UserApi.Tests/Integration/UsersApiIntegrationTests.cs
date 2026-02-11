using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Models;

namespace UserApi.Tests.Integration
{
    public class UsersApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UsersApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<UserDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            Assert.Empty(users);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users", createUserDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(createdUser);
            Assert.Equal("John Doe", createdUser.Name);
            Assert.Equal("john@example.com", createdUser.Email);
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

            // Act
            var response = await _client.GetAsync($"/api/users/{createdUser!.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal("John Doe", user.Name);
        }

        [Fact]
        public async Task UpdateUser_ReturnsUpdatedUser_WhenUserExists()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

            var updateUserDto = new UpdateUserDto { Name = "John Updated", Email = "john.updated@example.com" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/{createdUser!.Id}", updateUserDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedUser = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(updatedUser);
            Assert.Equal("John Updated", updatedUser.Name);
            Assert.Equal("john.updated@example.com", updatedUser.Email);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent_WhenUserExists()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/users/{createdUser!.Id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_ReturnsHealthyStatus()
        {
            // Act
            var response = await _client.GetAsync("/api/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var health = await response.Content.ReadFromJsonAsync<object>();
            Assert.NotNull(health);
        }
    }
}
