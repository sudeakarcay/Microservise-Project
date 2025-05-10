using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Users
{
    /// <summary>
    /// Represents a request to update a user's information.
    /// </summary>
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

       
        /// <summary>
        /// Gets or sets the status indicating if the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the user.
        /// </summary>
        [StringLength(50)]
        public string Surname { get; set; }
    }

    /// <summary>
    /// Handles the request to update a user's information.
    /// </summary>
    public class UserUpdateHandler : UsersDbHandler, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling user-related operations.</param>
        public UserUpdateHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to update a user's information.
        /// </summary>
        /// <param name="request">The request containing the updated user data.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a <see cref="CommandResponse"/> indicating the outcome of the operation.</returns>
        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            // Check if another user with the same username or full name exists (excluding the user being updated)
            if (_db.Users.Any(u => u.Id != request.Id && (u.UserName == request.UserName || (u.FirstName == request.Name && u.LastName == request.Surname))))
                return Error("User with the same user name or full name exists!");

            // Retrieve the user from the database
            var user = _db.Users.Include(u => u.UserSkills).SingleOrDefault(u => u.Id == request.Id);
            if (user is null)
                return Error("User not found!");

            // Update the user information
            user.IsActive = request.IsActive;
            user.FirstName = request.Name?.Trim();   
            user.LastName = request.Surname?.Trim();
            user.UserName = request.UserName;

            // Save the updated user data to the database
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("User updated successfully.", user.Id);
        }
    }
}