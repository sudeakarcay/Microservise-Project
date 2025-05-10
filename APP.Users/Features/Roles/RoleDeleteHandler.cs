using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Roles
{
    /// <summary>
    /// Represents a request to delete a role.
    /// </summary>
    public class RoleDeleteRequest : Request, IRequest<CommandResponse>
    {
        // No additional properties are required for this request.
    }

    /// <summary>
    /// Handles the deletion of a role in the system.
    /// </summary>
    public class RoleDeleteHandler : UsersDbHandler, IRequestHandler<RoleDeleteRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDeleteHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling role-related operations.</param>
        public RoleDeleteHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to delete a role.
        /// </summary>
        /// <param name="request">The request containing the role ID to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the role deletion operation.</returns>
        public async Task<CommandResponse> Handle(RoleDeleteRequest request, CancellationToken cancellationToken)
        {
            // Retrieve the role with its related users
            var role = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == request.Id);

            // If the role is not found, return an error
            if (role is null)
                return Error("Role not found!");

            // If the role has associated users, it cannot be deleted
            if (role.Users.Count > 0)
                return Error("Role can't be deleted because it has relational users!");

            // Remove the role and save changes
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Role deleted successfully", role.Id);
        }
    }
}