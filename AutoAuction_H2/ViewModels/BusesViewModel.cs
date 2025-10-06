using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class BusesViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public BusesViewModel(AuctionService service) => _auctionService = service;
    }

}
