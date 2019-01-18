using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class UsersLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        public string PASSWORD { get; set; }
    }
}
