using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tabim_Proje.Models
{
    public class RequestEvaluateVM
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(50)]
        public string UserLastName { get; set; }

        public string Explanation { get; set; }

        public string ResultOfConsideration { get; set; }

        public bool ConsiderationStatus { get; set; }

        public DateTime TimeOfConsideration { get; set; }
    }
}
