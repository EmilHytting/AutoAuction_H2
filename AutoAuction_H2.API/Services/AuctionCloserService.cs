using AutoAuction_H2.Models.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.API.Services
{
    public class AuctionCloserService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AuctionCloserService> _logger;

        public AuctionCloserService(IServiceScopeFactory scopeFactory, ILogger<AuctionCloserService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

                    var now = DateTime.UtcNow;

                    // Find aktive auktioner der er udløbet
                    var expired = await db.Auctions
                        .Include(a => a.Seller)
                        .Include(a => a.HighestBidder)
                        .Where(a => !a.IsSold && a.EndTime <= now)
                        .ToListAsync(stoppingToken);

                    foreach (var auction in expired)
                    {
                        if (auction.CurrentBid >= auction.MinPrice && auction.HighestBidderId != null)
                        {
                            var winner = auction.HighestBidder;
                            var seller = auction.Seller;

                            if (winner != null && seller != null)
                            {
                                if (winner.ReservedAmount >= auction.CurrentBid)
                                    winner.ReservedAmount -= auction.CurrentBid;

                                winner.Balance -= auction.CurrentBid;
                                seller.Balance += auction.CurrentBid;

                                auction.IsSold = true;
                                _logger.LogInformation($"Auction {auction.Id} closed. Winner {winner.UserName} paid {auction.CurrentBid:N0} kr to seller {seller.UserName}");
                            }
                        }
                        else
                        {
                            auction.IsSold = true; // Lukket uden vinder
                            _logger.LogInformation($"Auction {auction.Id} closed without a winner.");
                        }
                    }

                    if (expired.Any())
                        await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error closing auctions automatically");
                }

                // Vent 1 minut før næste tjek
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
