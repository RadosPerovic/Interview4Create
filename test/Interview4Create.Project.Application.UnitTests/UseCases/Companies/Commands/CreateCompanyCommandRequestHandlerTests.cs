using AutoFixture.Xunit2;
using FluentAssertions;
using Interview4Create.Project.Application.UnitTests.Mocks;
using Interview4Create.Project.Application.UseCases.Companies.Commands;
using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.Errors;
using Interview4Create.Project.Domain.Common.Generators;
using Interview4Create.Project.Domain.Companies;
using Interview4Create.Project.Domain.Companies.Factory;
using Interview4Create.Project.Domain.Employees;
using Interview4Create.Project.Domain.Employees.Factories;
using Moq;

namespace Interview4Create.Project.Application.UnitTests.UseCases.Companies.Commands;
public class CreateCompanyCommandRequestHandlerTests
{
    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Manager, EmployeeTitle.Tester)]
    [AutoMoqInlineData(EmployeeTitle.Manager, EmployeeTitle.Developer)]
    [AutoMoqInlineData(EmployeeTitle.Developer, EmployeeTitle.Tester)]
    public async Task Handle_ValidRequest_CreateCompanyAndAssigneItToExistingEmployees(
        EmployeeTitle employeeTitle1,
        EmployeeTitle employeeTitle2,
        Guid employeeId1,
        Guid employeeId2,
        string email1,
        string email2,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        CreateCompanyCommandRequest request,
        CreateCompanyCommandRequestHandler sut,
        Guid companyId)
    {
        var createdTs = new DateTime(2023, 10, 10);
        var employees = new List<Employee>()
        {
            new Employee(new EmployeeIdentity(employeeId1), email1, employeeTitle1, createdTs),
            new Employee(new EmployeeIdentity(employeeId2), email2, employeeTitle2, createdTs),
        };

        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        mockGuidGenerator
            .Setup(g => g
                .Generate())
            .Returns(companyId);

        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(true);

        request.Employees = employees.Select(e => new CreateCompanyEmployeeCommandRequest()
        {
            Id = e.Id.Value,
            Email = null,
            Title = null,
        })
        .ToList();

        var employeeIdsFromRequest = request.Employees
            .Select(e => e.Id!.Value)
            .ToList();

        mockEmployeeRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<EmployeeIdentity>>(
                    listOfEmployeeIds => listOfEmployeeIds
                        .All(ei => employeeIdsFromRequest
                            .Contains(ei.Value)))))
            .ReturnsAsync(employees);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeTrue();
        result.Data.CreatedId.Should().Be(companyId);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.Is<Company>(
                    e => e.Id.Value == companyId
                    && e.Name == request.Name
                    && e.CreatedTs == createdTs
                    && e.Employees.All(x => employeeIdsFromRequest.Contains(x.Id.Value)))),
            Times.Once);

        mockEmployeeRepository
            .Verify(repo => repo.
                AddRange(It.IsAny<IEnumerable<Employee>>()),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Manager, EmployeeTitle.Tester)]
    [AutoMoqInlineData(EmployeeTitle.Manager, EmployeeTitle.Developer)]
    [AutoMoqInlineData(EmployeeTitle.Developer, EmployeeTitle.Tester)]
    public async Task Handle_ValidRequest_CreateCompanyAndAssigneItToNewEmployees(
        EmployeeTitle employeeTitle1,
        EmployeeTitle employeeTitle2,
        string email1,
        string email2,
        Guid employeeId1,
        Guid employeeId2,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
        [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
        CreateCompanyCommandRequest request,
        Guid companyId)
    {
        var createdTs = new DateTime(2023, 10, 10);
        var employees = new List<Employee>()
        {
            new Employee(new EmployeeIdentity(employeeId1), email1, employeeTitle1, createdTs),
            new Employee(new EmployeeIdentity(employeeId2), email2, employeeTitle2, createdTs),
        };

        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        mockGuidGenerator
            .Setup(g => g
                .Generate())
            .Returns(companyId);

        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(true);

        request.Employees = employees.Select(e => new CreateCompanyEmployeeCommandRequest()
        {
            Id = null,
            Email = e.Email,
            Title = e.Title,
        })
        .ToList();

        var emailsFromRequest = request.Employees
            .Select(e => e.Email)
            .ToList();

        mockEmployeeRepository
            .Setup(repo => repo
                .AreEmailsUnique(emailsFromRequest!))
            .ReturnsAsync(true);

        var companyFactory = new CompanyFactory(mockGuidGenerator.Object, mockDateTimeProvider.Object);
        var employeeFactory = new EmployeeFactory(new MockGuidGenerator(), new MockDateTimeProvider(new DateTime(2023, 09, 09)));
        var unitOfWork = new MockUnitOfWork();

        var sut = new CreateCompanyCommandRequestHandler(
            mockCompanyRepository.Object,
            companyFactory,
            mockEmployeeRepository.Object,
            employeeFactory,
            unitOfWork);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeTrue();
        result.Data.CreatedId.Should().Be(companyId);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.Is<Company>(
                    e => e.Id.Value == companyId
                    && e.Name == request.Name
                    && e.CreatedTs == createdTs
                    && e.Employees.Select(e => e.Email).All(e => emailsFromRequest.Contains(e)))),
            Times.Once);

        mockEmployeeRepository
            .Verify(repo => repo
                .AddRange(It.Is<IEnumerable<Employee>>(
                    listOfEmployees => listOfEmployees.Any(e => emailsFromRequest.Contains(e.Email)))),
            Times.Once);
    }

    [Theory]
    [AutoMoqInlineData]
    public async Task Handle_CompanyNameIsNotUnique_ReturnError(
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        CreateCompanyCommandRequest request,
        CreateCompanyCommandRequestHandler sut)
    {
        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(false);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(Errors.ApplicationErrors.Company.NameIsNotUnique().ErrorCode);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.IsAny<Company>()),
            Times.Never);

        mockEmployeeRepository
            .Verify(repo => repo.
                AddRange(It.IsAny<IEnumerable<Employee>>()),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Manager, EmployeeTitle.Tester)]
    public async Task Handle_WhenEmployeeIdsAreNotNull_AtleastOneEmployeeDoesNotExist_ReturnError(
        EmployeeTitle employeeTitle1,
        EmployeeTitle employeeTitle2,
        Guid employeeId1,
        Guid employeeId2,
        string email1,
        string email2,
        [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
        [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
        CreateCompanyCommandRequest request,
        CreateCompanyCommandRequestHandler sut)
    {
        var createdTs = new DateTime(2023, 10, 10);
        var employees = new List<Employee>()
        {
            new Employee(new EmployeeIdentity(employeeId1), email1, employeeTitle1, createdTs),
            new Employee(new EmployeeIdentity(employeeId2), email2, employeeTitle2, createdTs),
        };

        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(true);

        request.Employees = employees.Select(e => new CreateCompanyEmployeeCommandRequest()
        {
            Id = e.Id.Value,
            Email = null,
            Title = null,
        })
        .ToList();

        var employeeIdsFromRequest = request.Employees
            .Select(e => e.Id!.Value)
            .ToList();

        mockEmployeeRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<EmployeeIdentity>>(
                    listOfEmployeeIds => listOfEmployeeIds
                        .All(ei => employeeIdsFromRequest
                            .Contains(ei.Value)))))
            .ReturnsAsync(employees.Take(1));

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(Errors.ApplicationErrors.Employee.EmployeeDoesNotExist().ErrorCode);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.IsAny<Company>()),
            Times.Never);

        mockEmployeeRepository
            .Verify(repo => repo.
                AddRange(It.IsAny<IEnumerable<Employee>>()),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Developer, EmployeeTitle.Tester)]
    public async Task Handle_EmployeeDoesNotExist_EmailIsNotUnique_ReturnError(
       EmployeeTitle employeeTitle1,
       EmployeeTitle employeeTitle2,
       string email1,
       string email2,
       Guid employeeId1,
       Guid employeeId2,
       [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
       [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
       [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
       [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
       CreateCompanyCommandRequestHandler sut,
       CreateCompanyCommandRequest request,
       Guid companyId)
    {
        var createdTs = new DateTime(2023, 10, 10);
        var employees = new List<Employee>()
        {
            new Employee(new EmployeeIdentity(employeeId1), email1, employeeTitle1, createdTs),
            new Employee(new EmployeeIdentity(employeeId2), email2, employeeTitle2, createdTs),
        };

        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        mockGuidGenerator
            .Setup(g => g
                .Generate())
            .Returns(companyId);

        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(true);

        request.Employees = employees.Select(e => new CreateCompanyEmployeeCommandRequest()
        {
            Id = null,
            Email = e.Email,
            Title = e.Title,
        })
        .ToList();

        var emailsFromRequest = request.Employees
            .Select(e => e.Email)
            .ToList();

        mockEmployeeRepository
            .Setup(repo => repo
                .AreEmailsUnique(emailsFromRequest!))
            .ReturnsAsync(false);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(Errors.ApplicationErrors.Employee.EmailIsNotUnique().ErrorCode);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.IsAny<Company>()),
            Times.Never);

        mockEmployeeRepository
            .Verify(repo => repo.
                AddRange(It.IsAny<IEnumerable<Employee>>()),
            Times.Never);
    }

    [Theory]
    [AutoMoqInlineData(EmployeeTitle.Developer)]
    [AutoMoqInlineData(EmployeeTitle.Tester)]
    [AutoMoqInlineData(EmployeeTitle.Manager)]
    public async Task Handle_EmployeeWithSameTitleAlreadyExistInCompany_ReturnError(
           EmployeeTitle employeeTitle,
           Guid employeeId1,
           Guid employeeId2,
           string email1,
           string email2,
           [Frozen] Mock<ICompanyRepository> mockCompanyRepository,
           [Frozen] Mock<IEmployeeRepository> mockEmployeeRepository,
           [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
           [Frozen] Mock<IGenerator<Guid>> mockGuidGenerator,
           CreateCompanyCommandRequest request,
           CreateCompanyCommandRequestHandler sut,
           Guid companyId)
    {
        var createdTs = new DateTime(2023, 10, 10);
        var employees = new List<Employee>()
        {
            new Employee(new EmployeeIdentity(employeeId1), email1, employeeTitle, createdTs),
            new Employee(new EmployeeIdentity(employeeId2), email2, employeeTitle, createdTs)
        };

        mockDateTimeProvider
            .Setup(d => d.Now())
            .Returns(createdTs);

        mockGuidGenerator
            .Setup(g => g
                .Generate())
            .Returns(companyId);

        mockCompanyRepository
            .Setup(repo => repo
                .IsNameUnique(request.Name))
            .ReturnsAsync(true);

        request.Employees = employees.Select(e => new CreateCompanyEmployeeCommandRequest()
        {
            Id = e.Id.Value,
            Email = null,
            Title = null,
        })
        .ToList();

        var employeeIdsFromRequest = request.Employees
            .Select(e => e.Id!.Value)
            .ToList();

        mockEmployeeRepository
            .Setup(repo => repo
                .GetByIds(It.Is<IEnumerable<EmployeeIdentity>>(
                    listOfEmployeeIds => listOfEmployeeIds
                        .All(ei => employeeIdsFromRequest
                            .Contains(ei.Value)))))
            .ReturnsAsync(employees);

        var result = await sut.Handle(request, default);

        result.IsSuccessful.Should().BeFalse();
        result.Errors.Select(e => e.ErrorCode).Should().Contain(DomainErrors.Company.CompanyPlaceForSpecificTitleFilled(employeeTitle).ErrorCode);

        mockCompanyRepository
            .Verify(repo => repo
                .Add(It.IsAny<Company>()),
            Times.Never);

        mockEmployeeRepository
            .Verify(repo => repo.
                AddRange(It.IsAny<IEnumerable<Employee>>()),
            Times.Never);
    }
}
