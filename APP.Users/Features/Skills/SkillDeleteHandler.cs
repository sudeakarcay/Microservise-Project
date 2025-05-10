using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Skills
{
    /// <summary>
    /// Represents a request to delete a skill from the system.
    /// </summary>
    public class SkillDeleteRequest : Request, IRequest<CommandResponse>
    {
        // No additional properties are required for this request.
    }

    /// <summary>
    /// Handles the deletion of a skill from the system.
    /// </summary>
    public class SkillDeleteHandler : UsersDbHandler, IRequestHandler<SkillDeleteRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillDeleteHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling skill-related operations.</param>
        public SkillDeleteHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to delete a skill from the system.
        /// </summary>
        /// <param name="request">The request containing the skill ID to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the skill deletion operation.</returns>
        public async Task<CommandResponse> Handle(SkillDeleteRequest request, CancellationToken cancellationToken)
        {
            // Find the skill by ID, including its associated user skills
            var skill = _db.Skills.Include(s => s.UserSkills).SingleOrDefault(s => s.Id == request.Id);

            // If the skill is not found, return an error response
            if (skill is null)
                return Error("Skill not found!");

            // Remove associated user skills before deleting the skill
            _db.UserSkills.RemoveRange(skill.UserSkills);
            _db.Skills.Remove(skill);

            // Save the changes to the database
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Skill deleted successfully", skill.Id);
        }
    }
}