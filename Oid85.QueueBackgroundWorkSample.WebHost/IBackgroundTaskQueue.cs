namespace Oid85.QueueBackgroundWorkSample.WebHost;

public interface IBackgroundTaskQueue
{
    Task QueueBackgroundWorkItemAsync(Func<Task> workItem);

    Task<Func<Task>> DequeueAsync();
}