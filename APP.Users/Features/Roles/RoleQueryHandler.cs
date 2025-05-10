using APP.Users.Domain;
using APP.Users.Features.Users;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APP.Users.Features.Roles
{
    /// <summary>
    /// Represents a request to query roles from the system.
    /// </summary>
    public class RoleQueryRequest : Request, IRequest<IQueryable<RoleQueryResponse>>
    {
        // No additional properties are required for this request.
    }

    /// <summary>
    /// Represents the response format for querying roles.
    /// </summary>
    public class RoleQueryResponse : QueryResponse
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of users associated with the role.
        /// </summary>
        /// <remarks>
        /// This property is ignored during serialization as it's intended for internal processing.
        /// </remarks>
        [JsonIgnore]
        public List<UserQueryResponse> Users { get; set; }
    }

    /// <summary>
    /// Handles the query for roles in the system.
    /// </summary>
    public class RoleQueryHandler : UsersDbHandler, IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleQueryHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for role-related operations.</param>
        public RoleQueryHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to query roles from the database.
        /// </summary>
        /// <param name="request">The request containing the query parameters.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="IQueryable{RoleQueryResponse}"/> as the result.</returns>
        public Task<IQueryable<RoleQueryResponse>> Handle(RoleQueryRequest request, CancellationToken cancellationToken)
        {
            // Build the query to get roles along with their associated users
            var query = _db.Roles.Include(r => r.Users)
                .OrderBy(r => r.Name).Select(r => new RoleQueryResponse()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Users = r.Users.Select(u => new UserQueryResponse()
                    {
                        FullName = u.FirstName + " " + u.LastName,
                        Id = u.Id,
                        IsActive = u.IsActive,
                        IsActiveF = u.IsActive ? "Active" : "Inactive",
                        FirstName = u.FirstName,
                        Password = u.Password,
                        LastName = u.LastName,
                        UserName = u.UserName
                    }).ToList()
                });

            return Task.FromResult(query);
        }
    }
}