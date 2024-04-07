namespace Ghost.Services;

public interface IEmailDaemon : IHostedService, IDisposable
{
    void SendRegular();
    void SendImmediate();
}