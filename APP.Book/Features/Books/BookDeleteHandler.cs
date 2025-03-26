using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Book.Features.Books
{
    public class BookDeleteRequest : Request, IRequest<CommandResponse>
    {
    }
    public class BookDeleteHandler : BookDbHandler, IRequestHandler<BookDeleteRequest, CommandResponse>
    {
        public BookDeleteHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(BookDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _bookDb.Books.FindAsync(request.Id, cancellationToken);

            if (entity is null)
                return Error("Book not found!");

            _bookDb.Books.Remove(entity);

            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Book deleted successfully.", entity.Id);
        }
    }
}
