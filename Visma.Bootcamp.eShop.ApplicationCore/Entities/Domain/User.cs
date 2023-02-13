using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain
{
    public class User
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; } = new Byte[0];
        [Required]
        public byte[] PasswordSalt { get; set; } = new Byte[0];
    }
}