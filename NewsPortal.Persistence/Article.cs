using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NewsPortal.Persistence
{
    public class Article
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public String Summary { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public String Content { get; set; }

        [Required]
        public Boolean Lead { get; set; }

        [DisplayName("Author")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User Author { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}
