using AutoAuction_H2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Services
{
    public class AuctionService
    {
        private readonly HttpClient _http = new();

        public async Task<List<Auction>> GetAuctionsAsync()
        {
            var res = await _http.GetFromJsonAsync<List<Auction>>("https://localhost:5001/api/auctions");
            return res ?? new List<Auction>();
        }

        public async Task<bool> PlaceBidAsync(int auctionId, decimal bid, int userId)
        {
            var response = await _http.PostAsJsonAsync(
                $"https://localhost:5001/api/auctions/{auctionId}/bid",
                new { UserId = userId, Amount = bid }
            );
            return response.IsSuccessStatusCode;
        }
    }

}
