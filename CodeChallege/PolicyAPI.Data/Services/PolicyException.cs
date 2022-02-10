using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyAPI.Data
{
    public enum Reason { General, ThirtyDays, Classic, AddressFormat, StateUnhappy, ExpireDate, Premium}
    public class PolicyException : ApplicationException
    {
        public PolicyException(string msg) : base(msg) { }
        public Reason ErrorCode { get; set; }
    }
}
