namespace SmingCode.Utilities.StartupProcesses.AspNetCore;

public interface IServiceInitializer
{
    Delegate ServiceInitializer { get; }
}