using Microsoft.AspNetCore.Mvc;

namespace Oid85.QueueBackgroundWorkSample.WebHost.Controller;

[Route("api/[controller]")]
[ApiController]
public class TestController(
    ILogger<TestController> logger,
    SomeService someService,
    IBackgroundTaskQueue backgroundTaskQueue) 
    : ControllerBase
{
    /// <summary>
    /// Загрузить справочник облигаций
    /// </summary>
    [HttpGet("test")]
    public async Task<IActionResult> TestAsync()
    {
        await backgroundTaskQueue.QueueBackgroundWorkItemAsync(someService.DoWork);
        
        logger.LogInformation("Request is completed");
        
        return Ok();
    }
}