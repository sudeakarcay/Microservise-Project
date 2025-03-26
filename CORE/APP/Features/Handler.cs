using System.Globalization;

namespace CORE.APP.Features
{
    public abstract class Handler
    {
        protected Handler(CultureInfo cultureInfo) //en-US, tr-TR
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public CommandResponse Success(string message = "", int id = 0) => new CommandResponse(true, message, id);
        public CommandResponse Error(string message = "") => new CommandResponse(false, message);
    }
}