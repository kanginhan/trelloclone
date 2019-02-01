using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Dtos
{
    public class BoardSaveCardRequestDto
    {
        public int boardSeq { get; set; }
        public int listSeq { get; set; }
        public int cardSeq { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
