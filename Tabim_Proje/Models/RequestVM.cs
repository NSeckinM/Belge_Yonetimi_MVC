using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tabim_Proje.Models
{
    public class RequestVM
    {

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(50)]
        public string UserLastName { get; set; }

        public string Explanation { get; set; }

        [Required]
        public string Document { get; set; }

        public IFormFile File { get; set; }

    }
}
