using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoAuction_H2.Models.Persistence;

namespace AutoAuction_H2.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        // LOGIN
        public async Task<UserEntity?> LoginAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", new { UserName = username, Password = password });
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result == null ? null : new UserEntity
            {
                Id = result.UserId,
                UserName = result.UserName,
                Balance = result.Balance,
                UserType = result.UserType
            };
        }

        // GET AUCTIONS
        public async Task<List<AuctionEntity>> GetAuctionsAsync()
        {
            return await _http.GetFromJsonAsync<List<AuctionEntity>>("api/Auctions") ?? new();
        }

        // PLACE BID
        public async Task<bool> PlaceBidAsync(int auctionId, int userId, decimal amount)
        {
            var response = await _http.PostAsJsonAsync("api/Bids", new { auctionId, userId, amount });
            return response.IsSuccessStatusCode;
        }
    }

    public class LoginResponse
    {
        public string Message { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public decimal Balance { get; set; }
        public int UserType { get; set; }
    }
}
