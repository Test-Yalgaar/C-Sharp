using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GNM.Models
{
    public class User
    {
        public User()
        {
            GatewayData = new List<Gateway>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage="UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Paassword is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "EmailAddress is required")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage="Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Conatct No. is required")]
        [StringLength(15, ErrorMessage = "Cannot be longer than 10 characters")]
        public string ContactNo { get; set; }

        public DateTime? PasswordChangeOn { get; set; }

        public IList<Gateway> GatewayData { get; set; }
    }
}