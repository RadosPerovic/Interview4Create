using AutoFixture.Xunit2;
using FluentAssertions;
using Interview4Create.Project.Application.UseCases.Employees.Commands;
using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.Errors;
using Interview4Create.Project.Domain.Common.Generators;
using Interview4Create.Project.Domain.Companies;
using Interview4Create.Project.Domain.Employees;
using Moq;

namespace Interview4Create.Project.Application.UnitTests.UseCases.Employees.Commands;
public class CreateEmployeeCommandRequestHandlerTests
{
    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Developer)]
    [AutoMoqInlineData(EmployeeTitle.Manager)]
    [AutoMoqInlineData(EmployeeTitle.Tester)]
    public async Task Handle_ValidRequest_CreateEmployeeAndAssigneToRequestedCompanies(
        EmployeeTitle employeeTitle,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        CreateEmployeeCommandRequest request,
        CreateEmployeeCommandRequestHandler sut,
        List<Company> companies,
        Guid employeeId)
    {
        mockEmployeeRepository
            .Setup(repo => repo.IsEmailUnique(request.Email))
            .ReturnsAsync(true);

        var requestCompanyIds = companies
            .Select(company => company.Id.Value)
            .ToList();
        request.CompanyIds = requestCompanyIds;
        request.Title = employeeTitle;

        mockCompanyRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<CompanyIdentity>>(
                    listOfIdentites => listOfIdentites
                        .All(ci => requestCompanyIds
                            .Contains(ci.Value)))))
            .ReturnsAsync(companies);

        mockGuidGenerator
            .Setup(g => g.Generate())
            .Returns(employeeId);

        var createdTs = new DateTime(2023, 10, 10);
        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeTrue();
        result.Data.CreatedId.Should().Be(employeeId);

        mockEmployeeRepository
            .Verify(repo => repo
                .Add(It.Is<Employee>(
                    e => e.Id.Value == employeeId
                    && e.Email == request.Email
                    && e.Title == request.Title
                    && e.CreatedTs == createdTs
                    )),
            Times.Once);

        mockCompanyRepository
            .Verify(repo => repo
                .UpdateRange(
                    It.Is<IEnumerable<Company>>(
                            listOfCompanies => listOfCompanies
                                .All(c => c.Employees
                                    .Select(e => e.Id.Value)
                                        .Contains(employeeId)
                                        && requestCompanyIds.Contains(c.Id.Value)))),
            Times.Once);

        mockDateTimeProvider
            .Verify(e => e.Now(),
            Times.Once);

        mockGuidGenerator
            .Verify(e => e.Generate(),
            Times.Once);
    }

    [Theory]
    [AutoMoqInlineData()]
    public async Task Handle_EmailIsNotUnique_ReturnsError(
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        CreateEmployeeCommandRequest request,
        CreateEmployeeCommandRequestHandler sut)
    {
        mockEmployeeRepository
            .Setup(repo => repo.IsEmailUnique(request.Email))
            .ReturnsAsync(false);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(Errors.ApplicationErrors.Employee.EmailIsNotUnique().ErrorCode);

        mockEmployeeRepository
            .Verify(repo => repo
                .Add(It.IsAny<Employee>()),
            Times.Never);

        mockCompanyRepository
            .Verify(repo => repo
                .UpdateRange(
                    It.IsAny<IEnumerable<Company>>()),
            Times.Never);

        mockDateTimeProvider
            .Verify(e => e.Now(),
            Times.Never);

        mockGuidGenerator
            .Verify(e => e.Generate(),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData()]
    public async Task Handle_AtleastOneCompanyFromRequestDoesNotExist_ReturnsError(
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        CreateEmployeeCommandRequest request,
        CreateEmployeeCommandRequestHandler sut,
        List<Company> companies)
    {
        mockEmployeeRepository
            .Setup(repo => repo.IsEmailUnique(request.Email))
            .ReturnsAsync(true);

        var requestCompanyIds = companies
            .Select(company => company.Id.Value)
            .ToList();
        request.CompanyIds = requestCompanyIds;
        request.Title = EmployeeTitle.Developer;

        mockCompanyRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<CompanyIdentity>>(
                    listOfIdentites => listOfIdentites
                        .All(ci => requestCompanyIds
                            .Contains(ci.Value)))))
            .ReturnsAsync(companies.Take(2));

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(Errors.ApplicationErrors.Company.CompanyDoesNotExist().ErrorCode);

        mockEmployeeRepository
            .Verify(repo => repo
                .Add(It.IsAny<Employee>()),
            Times.Never);

        mockCompanyRepository
            .Verify(repo => repo
                .UpdateRange(
                    It.IsAny<IEnumerable<Company>>()),
            Times.Never);

        mockDateTimeProvider
            .Verify(e => e.Now(),
            Times.Never);

        mockGuidGenerator
            .Verify(e => e.Generate(),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Developer)]
    [AutoMoqInlineData(EmployeeTitle.Manager)]
    [AutoMoqInlineData(EmployeeTitle.Tester)]
    public async Task Handle_EmployeeWithSameTitleAlreadyExistInCompany_ReturnsDomainError(
        EmployeeTitle employeeTitle,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        CreateEmployeeCommandRequest request,
        CreateEmployeeCommandRequestHandler sut,
        List<Company> companies,
        Guid employeeId,
        string existingEmployeeEmail,
        Guid existingEmployeeId)
    {
        mockEmployeeRepository
            .Setup(repo => repo.IsEmailUnique(request.Email))
            .ReturnsAsync(true);

        var requestCompanyIds = companies
            .Select(company => company.Id.Value)
            .ToList();
        request.CompanyIds = requestCompanyIds;
        request.Title = employeeTitle;

        var existingEmployee = new Employee(new EmployeeIdentity(existingEmployeeId), existingEmployeeEmail, employeeTitle, DateTime.Now);
        companies.FirstOrDefault()!.AddEmployee(existingEmployee);

        mockCompanyRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<CompanyIdentity>>(
                    listOfIdentites => listOfIdentites
                        .All(ci => requestCompanyIds
                            .Contains(ci.Value)))))
            .ReturnsAsync(companies);

        mockGuidGenerator
            .Setup(g => g.Generate())
            .Returns(employeeId);

        var createdTs = new DateTime(2023, 10, 10);
        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(DomainErrors.Company.CompanyPlaceForSpecificTitleFilled(employeeTitle).ErrorCode);

        mockEmployeeRepository
            .Verify(repo => repo
                .Add(It.IsAny<Employee>()),
            Times.Never);

        mockCompanyRepository
            .Verify(repo => repo
                .UpdateRange(
                    It.IsAny<IEnumerable<Company>>()),
            Times.Never);
    }
}
