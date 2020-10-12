using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class ConferenceComparer:IEqualityComparer<Conference>
    {
        public bool Equals(Conference x, Conference y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(Conference obj)
        {
            unchecked
            {
                return ((obj.Id != null ? obj.Id.GetHashCode() : 0) * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
            }
        }
    }
}
