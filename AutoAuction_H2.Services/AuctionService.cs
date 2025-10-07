using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Persistence;
using System.Net.Http.Json;

public class AuctionService
{
    private readonly HttpClient _client;

    public AuctionService(HttpClient client)
    {
        _client = client;
    }

    // ---------- Basis ----------
    public async Task<IEnumerable<AuctionEntity>> GetAuctionsAsync()
    {
        var response = await _client.GetAsync("api/auctions");
        if (!response.IsSuccessStatusCode)
            return new List<AuctionEntity>();

        return await response.Content.ReadFromJsonAsync<IEnumerable<AuctionEntity>>() ?? new List<AuctionEntity>();
    }

    public async Task<AuctionEntity?> GetAuctionAsync(int id)
    {
        var response = await _client.GetAsync($"api/auctions/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AuctionEntity>();
    }

    public async Task<(bool success, string? error)> CreateAuctionAsync(AuctionEntity auction)
    {
        var response = await _client.PostAsJsonAsync("api/auctions", auction);
        if (response.IsSuccessStatusCode)
            return (true, null);

        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool success, string? error)> PlaceBidAsync(int auctionId, int userId, decimal amount)
    {
        var request = new { AuctionId = auctionId, UserId = userId, Amount = amount };
        var response = await _client.PostAsJsonAsync("api/bids", request);

        if (response.IsSuccessStatusCode)
            return (true, null);

        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool success, string? error)> CloseAuctionAsync(int auctionId)
    {
        var response = await _client.PutAsync($"api/auctions/{auctionId}/close", null);
        if (response.IsSuccessStatusCode)
            return (true, null);

        return (false, await response.Content.ReadAsStringAsync());
    }

    public async Task<(bool success, string? error)> DeleteAuctionAsync(int auctionId)
    {
        var response = await _client.DeleteAsync($"api/auctions/{auctionId}");
        if (response.IsSuccessStatusCode)
            return (true, null);

        return (false, await response.Content.ReadAsStringAsync());
    }
    public async Task<IEnumerable<AuctionEntity>> GetMyAuctionsAsync(int userId)
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.SellerId == userId);
    }

    public async Task<IEnumerable<AuctionEntity>> GetActiveBidsAsync(int userId)
    {
        var all = await GetAuctionsAsync();
        return all.Where(a =>
            a.Bids.Any(b => b.UserId == userId) &&
            !a.IsSold &&
            a.EndTime > DateTime.UtcNow);
    }

    public async Task<IEnumerable<AuctionEntity>> GetOverbidAuctionsAsync(int userId)
    {
        var all = await GetAuctionsAsync();
        return all.Where(a =>
            a.Bids.Any(b => b.UserId == userId) &&
            a.HighestBidderId != userId &&
            !a.IsSold &&
            a.EndTime > DateTime.UtcNow);
    }

    // ---------- Ekstra filters til Views ----------
    public async Task<IEnumerable<AuctionEntity>> GetPrivateCarsAsync()
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.Vehicle is PrivateCarEntity);
    }

    public async Task<IEnumerable<AuctionEntity>> GetProfessionalCarsAsync()
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.Vehicle is ProfessionalCarEntity);
    }

    public async Task<IEnumerable<AuctionEntity>> GetTrucksAsync()
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.Vehicle is TruckEntity);
    }

    public async Task<IEnumerable<AuctionEntity>> GetBusesAsync()
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.Vehicle is BusEntity);
    }

    public async Task<IEnumerable<AuctionEntity>> GetMySalesAsync(int userId)
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.SellerId == userId);
    }

    public async Task<IEnumerable<AuctionEntity>> GetMyBidsAsync(int userId)
    {
        var all = await GetAuctionsAsync();
        return all.Where(a => a.Bids.Any(b => b.UserId == userId));
    }

}
