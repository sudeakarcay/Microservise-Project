﻿using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Users.Features.Roles
{
    /// <summary>
    /// Represents a request to create a new role.
    /// </summary>
    public class RoleCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; } // Inherited ID property, ignored in JSON serialization.

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
    /// Handles the creation of a new role in the system.
    /// </summary>
    public class RoleCreateHandler : UsersDbHandler, IRequestHandler<RoleCreateRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCreateHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling role-related operations.</param>
        public RoleCreateHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to create a new role.
        /// </summary>
        /// <param name="request">The request containing the role information to be created.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the role creation operation.</returns>
        public async Task<CommandResponse> Handle(RoleCreateRequest request, CancellationToken cancellationToken)
        {
            // Check if the role with the same name already exists
            if (_db.Roles.Any(r => r.Name == request.Name))
                return Error("Role with the same name exists!");

            // Create a new role entity
            var role = new Role()
            {
                Name = request.Name?.Trim()
            };

            // Add the new role to the database and save changes
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Role created successfully.", role.Id);
        }
    }
}