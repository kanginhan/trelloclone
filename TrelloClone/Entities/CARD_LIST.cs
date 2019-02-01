using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Entities
{
    public class CARD_LIST
    {
        public string EMAIL { get; set; }

        public int BOARD_SEQ { get; set; }

        public int LIST_SEQ { get; set; }

        public string LIST_TITLE { get; set; }
    }
}
