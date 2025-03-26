using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Book.Domain
{
    public class Book : Entity
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(255)]
        public string Name { get; set; } // I set nullable as disable inside APP properties so that I do not have to use ? for strings.

        public short? NumberOfPages { get; set; }

        public DateTime? PublishDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public bool IsTopSeller { get; set; }

        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

        [NotMapped] // to not create any column for that
        public List<int> GenreIds
        {
            get => BookGenres.Select(bookGenre => bookGenre.GenreId).ToList();
            set => BookGenres = value.Select(v => new BookGenre() { GenreId = v }).ToList();
        }
    }
}
