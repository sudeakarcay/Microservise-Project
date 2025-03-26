using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Book.Features.Books
{
    public class BookCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Name { get; set; }

        public short NumberOfPages { get; set; }
        public DateTime? PublishDate { get; set; }

        public decimal Price { get; set; }

        public bool IsTopSeller { get; set; } = false;

        public int AuthorId { get; set; }

        public List<int> GenreIds { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }


    }

    public class BookCreateHandler : BookDbHandler, IRequestHandler<BookCreateRequest, CommandResponse>
    {
        public BookCreateHandler(BookDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BookCreateRequest request, CancellationToken cancellationToken)
        {

            if (request.NumberOfPages <= 0)
            {
                return Error("Number of pages must be greater than 0!");
            }

            if (request.Price <= 0)
            {
                return Error("Price must be greater than 0!");
            }

            if (await _bookDb.Books.AnyAsync(b => b.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Book with the same name exists!");

            var entity = new Domain.Book()
            {
                Name = request.Name.Trim(),
                AuthorId = request.AuthorId,
                NumberOfPages = request.NumberOfPages,
                PublishDate = request.PublishDate,
                Price = request.Price,
                GenreIds = request.GenreIds,
                IsTopSeller = request.IsTopSeller
            };
            _bookDb.Books.Add(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);
            return Success("Book created successfully!", entity.Id);
        }

    }
}
