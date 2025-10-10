using AutoAuction_H2.Models.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public AuctionsController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: api/auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionEntity>>> GetAuctions()
        {
            return await _context.Auctions
                .Include(a => a.Vehicle)
                .Include(a => a.Seller)
                .Include(a => a.HighestBidder)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        // GET: api/auctions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionEntity>> GetAuction(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Vehicle)
                .Include(a => a.Seller)
                .Include(a => a.HighestBidder)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            return auction;
        }


        [HttpGet("user/{userId}/bids")]
        public async Task<ActionResult<IEnumerable<AuctionEntity>>> GetMyBids(int userId)
        {
            var auctions = await _context.Auctions
                .Include(a => a.Vehicle)
                .Include(a => a.Seller)
                .Include(a => a.HighestBidder)
                .Include(a => a.Bids)              // ✅ ensures bids are loaded
                .Where(a => a.Bids.Any(b => b.UserId == userId))
                .ToListAsync();

            return auctions;
        }

        // POST: api/auctions
        [HttpPost]
        public async Task<ActionResult<AuctionEntity>> CreateAuction(AuctionEntity auction)
        {
            // Ensure seller exists
            var seller = await _context.Users.FindAsync(auction.SellerId);
            if (seller == null)
                return BadRequest("Seller does not exist.");

            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuction), new { id = auction.Id }, auction);
        }

        // POST: api/auctions/{id}/bids
        // POST: api/auctions/{id}/bids
        [HttpPost("{id}/bids")]
        public async Task<ActionResult<UserEntity>> PlaceBid(int id, [FromBody] BidEntity bid)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound("Auction not found.");

            var user = await _context.Users.FindAsync(bid.UserId);
            if (user == null)
                return BadRequest("User not found.");

            if (bid.Amount <= auction.CurrentBid || bid.Amount < auction.MinPrice)
                return BadRequest("Bid must be higher than current and at least min price.");

            // Frigiv tidligere højeste bud
            if (auction.HighestBidderId.HasValue && auction.HighestBidderId != bid.UserId)
            {
                var oldBidder = await _context.Users.FindAsync(auction.HighestBidderId);
                if (oldBidder != null)
                    oldBidder.ReservedAmount -= auction.CurrentBid;
            }

            if (user.AvailableBalance < bid.Amount)
                return BadRequest("Not enough balance.");

            user.ReservedAmount += bid.Amount;

            auction.CurrentBid = bid.Amount;
            auction.HighestBidderId = bid.UserId;
            bid.AuctionId = auction.Id;
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            // 👇 Returnér kun den opdaterede bruger (uden passwordhash)
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Balance,
                user.ReservedAmount
            });
        }





        // PUT: api/auctions/{id}/close
        [HttpPut("{id}/close")]
        public async Task<IActionResult> CloseAuction(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.HighestBidder)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound("Auction not found.");

            if (auction.IsSold)
                return BadRequest("Auction is already closed.");

            if (auction.CurrentBid >= auction.MinPrice && auction.HighestBidderId != null)
            {
                var winner = auction.HighestBidder;
                var seller = auction.Seller;

                if (winner == null || seller == null)
                    return BadRequest("Auction must have valid seller and highest bidder.");

                // Træk betaling fra vinderens reservering
                if (winner.ReservedAmount >= auction.CurrentBid)
                    winner.ReservedAmount -= auction.CurrentBid;

                winner.Balance -= auction.CurrentBid;

                // Udbetal til sælger
                seller.Balance += auction.CurrentBid;

                auction.IsSold = true;
            }
            else
            {
                // Ingen vinder → bare luk auktionen uden betaling
                auction.IsSold = true;
            }

            await _context.SaveChangesAsync();
            return Ok(auction);
        }


        // DELETE: api/auctions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
                return NotFound();

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
