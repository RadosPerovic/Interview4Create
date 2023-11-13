using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Interview4Create.Project.Application.UnitTests.Mocks;
using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Domain.Common.Generators;
using Interview4Create.Project.Domain.Common.UnitOfWork;
using Interview4Create.Project.Domain.Companies.Factory;
using Interview4Create.Project.Domain.Employees.Factories;

namespace Interview4Create.Project.Application.UnitTests;
public class AutoMoqInlineDataAttribute : InlineAutoDataAttribute
{
    public AutoMoqInlineDataAttribute(params object?[]? values) : base(new AutoMoqAttribute(), values)
    {

    }

    private class AutoMoqAttribute : AutoDataAttribute
    {
        public AutoMoqAttribute() : base(FixtureFactory)
        {

        }

        private static IFixture FixtureFactory()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Register<IUnitOfWork>(() => new MockUnitOfWork());
            fixture.Register<IEmployeeFactory>(() =>
            {
                var dateTimeProvider = fixture.Create<IDateTimeProvider>();
                var guidGenerator = fixture.Create<IGenerator<Guid>>();

                return new EmployeeFactory(guidGenerator, dateTimeProvider);
            });
            fixture.Register<ICompanyFactory>(() =>
            {
                var dateTimeProvider = fixture.Create<IDateTimeProvider>();
                var guidGenerator = fixture.Create<IGenerator<Guid>>();

                return new CompanyFactory(guidGenerator, dateTimeProvider);
            });

            return fixture;
        }
    }
}
