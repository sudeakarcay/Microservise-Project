using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Book.Features.Genres
{

    public class GenreDeleteRequest : Request, IRequest<CommandResponse>
    {
    }
    public class GenreDeleteHandler : BookDbHandler, IRequestHandler<GenreDeleteRequest, CommandResponse>
    {
        public GenreDeleteHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(GenreDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _bookDb.Genres.Include(g => g.BookGenres).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Genre is not found!");

            _bookDb.BookGenres.RemoveRange(entity.BookGenres);

            _bookDb.Genres.Remove(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Genre deleted successfully.", entity.Id);
        }
    }
}
