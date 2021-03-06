﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Dtos
{
    public class BoardSaveListRequestDto
    {
        public int boardSeq { get; set; }
        public int seq { get; set; }
        public string listTitle { get; set; }
        public int? prevSeq { get; set; }
    }
}
