using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Book.Features.Genres
{
    public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>   
    {
    }
    public class GenreQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class GenreQueryHandler : BookDbHandler, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        public GenreQueryHandler(BookDb db) : base(db)
        {
        }

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _bookDb.Genres.Include(genre => genre.BookGenres).ThenInclude(bookgenre => bookgenre.Book)
                .OrderBy(g => g.Name)
                .Select(g => new GenreQueryResponse()
                {
                    Id = g.Id,
                    Name = g.Name
                });
            return Task.FromResult(query);
        }
    }
}
