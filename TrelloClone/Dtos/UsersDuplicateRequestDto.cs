using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class UsersDuplicateRequestDto
    {
        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }
    }
}
