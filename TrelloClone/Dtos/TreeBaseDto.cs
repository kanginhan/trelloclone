using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class TreeBaseDto
    {
        public int seq { get; set; }
        public int? prevSeq { get; set; }
        public int order { get; set; }
    }
}
