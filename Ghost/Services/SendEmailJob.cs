using Quartz;

namespace Ghost.Services;

public class SendEmailJob : IJob
{
    private readonly IEmailDaemon _daemon;

    public SendEmailJob(IEmailDaemon daemon)
    {
        _daemon = daemon;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _daemon.SendRegular();

        return Task.FromResult(true);
    }
}