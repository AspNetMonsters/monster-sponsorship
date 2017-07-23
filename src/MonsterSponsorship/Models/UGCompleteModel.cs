using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterSponsorship.Models
{
    public class UGCompleteModel
    {
        public string GroupName { get; set; }
        public string StripeToken { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Telephone { get; set; }
    }
}
