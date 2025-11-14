namespace Oid85.QueueBackgroundWorkSample.WebHost;
public class BackgroundTaskQueueProcessor : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public BackgroundTaskQueueProcessor(IBackgroundTaskQueue taskQueue,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
    {
        _taskQueue = taskQueue;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<BackgroundTaskQueueProcessor>();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Queued Processor Background Service is starting.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(cancellationToken);
           
            try
            {
                if (workItem is not null)
                    _ = Task.Run(() => { workItem(_serviceProvider, cancellationToken); }, cancellationToken);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
            }

            await Task.Delay(1000, cancellationToken);
        }

        _logger.LogInformation("Queued Processor Background Service is stopping.");
    }
}