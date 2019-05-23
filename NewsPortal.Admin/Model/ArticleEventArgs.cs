using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Admin.Model
{
    public class ArticleEventArgs : EventArgs
    {
        public Int32 ArticleId { get; set; }
    }
}
