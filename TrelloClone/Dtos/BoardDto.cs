using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Entities;

namespace TrelloClone.Dtos
{
    public class BoardDto
    {
        public string hashId { get; set; }
        public int boardSeq { get; set; }
        public string boardTitle { get; set; }
        public bool isPublic { get; set; }
        public bool canEditing { get; set; }
        public IEnumerable<CardListResultDto> cardLists { get; set; }
    }

    public class CardListResultDto : TreeBaseDto
    {
        public string listTitle { get; set; }
        public IEnumerable<CardResultDto> cards { get; set; }
    }

    public class CardResultDto : TreeBaseDto
    {
        public string title { get; set; }
        public string description { get; set; }
    }
}
