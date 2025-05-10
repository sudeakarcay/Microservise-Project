using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Users.Features.Users
{
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; } // Inherited ID property, ignored in JSON serialization.

      
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

      
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

      
        public bool IsActive { get; set; }

      
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? RegistrationDate { get; set; }

      
        [StringLength(50)]
        public string Surname { get; set; }

        public int RoleId { get; set; }

        public List<int> SkillIds { get; set; }
    }

   
    public class UserCreateHandler : UsersDbHandler, IRequestHandler<UserCreateRequest, CommandResponse>
    {
        public UserCreateHandler(UsersDb db) : base(db)
        {
        }
        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            // Check if a user with the same username or full name already exists
            if (_db.Users.Any(u => u.UserName == request.UserName || (u.FirstName == request.Name && u.LastName == request.Surname)))
                return Error("User with the same user name or full name exists!");

            // Create a new user entity from the request data
            var user = new User()
            {
                IsActive = request.IsActive,
                FirstName = request.Name?.Trim(),
                Password = request.Password,
                RoleId = request.RoleId,
                LastName = request.Surname?.Trim(),
                UserName = request.UserName,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = DateTime.Now.AddDays(AppSettings.RefreshTokenExpirationInDays),
                RegistrationDate = request.RegistrationDate
            };

            // Add the new user to the database
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            // Add skills to the user
            if (request.SkillIds != null && request.SkillIds.Any())
            {
                var userSkills = request.SkillIds.Select(skillId => new UserSkill
                {
                    UserId = user.Id,
                    SkillId = skillId
                }).ToList();
                _db.UserSkills.AddRange(userSkills);
            }

            // Save the changes to the database
            await _db.SaveChangesAsync(cancellationToken);

            return Success("User created successfully.", user.Id);
        }
    }
}