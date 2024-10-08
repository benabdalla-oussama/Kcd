﻿using Bogus;
using Kcd.Application.Models;
using Kcd.Common;
using Kcd.Domain;
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
    private Faker<UserApplication> _userApplicationFaker;

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

        // Initialize Faker for generating test data
        _userApplicationFaker = new Faker<UserApplication>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(u => u.Company, f => f.Company.CompanyName());
    }

    private async Task<AuthResponse> GetAuthTokenAsync()
    {
        var request = new AuthRequest
        {
            Email = Constants.DefaultAdminEmail,
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
        var userApplication = _userApplicationFaker.Generate();

        var url = "/api/applications";
        var content = new MultipartFormDataContent
        {
            { new StringContent(userApplication.Name), "Name" },
            { new StringContent(userApplication.Email), "Email" },
            { new StringContent(userApplication.Country), "Country" },
            { new StringContent(userApplication.Company), "Company" }
        };

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

        // Check if application was created 
        var result = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

        var getResponse = await _client.GetAsync("/api/applications");
        getResponse.EnsureSuccessStatusCode();

        // Deserialize the response content to a list of UserApplicationResponse
        var responseBody = await getResponse.Content.ReadAsStringAsync();
        var applications = JsonSerializer.Deserialize<List<UserApplicationResponse>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert - Check if the application with the generated email exists in the response
        Assert.IsNotNull(applications);
        Assert.IsTrue(applications.Any(a => a.Email == userApplication.Email), $"The application with the email {userApplication.Email} was not found.");
    }

    [Test]
    public async Task EndToEnd_ApplyApproveAndLogin_ShouldSucceed()
    {
        // Step 1: Apply for a new user application
        var userApplication = _userApplicationFaker.Generate();

        var applyUrl = "/api/applications";
        var content = new MultipartFormDataContent
        {
            { new StringContent(userApplication.Name), "Name" },
            { new StringContent(userApplication.Email), "Email" },
            { new StringContent(userApplication.Country), "Country" },
            { new StringContent(userApplication.Company), "Company" }
        };

        // Act 1: Post application
        var applyResponse = await _client.PostAsync(applyUrl, content);
        applyResponse.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.Created, applyResponse.StatusCode);

        // Step 2: Approve the application using admin token
        var adminToken = await GetAuthTokenAsync(); // Get the admin token
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken.Token);
        var applications = await _client.GetAsync("/api/applications");
        applications.EnsureSuccessStatusCode();

        var responseBody = await applications.Content.ReadAsStringAsync();
        var applicationList = JsonSerializer.Deserialize<List<UserApplicationResponse>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Find the newly created application
        var createdApplication = applicationList.FirstOrDefault(a => a.Email == userApplication.Email);
        Assert.IsNotNull(createdApplication, "The created application was not found.");

        // Approve the application
        var approveUrl = $"/api/applications/approve/{createdApplication.Id}";
        var approveResponse = await _client.PutAsync(approveUrl, null);
        approveResponse.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, approveResponse.StatusCode);

        // Step 3: Authenticate with the new user's credentials
        var newUserCredentials = new AuthRequest
        {
            Email = userApplication.Email,
            Password = Constants.DefaultPassword // Assuming the user application password is the default
        };

        var loginUrl = "/api/auth/login";
        var loginContent = new StringContent(JsonSerializer.Serialize(newUserCredentials), Encoding.UTF8, "application/json");

        var loginResponse = await _client.PostAsync(loginUrl, loginContent);

        // Act 3: Check if login was successful
        loginResponse.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<AuthResponse>(loginResponseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert that the new user received a valid token
        Assert.IsNotNull(loginResult.Token, "New user login failed or no token returned.");
    }

    [TearDown]
    public async Task TearDown()
    {
        try
        {
            await _sqlContainer.DisposeAsync();
            _scope.Dispose();
            _client.Dispose();
            _factory.Dispose();
        }
        catch // Ignore dispose issues.
        {
        }
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