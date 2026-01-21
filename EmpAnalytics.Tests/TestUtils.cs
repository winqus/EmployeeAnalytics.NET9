namespace EmpAnalytics.Tests;

public static class TestUtils
{
    public static Guid CreateGuid(int seed) => new Guid($"00000000-0000-0000-0000-{seed:D12}");
    
}