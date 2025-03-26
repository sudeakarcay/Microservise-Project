using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Book.Features.Books
{

    public class BookUpdateRequest : Request, IRequest<CommandResponse>
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
    }
    public class BookUpdateHandler : BookDbHandler, IRequestHandler<BookUpdateRequest, CommandResponse>
    {
        public BookUpdateHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(BookUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.NumberOfPages <= 0)
            {
                return Error("Number of pages must be greater than 0!");
            }

            if (request.Price <= 0)
            {
                return Error("Price must be greater than 0!");
            }

            if (await _bookDb.Books.AnyAsync(b => b.Id != request.Id && b.Name.ToLower() == request.Name.ToLower().Trim(), cancellationToken))
                return Error("Book with the same name exitsts!");

           
            var entity = await _bookDb.Books.FindAsync(request.Id, cancellationToken);

            if (entity is null)
                return Error("Book not found!");

            entity.Name = request.Name.Trim();
            entity.AuthorId = request.AuthorId;
            entity.Price = request.Price;
            entity.NumberOfPages = request.NumberOfPages;
            entity.PublishDate = request.PublishDate;
            entity.IsTopSeller = request.IsTopSeller;

            _bookDb.Books.Update(entity);

            await _bookDb.SaveChangesAsync(cancellationToken);
            return Success("Book updated successfully!", entity.Id);
        }
    }
}
