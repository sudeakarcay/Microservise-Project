using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Users.Domain
{
    public class Skill : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<UserSkill> UserSkills { get; set; }

        [NotMapped] //not necessary
        public List<int> UserIds
        {
            get => UserSkills?.Select(us => us.UserId).ToList();
            set => UserSkills = value?.Select(v => new UserSkill() { UserId = v }).ToList();
        }
    }
}
