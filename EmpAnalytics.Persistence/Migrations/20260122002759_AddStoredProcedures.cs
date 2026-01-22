using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpAnalytics.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE PROCEDURE GetTop10RecentlyActiveEmployees
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    SELECT TOP 10
                        u.Username AS FirstName,
                        u.Lastname AS LastName,
                        j.Name AS JobName,
                        uj.DateTimeCreated AS LastJobDate
                    FROM UserJobs uj
                    INNER JOIN Users u ON uj.UserId = u.UserId
                    INNER JOIN Jobs j ON uj.JobId = j.JobId
                    ORDER BY uj.DateTimeCreated DESC;
                END
                """);

            migrationBuilder.Sql("""
                CREATE PROCEDURE GetTop5AboveAveragePerformers
                    @StartDate DATETIME2,
                    @EndDate DATETIME2
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    WITH JobCounts AS (
                        SELECT 
                            uj.UserId,
                            COUNT(*) AS JobCount
                        FROM UserJobs uj
                        WHERE uj.DateTimeCreated >= @StartDate 
                          AND uj.DateTimeCreated <= @EndDate
                        GROUP BY uj.UserId
                    ),
                    AvgJobCount AS (
                        SELECT AVG(CAST(JobCount AS FLOAT)) AS AvgCount
                        FROM JobCounts
                    )
                    SELECT TOP 5
                        u.Username AS FirstName,
                        u.Lastname AS LastName,
                        jc.JobCount
                    FROM JobCounts jc
                    INNER JOIN Users u ON jc.UserId = u.UserId
                    CROSS JOIN AvgJobCount avg
                    WHERE jc.JobCount > avg.AvgCount
                    ORDER BY jc.JobCount DESC;
                END
                """);

            migrationBuilder.Sql("""
                CREATE PROCEDURE GetTop5EmployeesExceedingOwnAverage
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Now DATETIME2 = GETUTCDATE();
                    DECLARE @Last30Days DATETIME2 = DATEADD(DAY, -30, @Now);
                    DECLARE @Last6Months DATETIME2 = DATEADD(MONTH, -6, @Now);
                    
                    SELECT TOP 5
                        u.Username AS FirstName,
                        u.Lastname AS LastName,
                        (
                            SELECT COUNT(*)
                            FROM UserJobs uj
                            WHERE uj.UserId = u.UserId 
                              AND uj.DateTimeCreated >= @Last30Days
                        ) AS JobCountLast30Days
                    FROM Users u
                    WHERE (
                        SELECT COUNT(*)
                        FROM UserJobs uj
                        WHERE uj.UserId = u.UserId 
                          AND uj.DateTimeCreated >= @Last30Days
                    ) > (
                        SELECT COUNT(*) / 6.0
                        FROM UserJobs uj
                        WHERE uj.UserId = u.UserId 
                          AND uj.DateTimeCreated >= @Last6Months
                    )
                    ORDER BY JobCountLast30Days DESC;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTop10RecentlyActiveEmployees");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTop5AboveAveragePerformers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTop5EmployeesExceedingOwnAverage");
        }
    }
}
