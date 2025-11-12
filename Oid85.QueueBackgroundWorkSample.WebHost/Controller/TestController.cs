using Microsoft.AspNetCore.Mvc;

namespace Oid85.QueueBackgroundWorkSample.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class TestController(
    ILogger<TestController> logger,
    SomeService someService,
    IBackgroundTaskQueue taskQueue) 
    : ControllerBase
{
    /// <summary>
    /// Загрузить справочник облигаций
    /// </summary>
    [HttpGet("test")]
    public async Task<IActionResult> TestAsync()
    {
        int number = DateTime.Now.Millisecond;

        taskQueue.QueueBackgroundWorkItem(async (serviceProvider, token) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<TestController>>();
            var someService = serviceProvider.GetRequiredService<SomeService>();

            logger.LogInformation("Job started");

            try
            {
                await someService.DoWork(number);
                logger.LogInformation("Job completed successfully.");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while job");
            }
        });

        return Ok();
    }
}