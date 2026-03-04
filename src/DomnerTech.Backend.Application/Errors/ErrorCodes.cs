namespace DomnerTech.Backend.Application.Errors;

public class ErrorCodes
{
    protected ErrorCodes(){ }
    private const string Name = "MNT";

    public const string SystemError = $"{Name}-SYSERR";
    public const string Validation = $"{Name}-VALIDATION-ERR";
    public const string HeaderMissing = $"{Name}-HEADER-REQ";
    public const string NotFound = $"{Name}-NOT-FOUND";
    public const string Conflict = $"{Name}-CONFLICT";
    public const string Unauthorized = $"{Name}-UNAUTHORIZED";
    public const string Forbidden = $"{Name}-FORBIDDEN";
    public const string MaxPageSize = $"{Name}-MAX-PAGE-SIZE";

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