using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Data
{
    public class PictureDTO
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }

        public Byte[] ImageSmall { get; set; }

        public Byte[] ImageLarge { get; set; }

    }
}
