using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Entities
{
    public class CARD
    {
        public string EMAIL { get; set; }

        public int BOARD_SEQ { get; set; }

        public int LIST_SEQ { get; set; }

        public int CARD_SEQ { get; set; }

        public string CARD_TITLE { get; set; }

        public string DESCRIPTION { get; set; }

        public int? PREV_CARD { get; set; }
    }
}
