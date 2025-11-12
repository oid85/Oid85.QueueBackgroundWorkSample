using Oid85.QueueBackgroundWorkSample.WebHost.Extensions;

namespace Oid85.QueueBackgroundWorkSample.WebHost;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
            
        builder.Services.AddControllers();
        builder.Services.ConfigureSwagger(builder.Configuration);
        builder.Services.ConfigureCors(builder.Configuration);

        builder.Services.AddTransient<SomeService>();
        builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        builder.Services.AddHostedService<TaskQueueProcessor>();

        var app = builder.Build();
        
        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
        });
        
        app.MapControllers();

        await app.RunAsync();
    }
}