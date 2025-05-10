
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Roles
{
    /// <summary>
    /// Represents a request to update an existing role.
    /// </summary>
    public class RoleUpdateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <remarks>
        /// The name is required and must have a maximum length of 10 characters.
        /// </remarks>
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Handles the update of an existing role in the system.
    /// </summary>
    public class RoleUpdateHandler : UsersDbHandler, IRequestHandler<RoleUpdateRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUpdateHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling role-related operations.</param>
        public RoleUpdateHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to update an existing role.
        /// </summary>
        /// <param name="request">The request containing the role ID and the updated role name.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the role update operation.</returns>
        public async Task<CommandResponse> Handle(RoleUpdateRequest request, CancellationToken cancellationToken)
        {
            // Check if the updated role name already exists (excluding the current role)
            if (_db.Roles.Any(r => r.Id != request.Id && r.Name == request.Name))
                return Error("Role with the same name exists!");

            // Find the role by ID
            var role = _db.Roles.Find(request.Id);
            if (role is null)
                return Error("Role not found!");

            // Update the role's name and save changes
            role.Name = request.Name?.Trim();
            _db.Roles.Update(role);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Role updated successfully.", role.Id);
        }
    }
}
