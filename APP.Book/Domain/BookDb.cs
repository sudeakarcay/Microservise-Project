using Microsoft.EntityFrameworkCore;

namespace APP.Book.Domain
{
    public class BookDb : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BookDb(DbContextOptions options) : base(options)
        {
        }


    }
}
