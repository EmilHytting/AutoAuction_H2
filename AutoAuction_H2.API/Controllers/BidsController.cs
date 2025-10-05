using AutoAuction_H2.Models.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public BidsController(AuctionDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] BidEntity bidRequest)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == bidRequest.AuctionId);

            if (auction == null)
                return NotFound("Auction not found.");

            if (auction.IsSold || auction.EndTime <= DateTime.UtcNow)
                return BadRequest("Auction is closed.");

            if (bidRequest.Amount <= auction.CurrentBid)
                return BadRequest("Bid must be higher than the current bid.");

            if (bidRequest.Amount < auction.MinPrice)
                return BadRequest("Bid must be at least the minimum price.");

            // (Optional) Check user balance
            var user = await _context.Users.FindAsync(bidRequest.UserId);
            if (user == null)
                return BadRequest("User not found.");
            if (user.Balance < bidRequest.Amount)
                return BadRequest("User does not have enough balance.");

            // Save bid
            var bid = new BidEntity
            {
                AuctionId = bidRequest.AuctionId,
                UserId = bidRequest.UserId,
                Amount = bidRequest.Amount,
                Timestamp = DateTime.UtcNow
            };

            _context.Bids.Add(bid);

            // Update auction
            auction.CurrentBid = bidRequest.Amount;
            auction.HighestBidderId = bidRequest.UserId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Bid placed successfully",
                AuctionId = auction.Id,
                CurrentBid = auction.CurrentBid,
                HighestBidderId = auction.HighestBidderId
            });
        }
    }
}
