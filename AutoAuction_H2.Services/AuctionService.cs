using AutoAuction_H2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoAuction_H2.Services
{
    public class AuctionService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public AuctionService(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<bool> PlaceBidAsync(int auctionId, int userId, decimal amount)
        {
            try
            {
                var bidRequest = new
                {
                    AuctionId = auctionId,
                    UserId = userId,
                    Amount = amount
                };

                // Brug det rigtige endpoint
                var response = await _httpClient.PostAsJsonAsync($"api/Auctions/{auctionId}/bids", bidRequest);

                if (response.IsSuccessStatusCode)
                    return true;

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"⚠️ PlaceBidAsync fejl: {error}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Exception i PlaceBidAsync: {ex.Message}");
                return false;
            }
        }


        public async Task<List<AuctionEntity>> GetAuctionsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("api/Auctions");
                return JsonSerializer.Deserialize<List<AuctionEntity>>(response, _options) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error fetching auctions: {ex.Message}");
                return new();
            }
        }

        // 👇 NYE METODER 👇

        public async Task<List<AuctionEntity>> GetMyAuctionsAsync(int userId)
        {
            var all = await GetAuctionsAsync();
            return all.Where(a => a.SellerId == userId).ToList();
        }

        public async Task<List<AuctionEntity>> GetActiveBidsAsync(int userId)
        {
            var all = await GetAuctionsAsync();
            return all.Where(a => a.HighestBidderId == userId).ToList();
        }

        public async Task<List<AuctionEntity>> GetOverbidAuctionsAsync(int userId)
        {
            var all = await GetAuctionsAsync();
            return all.Where(a =>
                a.Bids.Any(b => b.UserId == userId) &&
                a.HighestBidderId != userId &&
                !a.IsSold).ToList();
        }
    }
}
