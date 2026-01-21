namespace EmpAnalytics.Application.Users.Get;

public record RecentlyActiveEmployeeResponse(
    string FirstName,
    string LastName,
    string JobName,
    DateTime LastJobDate
);