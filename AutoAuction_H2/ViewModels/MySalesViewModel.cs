using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class MySalesViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public MySalesViewModel(AuctionService service) => _auctionService = service;
    }
}
