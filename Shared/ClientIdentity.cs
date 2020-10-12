using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Shared
{

    public class ClientIdentity
    {
        public string IdentityProvider { get; set; }
        public string UserId { get; set; }
        public string UserDetails { get; set; }
        public IEnumerable<string> UserRoles { get; set; } = new string[0];

        public bool IsInRole(string role)
        {
            return UserRoles.Contains(role);
        }
    }
}