using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Data
{
    public class ArticleDTO
    {
        public Int32 Id { get; set; }

        public String Title { get; set; }

        public DateTime LastModified { get; set; }

        public String Summary { get; set; }

        public String Content { get; set; }

        public Boolean Lead { get; set; }

        public int UserId { get; set; }

        public ICollection<PictureDTO> Pictures { get; set; }

        public ArticleDTO()
        {
            Pictures = new List<PictureDTO>();
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is ArticleDTO dto) && Id == dto.Id;
        }
    }
}
