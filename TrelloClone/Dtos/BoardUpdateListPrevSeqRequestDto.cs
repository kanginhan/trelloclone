using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class BoardUpdateListPrevSeqRequestDto
    {
        public int boardSeq { get; set; }
        public int seq { get; set; }
        public int? prevSeq { get; set; }
    }
}
