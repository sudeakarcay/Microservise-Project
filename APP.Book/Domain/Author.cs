using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APP.Book.Domain
{
    public class Author : Entity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Surname { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
