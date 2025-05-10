using Microsoft.EntityFrameworkCore;

namespace APP.Users.Domain
{
    public class UsersDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }

        public DbSet<Role> Roles { get; set; }

        public UsersDb(DbContextOptions options) : base(options) { }

    }
}
