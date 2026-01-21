namespace EmpAnalytics.Application.Users.Get;

public record EmployeeExceedingOwnAverageResponse(
    string FirstName,
    string LastName,
    int JobCountLast30Days
);