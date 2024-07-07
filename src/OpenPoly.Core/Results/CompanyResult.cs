using OpenPoly.Core.Models;

namespace OpenPoly.Core.Results
{
    public record CompanyResult : Result<Company, Error>
    {
        private CompanyResult(Result<Company, Error> original) : base(original)
        {
        }

        public static new CompanyResult Success(Company result) => new CompanyResult(result);
        public static new CompanyResult Failure(Error error) => new CompanyResult(error);
}
}
