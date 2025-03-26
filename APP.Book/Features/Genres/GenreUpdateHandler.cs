using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Book.Features.Genres
{

    public class GenreUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Name { get; set; }
    }
    public class GenreUpdateHandler : BookDbHandler, IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        public GenreUpdateHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(GenreUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _bookDb.Genres.AnyAsync(g => g.Id != request.Id && g.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Not completed Genre with the same title exists!");

            var entity = await _bookDb.Genres.Include(g => g.BookGenres).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Genre doesn't exists!");

            _bookDb.BookGenres.RemoveRange(entity.BookGenres);

            entity.Name = request.Name;

            _bookDb.Genres.Update(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Genre updated successfully.", entity.Id);
        }
    }
}
