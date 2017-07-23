using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MonsterSponsorship.Models
{
    public class SponsorshipModel
    {
        [Required]
        [Range(1,25)]
        public int NumberOfEpisodes { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Website { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Tagline { get; set; }

        public string Notes { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Gst { get; set; }
        public decimal Total { get; set; }
        public int StripeAmount { get; set; }
        public string StripeKey { get; set; }
    }
}
