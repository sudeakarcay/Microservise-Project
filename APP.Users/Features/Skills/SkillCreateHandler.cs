using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Users.Features.Skills
{
    /// <summary>
    /// Represents a request to create a new skill.
    /// </summary>
    public class SkillCreateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets or sets the ID of the skill.
        /// </summary>
        /// <remarks>
        /// The ID property is inherited from the base class and is ignored during JSON serialization.
        /// </remarks>
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// Gets or sets the name of the skill.
        /// </summary>
        /// <remarks>
        /// The name is required and must have a maximum length of 125 characters.
        /// </remarks>
        [Required, StringLength(125)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new skill in the system.
    /// </summary>
    public class SkillCreateHandler : UsersDbHandler, IRequestHandler<SkillCreateRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillCreateHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling skill-related operations.</param>
        public SkillCreateHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to create a new skill.
        /// </summary>
        /// <param name="request">The request containing the skill information to be created.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the skill creation operation.</returns>
        public async Task<CommandResponse> Handle(SkillCreateRequest request, CancellationToken cancellationToken)
        {
            // Check if a skill with the same name already exists
            if (_db.Skills.Any(s => s.Name == request.Name))
                return Error("Skill with the same name exists!");

            // Create a new skill entity
            var skill = new Skill()
            {
                Name = request.Name?.Trim()
            };

            // Add the new skill to the database and save changes
            _db.Skills.Add(skill);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Skill created successfully.", skill.Id);
        }
    }
}