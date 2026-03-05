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

        public const string IdReq = $"{EmpName}-ID-REQ";
        public const string IdInvalid = $"{EmpName}-ID-INVALID";
        public const string DobReq = $"{EmpName}-DOB-REQ";
        public const string DobInvalidAge = $"{EmpName}-DOB-INVALID-AGE";
        public const string EmpNotFound = $"{EmpName}-NOT-FOUND";
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

    public class Leave
    {
        protected Leave() { }
        private const string LvName = $"{Name}-LV";

        public const string RequestNotFound = $"{LvName}-REQ-NOT-FOUND";
        public const string TypeNotFound = $"{LvName}-TYPE-NOT-FOUND";
        public const string PolicyNotFound = $"{LvName}-POLICY-NOT-FOUND";
        public const string BalanceNotFound = $"{LvName}-BALANCE-NOT-FOUND";
        public const string HolidayNotFound = $"{LvName}-HOLIDAY-NOT-FOUND";
        
        public const string InsufficientBalance = $"{LvName}-INSUFFICIENT-BALANCE";
        public const string OverlappingRequest = $"{LvName}-OVERLAPPING-REQ";
        public const string InvalidPeriod = $"{LvName}-INVALID-PERIOD";
        public const string RequestAlreadyProcessed = $"{LvName}-REQ-ALREADY-PROCESSED";
        public const string UnauthorizedApproval = $"{LvName}-UNAUTHORIZED-APPROVAL";
        public const string PolicyViolation = $"{LvName}-POLICY-VIOLATION";
        public const string PastDateRequest = $"{LvName}-PAST-DATE-REQ";
        public const string MinimumNoticeRequired = $"{LvName}-MIN-NOTICE-REQ";
        public const string MaxDaysExceeded = $"{LvName}-MAX-DAYS-EXCEEDED";
        public const string ProbationRestriction = $"{LvName}-PROBATION-RESTRICTION";
        public const string DocumentRequired = $"{LvName}-DOCUMENT-REQUIRED";
        public const string InvalidRequestType = $"{LvName}-INVALID-REQ-TYPE";
        public const string TypeCodeConflict = $"{LvName}-TYPE-CODE-CONFLICT";
        public const string HolidayDateConflict = $"{LvName}-HOLIDAY-DATE-CONFLICT";
        public const string InvalidApprovalLevel = $"{LvName}-INVALID-APPROVAL-LEVEL";
        public const string ApprovalNotPending = $"{LvName}-APPROVAL-NOT-PENDING";
        public const string CannotCancelApproved = $"{LvName}-CANNOT-CANCEL-APPROVED";
        
        public const string TypeIdReq = $"{LvName}-TYPE-ID-REQ";
        public const string TypeNameReq = $"{LvName}-TYPE-NAME-REQ";
        public const string TypeCodeReq = $"{LvName}-TYPE-CODE-REQ";
        public const string StartDateReq = $"{LvName}-START-DATE-REQ";
        public const string EndDateReq = $"{LvName}-END-DATE-REQ";
        public const string TotalDaysReq = $"{LvName}-TOTAL-DAYS-REQ";
        public const string ReasonReq = $"{LvName}-REASON-REQ";
        public const string EmployeeIdReq = $"{LvName}-EMPLOYEE-ID-REQ";
        public const string RequestIdReq = $"{LvName}-REQUEST-ID-REQ";
        public const string HolidayNameReq = $"{LvName}-HOLIDAY-NAME-REQ";
        public const string HolidayDateReq = $"{LvName}-HOLIDAY-DATE-REQ";
        public const string PolicyNameReq = $"{LvName}-POLICY-NAME-REQ";
    }
}