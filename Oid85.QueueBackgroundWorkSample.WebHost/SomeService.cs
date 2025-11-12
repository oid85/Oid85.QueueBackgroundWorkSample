namespace Oid85.QueueBackgroundWorkSample.WebHost;

public class SomeService(ILogger<SomeService> logger)
{
    public async Task DoWork(int number)
    {
        await Task.Delay(10_000);
        
        logger.LogInformation($"DoWork {number} completed");
    }
}