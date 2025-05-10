using CORE.APP.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Users.Domain
{
    public class Role : Entity
    {
        [Required, StringLength(10)]
        public string Name { get; set; }

        public List<User>  Users { get; set; } = new List<User>(); //making sure that it will not be null
    }
}
