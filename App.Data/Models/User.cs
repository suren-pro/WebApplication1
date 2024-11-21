using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Models
{
    [Index(nameof(Username),IsUnique = true)]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        List<Post>  Posts { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
