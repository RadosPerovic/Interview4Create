using Interview4Create.Project.Domain.Common.Enums;

namespace Interview4Create.Project.Domain.Common.Errors;
public static class DomainErrors
{
    public static class Company
    {
        public static Error CompanyPlaceForSpecificTitleFilled(EmployeeTitle title) => new("DE_COMP001", $"Company place for title: {title} is already filled");
        public static Error EmployeeAlreadyExistInCompany() => new("DE_COMP002", "Employee already exists in company");
    }
}
