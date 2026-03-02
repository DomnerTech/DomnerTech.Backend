namespace DomnerTech.Backend.Application.Errors;

public class ErrorCodes
{
    protected ErrorCodes(){ }
    private const string Name = "MNT";

    public const string SystemError = $"{Name}-SYSERR";
    public const string Validation = $"{Name}-VALIDATION-ERR";

    public const string NotFound = "not_found";
    public const string Conflict = "conflict";
    public const string Unauthorized = "unauthorized";
    public const string Forbidden = "forbidden";
    public const string Internal = "internal_server_error";
    public const string HeaderMissing = "header_missing";

    public class Localize
    {
        protected Localize() { }
        private const string LzName = $"{Name}-LZ";
        public class ErrorMessage
        {
            protected ErrorMessage() { }
            private const string EmName = $"{LzName}-ERR-MSG";
            public const string KeyRequired = $"{EmName}-KEY-REQ";
            public const string MessagesRequired = $"{EmName}-MSG-REQ";
            public const string MessagesLangRequired = $"{EmName}-MSG-LANG-REQ";
            public const string MessagesValueRequired = $"{EmName}-MSG-VAL-REQ";
        }
    }
}