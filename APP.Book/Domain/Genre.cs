using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APP.Book.Domain
{
    public class Genre  : Entity
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "{0} must have minimum {2} maximum {1} characters!")]
        public string Name { get; set; }
        public  List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

        [NotMapped] // to not create any column for that
        public List<int> BookIds
        {
            get => BookGenres.Select(bookGenre => bookGenre.BookId).ToList();
            set => BookGenres = value.Select(v => new BookGenre() { BookId = v}).ToList();
        }
    }
}