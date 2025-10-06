using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoAuction_H2.Models.Entities;

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

    // Login
    public async Task<(bool success, string? error, int userId, string? userName, decimal balance, int userType)>
        LoginAsync(string username, string password)
    {
        // ⚡ Første hash på klienten
        var request = new { UserName = username, Password = Hash(password) };
        var response = await _client.PostAsJsonAsync("api/auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            return (false, msg, 0, null, 0, 0);
        }

        var dto = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return (true, null, dto!.UserId, dto.UserName, dto.Balance, dto.UserType);
    }

    // Change password
    public async Task<(bool success, string? error)> ChangePasswordAsync(int userId, string newPassword)
    {
        // ⚡ Hash én gang på klienten
        var request = new { Password = Hash(newPassword) };

        var response = await _client.PutAsJsonAsync($"api/users/{userId}", request);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var msg = await response.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(msg) ? "Kunne ikke ændre adgangskode" : msg);
    }



    private record LoginResponse(string Message, int UserId, string UserName, decimal Balance, int UserType);
}
