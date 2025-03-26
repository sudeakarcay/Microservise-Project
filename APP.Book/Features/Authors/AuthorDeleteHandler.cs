using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Book.Features.Authors
{
    public class AuthorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }
    public class AuthorDeleteHandler : BookDbHandler, IRequestHandler<AuthorDeleteRequest, CommandResponse>
    {
        public AuthorDeleteHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(AuthorDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _bookDb.Authors.SingleOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Author is not found!");

            _bookDb.Authors.Remove(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Author deleted successfully.", entity.Id);
        }
    }
}
