using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class PrivateCarsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public PrivateCarsViewModel(AuctionService service) => _auctionService = service;
    }

}
