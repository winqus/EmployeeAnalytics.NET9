namespace EmpAnalytics.Domain.Jobs;

public class Job
{
    public Job(Guid jobId, string name)
    {
        JobId = jobId;
        Name = name;
    }

    public Guid JobId { get; private set; }

    public string Name { get; private set; } = string.Empty;
}