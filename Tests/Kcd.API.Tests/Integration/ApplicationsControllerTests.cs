using Kcd.Application.Models;
using Kcd.Common;
using Kcd.Identity.DatabaseContext;
using Kcd.Identity.Models;
using Kcd.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Testcontainers.MsSql;

namespace Kcd.API.Tests.Integration;

[TestFixture]
public class ApplicationsControllerTests
{
    private MsSqlContainer _sqlContainer;
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private IServiceScope _scope;
    private IdentityDatabaseContext _identityContext;
    private UserApplicationDatabaseContext _userApplicationContext;

    [SetUp]
    public async Task SetUp()
    {
        // Start SQL Server container for IdentityDatabaseContext
        _sqlContainer = new MsSqlBuilder()
            .Build();

        await _sqlContainer.StartAsync();

        // Initialize TestClientApplicationFactory with the connection string
        var connectionString = _sqlContainer.GetConnectionString();
        _factory = new TestClientApplicationFactory<Program>(connectionString);

        _client = _factory.CreateClient();

        // Create service scope for direct DB context access
        _scope = _factory.Services.CreateScope();
        _identityContext = _scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
        _userApplicationContext = _scope.ServiceProvider.GetRequiredService<UserApplicationDatabaseContext>();
        CleanupDatabase();
    }

    private async Task<AuthResponse> GetAuthTokenAsync()
    {
        var request = new AuthRequest
        {
            Email = "admin@admin.com",
            Password = Constants.DefaultPassword
        };

        var response = await _client.PostAsync("/api/auth/login",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return authResponse;
    }

    [Test]
    public async Task Login_ShouldReturnJwtToken()
    {
        // Act
        var authResponse = await GetAuthTokenAsync();
        // Assert
        Assert.IsNotNull(authResponse.Token);
    }

    [Test]
    public async Task GetApplications_ShouldReturnOk_WhenAuthorized()
    {
        // Arrange
        var result = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

        var url = "/api/applications";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public async Task Apply_ShouldCreateApplication_WhenAuthorized()
    {
        // Arrange
        var email = "john.doe@example.com";
        var result = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

        var url = "/api/applications";
        var content = new MultipartFormDataContent
            {
                { new StringContent("John Doe"), "Name" },
                { new StringContent(email), "Email" },
                { new StringContent("Netherlands"), "Country" },
                { new StringContent("yellowtail"), "Company " }
            };

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

        // check if application was created 
        var getResponse = await _client.GetAsync("/api/applications");
        getResponse.EnsureSuccessStatusCode();

        // Deserialize the response content to a list of UserApplicationResponse
        var responseBody = await getResponse.Content.ReadAsStringAsync();
        var applications = JsonSerializer.Deserialize<List<UserApplicationResponse>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert - Check if the email exists in the response
        Assert.IsNotNull(applications);
        Assert.IsTrue(applications.Any(a => a.Email == email), $"The application with the email {email} was not found.");
    }

    [TearDown]
    public async Task TearDown()
    {
        await _sqlContainer.DisposeAsync();
        _scope.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }

    private void CleanupDatabase()
    {
        // Clean up SQLite database
        var tables = _userApplicationContext.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
        foreach (var table in tables)
        {
            _userApplicationContext.Database.ExecuteSqlRaw($"DELETE FROM [{table}]");
        }
    }
}