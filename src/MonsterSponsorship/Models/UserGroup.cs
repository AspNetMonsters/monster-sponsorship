using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterSponsorship.Models
{
    public class UserGroup
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
    }

    public class UserGroupOptions
    {
        public UserGroupOptions()
        {
            UserGroups = new List<UserGroup>();
        }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}
