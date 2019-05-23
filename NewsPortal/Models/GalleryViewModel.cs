using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPortal.WebSite.Models
{
    public class GalleryViewModel
    {
        public int ArticleId { get; set; }
        public IList<int> PictureIds { get; set; }
    }
}
