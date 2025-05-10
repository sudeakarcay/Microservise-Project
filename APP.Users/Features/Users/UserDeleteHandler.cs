using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    /// <summary>
    /// Represents a request to delete a user.
    /// </summary>
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    /// <summary>
    /// Handles the deletion of a user from the system.
    /// </summary>
    public class UserDeleteHandler : UsersDbHandler, IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeleteHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling user-related operations.</param>
        public UserDeleteHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to delete a user.
        /// </summary>
        /// <param name="request">The request containing the information for the user to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the user deletion operation.</returns>
        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            // Retrieve the user from the database, including their associated user skills
            var user = await _db.Users.Include(u => u.UserSkills).SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            // Check if the user exists
            if (user is null)
                return Error("User not found!");

            // Remove the user's skills from the UserSkills table
            _db.UserSkills.RemoveRange(user.UserSkills);

            // Remove the user from the Users table
            _db.Users.Remove(user);

            // Save the changes to the database
            await _db.SaveChangesAsync(cancellationToken);

            return Success("User deleted successfully", user.Id);
        }
    }
}