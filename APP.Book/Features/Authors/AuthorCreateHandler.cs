using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Book.Features.Authors
{
    public class AuthorCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Surname { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }
    public class AuthorCreateHandler : BookDbHandler, IRequestHandler<AuthorCreateRequest, CommandResponse>
    {
        public AuthorCreateHandler(BookDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(AuthorCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _bookDb.Authors.AnyAsync(a => a.Name.ToUpper() == request.Name.ToUpper().Trim() && 
                a.Surname.ToUpper() == request.Surname.ToUpper().Trim(), cancellationToken))
            {
                return Error("Author with the same name exists!");
            }
               
            var entity = new Author()
            {
                
                Name = request.Name,
                Surname = request.Surname  
            };

            _bookDb.Authors.Add(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Author created successfully.", entity.Id);
        }
    }
}
