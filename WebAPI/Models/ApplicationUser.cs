using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}

