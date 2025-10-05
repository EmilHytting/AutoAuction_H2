using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Services;

public class AuthService
{
    private readonly HttpClient _client;

    public AuthService(HttpClient client)
    {
        _client = client;
    }

    private static string Hash(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        return Convert.ToBase64String(SHA256.HashData(bytes));
    }

    public async Task<(bool success, string? error)> ChangePasswordAsync(string username, string currentPassword, string newPassword)
    {
        var request = new
        {
            Username = username,
            CurrentPasswordHash = Hash(currentPassword),
            NewPasswordHash = Hash(newPassword)
        };

        var response = await _client.PutAsJsonAsync("api/Auth/ChangePassword", request);
        if (response.IsSuccessStatusCode)
            return (true, null);

        var msg = await response.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(msg) ? "Ukendt fejl" : msg);
    }

    public async Task<(bool success, string? error, string? username, decimal balance, int userType, decimal creditLimit)> LoginAsync(string username, string password)
    {
        var request = new
        {
            UserName = username,
            Password = Hash(password)
        };

        var response = await _client.PostAsJsonAsync("api/Auth/login", request);
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            return (false, msg, null, 0, 0, 0);
        }

        var dto = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
        if (dto == null) return (false, "Kunne ikke parse svar", null, 0, 0, 0);

        return (true, null, dto.UserName, dto.Balance, dto.UserType, dto.CreditLimit);
    }

    private record UserLoginResponse(string UserName, decimal Balance, int UserType, decimal CreditLimit);
}
