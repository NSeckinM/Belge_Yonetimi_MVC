using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabim_Proje.Data
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required,MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(50)]
        public string UserLastName { get; set; }

        public string Explanation { get; set; }

        [Required(ErrorMessage ="This field is required and only .pdf extension document type  is acceptable !")]
        public string Document { get; set; }

        public bool ConsiderationStatus { get; set; }

        public DateTime TimeOfConsideration { get; set; }


        //Nav props

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
