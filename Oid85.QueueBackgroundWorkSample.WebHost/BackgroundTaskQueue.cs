using System.Threading.Channels;

namespace Oid85.QueueBackgroundWorkSample.WebHost;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<Task>> _queue;

    public BackgroundTaskQueue(int capacity)
    {
        // Capacity should be set based on the expected application load and
        // number of concurrent threads accessing the queue.            
        // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
        // which completes only when space became available. This leads to backpressure,
        // in case too many publishers/calls start accumulating.
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        
        _queue = Channel.CreateBounded<Func<Task>>(options);
    }

    public async Task QueueBackgroundWorkItemAsync(Func<Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        await _queue.Writer.WriteAsync(workItem);
    }

    public async Task<Func<Task>> DequeueAsync()
    {
        var workItem = await _queue.Reader.ReadAsync();

        return workItem;
    }
}