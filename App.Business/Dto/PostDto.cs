using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Dto
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int UserId { get; set; }
        public List<CommentDto> Comments { get; set; }
        public List<LikeDto> Likes { get; set; }
        public int LikesCount { get; set; }
    }
}
