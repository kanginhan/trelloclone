using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Entities
{
    public class BOARD
    {
        public string EMAIL { get; set; }

        public int BOARD_SEQ { get; set; }

        public string BOARD_TITLE { get; set; }
    }
}
