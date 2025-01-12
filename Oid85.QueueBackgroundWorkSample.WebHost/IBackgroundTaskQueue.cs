namespace Oid85.QueueBackgroundWorkSample.WebHost;

public interface IBackgroundTaskQueue
{
    Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken);
}