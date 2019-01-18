using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class UsersRegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        public string NAME { get; set; }

        [Required]
        public string PASSWORD { get; set; }
    }
}
