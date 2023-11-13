using Interview4Create.Project.Domain.Common.Errors;

namespace Interview4Create.Project.Application.Errors;
public class ApplicationErrors
{
    public static class Employee
    {
        public static Error EmailIsNotUnique() => new("AE_EMPL001", "Employee email is not unique");
        public static Error EmailMustNotBeEmpty() => new("AE_EMPL002", "Employee email must not be empty");
        public static Error EmailMaxLength(int maximumLength) => new("AE_EMPL003", $"Email can be max length: {maximumLength}");
        public static Error TitleCannotBeNone() => new("AE_EMPL004", "Title cannot be None");
        public static Error EmployeeDoesNotExist() => new("AE_EMPL005", "Employee does not exist");
    }

    public static class Company
    {
        public static Error NameIsNotUnique() => new("AE_COMP001", "Company name is not unique");
        public static Error CompanyDoesNotExist() => new("AE_COMP002", "Company does not exist");
        public static Error NameCannotBeEmpty() => new("AE_COMP003", "Company name cannot be empty");
        public static Error NameMaxLength(int maximumLength) => new("AE_COMP004", $"Name can be max length: {maximumLength}");
        public static Error MustHaveEmployees() => new("AE_COMP005", "Company must have employees");
        public static Error EmployeeIdMustNotBeNullWhenEmailAndTitleAreNull() => new("AE_COMP006", "Id property must not be null when email and title are null");
        public static Error EmployeeIdMustBeNullWhenEmailAndTitleAreNotNull() => new("AE_COMP007", "Id property must be null when email and title are not null");
        public static Error EmployeeEmailMustNotBeNullWhenIdIsNull() => new("AE_COMP006", "Email property must not be null when id is null");
        public static Error EmployeeEmailMustBeNullWhenIdIsNotNull() => new("AE_COMP007", "Email property must be null when id is not null");
        public static Error EmployeeTitleMustNotBeNullWhenIdIsNull() => new("AE_COMP006", "Title property must not be null when id is null");
        public static Error EmployeeTitleMustBeNullWhenIdIsNotNull() => new("AE_COMP007", "Title property must be null when id is not null");
    }

}
