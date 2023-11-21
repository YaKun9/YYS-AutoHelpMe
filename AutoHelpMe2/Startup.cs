using AutoHelpMe2.EventBus;
using Furion;
using Furion.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoHelpMe2
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();

            var eventPublisher = App.GetService<IEventPublisher>();

            services.AddConsoleFormatter(options =>
            {
                options.WriteHandler = (logMsg, _, _, _, _) =>
                {
                    switch (logMsg.LogLevel)
                    {
                        case LogLevel.Debug:
                        case LogLevel.Error:
                        case LogLevel.Information:
                        case LogLevel.Warning:
                            var log = $"{logMsg.LogDateTime:HH:mm:ss} {logMsg.LogLevel}：{logMsg.Message}";
                            eventPublisher.PublishAsync(new LogEventSource(logMsg.LogLevel, log));
                            break;
                    }
                };
            });

            services.AddFileLogging("logs/{0:yyyyMMdd}/app.log", options =>
            {
                options.FileNameRule = fileName => string.Format(fileName, DateTime.Now);
                options.Append = true;
                options.MinimumLevel = LogLevel.Debug;
                options.DateFormat = "HH:mm:ss";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}