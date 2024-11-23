using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Dto
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Description { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
