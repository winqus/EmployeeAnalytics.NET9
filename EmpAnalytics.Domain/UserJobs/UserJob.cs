namespace EmpAnalytics.Domain.UserJobs;

public class UserJob
{
    public UserJob(Guid userId, Guid jobId, DateTime dateTimeCreated)
    {
        UserId = userId;
        JobId = jobId;
        DateTimeCreated = dateTimeCreated;
    }

    public Guid UserId { get; private set; }

    public Guid JobId { get; private set; }

    public DateTime DateTimeCreated { get; private set; }
}