using CORE.APP.Features;
using MediatR;
using APP.Book.Domain;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


namespace APP.Book.Features.Books
{ 
    public class BookQueryRequest : Request, IRequest<IQueryable<BookQueryResponse>>
    {

        public decimal? Price { get; set; }

        public bool? IsTopSeller { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }
    public  class BookQueryResponse : QueryResponse 
    {
        public string Name { get; set; }
        public short NumberOfPages { get; set; }

        public DateTime? PublisDate { get; set; }
        public string PublishDateF { get; set; }

        public decimal Price { get; set; }

        public bool IsTopSeller { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
        public string GenreNames { get; set; }
    }

    public class BookQueryHandler : BookDbHandler, IRequestHandler<BookQueryRequest, IQueryable<BookQueryResponse>>
    {
        public BookQueryHandler(BookDb db) : base(db)
        {
        }

        public Task<IQueryable<BookQueryResponse>> Handle(BookQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = _bookDb.Books.Include(book => book.BookGenres).ThenInclude(bookgenre => bookgenre.Genre).Include(author => author.Author)
                .OrderBy(b => b.PublishDate).AsQueryable();

            //filtering

            if (request.Price.HasValue)
                entityQuery = entityQuery.Where(b => b.Price >= request.Price);

            if (request.IsTopSeller.HasValue)
                entityQuery = entityQuery.Where(b => b.IsTopSeller == request.IsTopSeller);


            var query = entityQuery.Select(b => new BookQueryResponse()
            {
                Id = b.Id,
                Name = b.Name,
                AuthorId = b.AuthorId,
                NumberOfPages = (short)b.NumberOfPages,
                PublishDateF = b.PublishDate.HasValue ? b.PublishDate.Value.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty,
                Price = b.Price,
                IsTopSeller = b.IsTopSeller,
                AuthorName = string.Join(" ", b.Author.Name, b.Author.Surname),
                GenreNames = string.Join(", ", b.BookGenres.Select(bookgenre => bookgenre.Genre.Name))
            });
            return Task.FromResult(query);
        }
    }
}


