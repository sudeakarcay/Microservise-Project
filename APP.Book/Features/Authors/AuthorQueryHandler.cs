using APP.Book.Domain;
using APP.Book.Features.Books;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Book.Features.Authors
{
    public class AuthorQueryRequest : Request, IRequest<IQueryable<AuthorQueryResponse>>
    {
    }
    public class AuthorQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string BooksNames{ get; set; }
    }

    public class AuthorQueryHandler : BookDbHandler, IRequestHandler<AuthorQueryRequest, IQueryable<AuthorQueryResponse>>
    {
        public AuthorQueryHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public Task<IQueryable<AuthorQueryResponse>> Handle(AuthorQueryRequest request, CancellationToken cancellationToken)
        {
            IQueryable<AuthorQueryResponse> query = _bookDb.Authors.OrderBy(a => a.Name)
                .Select(a => new AuthorQueryResponse()
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname,
                BooksNames = string.Join(", ", a.Books.Select(b => b.Name))
             });
            return Task.FromResult(query);
        }
    }
}
