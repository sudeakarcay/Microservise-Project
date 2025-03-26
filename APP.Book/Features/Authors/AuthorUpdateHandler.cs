using APP.Book.Domain;
using APP.Book.Features.Genres;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Book.Features.Authors
{
    public class AuthorUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Surname { get; set; }
    }
    public class AuthorUpdateHandler : BookDbHandler, IRequestHandler<AuthorUpdateRequest, CommandResponse>
    {
        public AuthorUpdateHandler(BookDb bookDb) : base(bookDb)
        {
        }

        public async Task<CommandResponse> Handle(AuthorUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _bookDb.Authors.AnyAsync(a => a.Id != request.Id && a.Name.ToUpper() == request.Name.ToUpper().Trim()
                && a.Surname.ToUpper() == request.Surname.ToUpper().Trim(), cancellationToken))
                return Error("Not completed Author with the same title exists!");

            var entity = await _bookDb.Authors.SingleOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Auhor doesn't exists!");

            entity.Name = request.Name;
            entity.Surname = request.Surname;

            _bookDb.Authors.Update(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Author updated successfully.", entity.Id);
        }
    }
}
