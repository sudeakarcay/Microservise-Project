using APP.Book.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Book.Features.Genres
{
    public class GenreCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(255, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Name { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

    public class GenreCreateHandler : BookDbHandler, IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        public GenreCreateHandler(BookDb db) : base(db) { }

        public async Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _bookDb.Genres.AnyAsync(g => g.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Not completed genre with the same name exists!");

            var entity = new Genre()
            {
                Name = request.Name
            };

            _bookDb.Genres.Add(entity);
            await _bookDb.SaveChangesAsync(cancellationToken);

            return Success("Genre created successfully.", entity.Id);
        }
    }
}
