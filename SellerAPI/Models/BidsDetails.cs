using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerAPI.Models
{
    public class BidsDetails
    {
        public Product ProductInfo { get; set; }

        public List<Bids> BidsList { get; set; }
    }
}
