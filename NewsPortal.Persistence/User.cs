using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NewsPortal.Persistence
{
    public class User : IdentityUser<int>
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        /*
        [Required]
        [MaxLength(50)]
        public String UserName { get; set; }

        /*
        [Required]
        [MaxLength(50)]
        public String Password { get; set; }
        */

        public ICollection<Article> Articles { get; set; }
    }
}
