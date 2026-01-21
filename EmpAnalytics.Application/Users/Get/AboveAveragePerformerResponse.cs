namespace EmpAnalytics.Application.Users.Get;

public record AboveAveragePerformerResponse(
    string FirstName,
    string LastName,
    int JobCount
);