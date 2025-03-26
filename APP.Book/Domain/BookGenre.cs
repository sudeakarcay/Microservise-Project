using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Book.Domain
{
    public class BookGenre : Entity
    {
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }
}