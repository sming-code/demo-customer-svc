using Microsoft.Extensions.Hosting;

namespace SmingCode.Utilities.StartupProcesses.AspNetCore;

public interface IStartupProcessDelegateInvoker
{
    Task<bool> TryInvoke(
        IHost host,
        IServiceProvider serviceProvider,
        Delegate @delegate
    );
}
