using CORE.APP.Domain;

namespace CORE.APP.Features
{
    public class CommandResponse : Entity
    {
        public bool IsSuccessful { get; }
        public string Message { get; }


        public CommandResponse(bool isSuccessful, string message = "", int id = 0) : base(id)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

    public class QueryResponse : Entity
    {
    }
}