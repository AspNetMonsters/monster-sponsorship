using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterSponsorship
{
    public class BusinessOptions
    {
        public string GstNumber { get; set; }    
        public decimal PricePerEpisode { get; set; }

        //TODO: Add tax rate here instead of hardcoding it all over
    }
}
