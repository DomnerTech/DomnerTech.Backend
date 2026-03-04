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

    public class Employee
    {
        protected Employee() { }
        private const string EmpName = $"{Name}-EMP";

        public const string DobReq = $"{EmpName}-DOB-REQ";
        public const string DobInvalidAge = $"{EmpName}-DOB-INVALID-AGE";
        public const string NotFound = $"{EmpName}-NOT-FOUND";
        public const string EmailConflict = $"{EmpName}-EMAIL-CONFLICT";
        public const string DepartmentReq = $"{EmpName}-DEPARTMENT-REQ";
        public const string EmailReq = $"{EmpName}-EMAIL-REQ";
        public const string EmailInvalid = $"{EmpName}-EMAIL-INVALID";
        public const string FirstNameReq = $"{EmpName}-FIRSTNAME-REQ";
        public const string HireDateReq = $"{EmpName}-HIREDATE-REQ";
        public const string HireDateFuture = $"{EmpName}-HIREDATE-FUTURE";
        public const string JobTitleReq = $"{EmpName}-JOBTITLE-REQ";
        public const string LastNameReq = $"{EmpName}-LASTNAME-REQ";
        public const string PhoneNumberReq = $"{EmpName}-PHONENUMBER-REQ";
        public const string StreetReq = $"{EmpName}-STREET-REQ";
        public const string CityReq = $"{EmpName}-CITY-REQ";
        public const string PostalCodeReq = $"{EmpName}-POSTALCODE-REQ";
        public const string CountryReq = $"{EmpName}-COUNTRY-REQ";
        public const string AddressReq = $"{EmpName}-ADDRESS-REQ";
    }
}