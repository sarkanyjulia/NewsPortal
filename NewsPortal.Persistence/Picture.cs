using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NewsPortal.Persistence
{
    public class Picture
    {
        [Key]
        public Int32 Id { get; set; }

        [DisplayName("Article")]
        public int ArticleId { get; set; }

        public byte[] ImageSmall { get; set; }

        public byte[] ImageLarge { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
