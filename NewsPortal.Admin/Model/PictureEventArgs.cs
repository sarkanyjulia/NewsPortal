﻿using NewsPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Admin.Model
{
    public class PictureEventArgs : EventArgs
    {
        public PictureDTO Picture { get; set; }
    }
}
