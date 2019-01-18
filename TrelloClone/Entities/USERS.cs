using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Entities
{
    public class USERS
    {
        [Key]
        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        public string NAME { get; set; }

        [Required]
        public byte[] PASSWORD_HASH { get; set; }

        [Required]
        public byte[] PASSWORD_SALT { get; set; }
    }
}
