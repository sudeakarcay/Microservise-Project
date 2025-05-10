using APP.Users.Domain;
using APP.Users.Features.Roles;
using APP.Users.Features.Skills;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : QueryResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveF { get; set; }

        public string Active { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }

        // Way 1:
        public string RoleName { get; set; }

        // Way 2:
        public RoleQueryResponse Role { get; set; }

        public List<int> SkillIds { get; set; }

        // Way 1:
        public string SkillNames { get; set; }

        // Way 2:
        public List<SkillQueryResponse> Skills { get; set; }
    }

    public class UserQueryHandler : UsersDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Users.Include(u => u.Role)
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .OrderBy(u => u.FirstName)
                .Select(u => new UserQueryResponse()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    FullName = u.FirstName + " " + u.LastName,
                    IsActive = u.IsActive,
                    IsActiveF = u.IsActive ? "Active" : "Inactive",
                    Password = u.Password,
                    RoleName = u.Role.Name,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    RoleId = u.RoleId,
                    SkillIds = u.SkillIds,
                    Skills = u.UserSkills.Select(us => new SkillQueryResponse()
                    {
                        Id = us.Skill.Id,
                        Name = us.Skill.Name
                    }).ToList()
                });
            return Task.FromResult(query);
        }
    }
}