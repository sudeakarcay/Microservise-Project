using APP.Book.Domain;
using System.Globalization;
using CORE.APP.Features;

namespace APP.Book.Features
{
    public abstract class BookDbHandler : Handler
    {
        protected readonly BookDb _bookDb;
       protected BookDbHandler(BookDb bookDb) : base(new CultureInfo("en-US"))
       {
            _bookDb = bookDb;
       }
    }
}
