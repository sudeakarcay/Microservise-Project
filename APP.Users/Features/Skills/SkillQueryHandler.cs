using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Users.Features.Skills
{
    /// <summary>
    /// Represents a request to query the list of skills.
    /// </summary>
    public class SkillQueryRequest : Request, IRequest<IQueryable<SkillQueryResponse>>
    {
        // No additional properties are required for this request.
    }

    /// <summary>
    /// Represents the response structure for querying skills.
    /// </summary>
    public class SkillQueryResponse : QueryResponse
    {
        /// <summary>
        /// Gets or sets the name of the skill.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Handles the querying of skills from the system.
    /// </summary>
    public class SkillQueryHandler : UsersDbHandler, IRequestHandler<SkillQueryRequest, IQueryable<SkillQueryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillQueryHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for querying skills.</param>
        public SkillQueryHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to retrieve the list of skills.
        /// </summary>
        /// <param name="request">The request to retrieve skills.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A queryable collection of <see cref="SkillQueryResponse"/> representing the skills.</returns>
        public Task<IQueryable<SkillQueryResponse>> Handle(SkillQueryRequest request, CancellationToken cancellationToken)
        {
            // Retrieve the list of skills ordered by name and project them into the response model
            var query = _db.Skills.OrderBy(s => s.Name).Select(s => new SkillQueryResponse()
            {
                Id = s.Id,
                Name = s.Name
            });

            return Task.FromResult(query);
        }
    }
}