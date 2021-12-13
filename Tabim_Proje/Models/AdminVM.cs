using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabim_Proje.Data;

namespace Tabim_Proje.Models
{
    public class AdminVM
    {
        public List<UserRequest> AllRequest { get; set; }
        public List<UserRequest> Approved { get; set; }
        public List<UserRequest> Unapproved { get; set; }
    }
}
