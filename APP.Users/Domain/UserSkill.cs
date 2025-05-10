using CORE.APP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Users.Domain
{
    public class UserSkill : Entity
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int SkillId {  get; set; }
        
        public Skill Skill { get; set; }
    }
}
