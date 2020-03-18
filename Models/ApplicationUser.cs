using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMvc.Models
{
    public class ApplicationUser : IdentityUser
    {
      
        public string First_Name { get; set; }

        public string Last_Name { get; set; }
    }
}
