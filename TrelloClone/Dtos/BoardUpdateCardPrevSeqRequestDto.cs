using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class BoardUpdateCardPrevSeqRequestDto
    {
        public int boardSeq { get; set; }
        public int? fromListSeq { get; set; }
        public int? fromSeq { get; set; }
        public int listSeq { get; set; }
        public int seq { get; set; }
        public int? prevSeq { get; set; }
    }
}
