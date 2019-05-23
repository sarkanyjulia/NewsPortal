﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Data
{
    public class ArticleListElement
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String AuthorName { get; set; }
        public DateTime LastModified { get; set; }
    }
}
