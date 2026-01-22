namespace EmpAnalytics.Tests.API;

public class BaseFunctionalTest(FunctionalTestWebAppFactory factory) : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; init; } = factory.CreateClient();
}