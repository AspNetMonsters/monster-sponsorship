using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterSponsorship.Models
{
    public class CompleteModel
    {
        public string StripeToken { get; set; }
        
        public int NumberOfEpisodes{ get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Tagline { get; set; }
        public string Notes { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Gst { get; set; }
        public decimal Total { get; set; }

        public int StripeAmount { get; set; }
    }
}
