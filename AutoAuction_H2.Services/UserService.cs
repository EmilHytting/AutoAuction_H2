using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Persistence;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AutoAuction_H2.Services
{
    public class UserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        // -------------------------
        // CREATE PRIVATE USER
        // -------------------------
        public async Task<(bool success, string? error)> CreatePrivateUserAsync(
            string username, string passwordHash, int zipCode, decimal initialBalance, string cprNumber)
        {
            var request = new
            {
                UserName = username,
                Password = passwordHash, // hash1 fra klient
                ZipCode = zipCode,
                Balance = initialBalance,
                CprNumber = cprNumber,
                UserType = 0,
                CreditLimit = 0m
            };

            var response = await _client.PostAsJsonAsync("api/users", request);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var msg = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(msg) ? "Ugyldigt brugernavn" : msg);

        }

        // -------------------------
        // CREATE CORPORATE USER
        // -------------------------
        public async Task<(bool success, string? error)> CreateCorporateUserAsync(
            string username, string passwordHash, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
        {

            var request = new
            {
                UserName = username,
                Password = passwordHash, // hash1 fra klient
                ZipCode = zipCode,
                Balance = initialBalance,
                CvrNumber = cvrNumber,
                UserType = 1,
                CreditLimit = credit
            };

            var response = await _client.PostAsJsonAsync("api/users", request);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var msg = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(msg) ? "Ugyldigt brugernavn" : msg);

        }

        // -------------------------
        // GET ALL USERS
        // -------------------------
        public async Task<List<UserEntity>?> GetAllUsersAsync()
        {
            return await _client.GetFromJsonAsync<List<UserEntity>>("api/users");
        }

        // -------------------------
        // GET USER BY ID
        // -------------------------
        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _client.GetFromJsonAsync<UserEntity>($"api/users/{id}");
        }

        // -------------------------
        // DELETE USER
        // -------------------------
        public async Task<(bool success, string? error)> DeleteUserAsync(int userId)
        {
            var response = await _client.DeleteAsync($"api/users/{userId}");

            if (response.IsSuccessStatusCode)
                return (true, null);

            var msg = await response.Content.ReadAsStringAsync();
            return (false, msg);
        }
    }
}
