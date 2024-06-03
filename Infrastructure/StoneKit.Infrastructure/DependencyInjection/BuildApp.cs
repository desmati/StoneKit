using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Microsoft.Extensions.Configuration
{
    public static class BuildAppExtensions
    {
        public static WebApplication BuildApp(this WebApplicationBuilder builder, AppConfigurationOptions config)
        {
            ValidateConfiguration(config);

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            config.InitializeMiddlewareAction?.Invoke();

            app.UseNoCacheHttpHeaders();
            app.UseRedirectValidation();
            app.UseReferrerPolicy(options => options.NoReferrer());
            app.UseXfo(options => options.Deny());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXRobotsTag(options => options.NoIndex().NoFollow().NoImageIndex());

            app.UseStaticFiles();

            if (config.OpenApiOptions?.Enabled == true)
            {
                var anyEndpoints = (config.OpenApiOptions.Endpoints?.Count ?? 0) > 0;
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    if (anyEndpoints)
                    {
                        foreach (string item in config.OpenApiOptions.Endpoints!)
                        {
                            options.SwaggerEndpoint($"/swagger/{item}/swagger.json", item);
                        }
                    }

                    options.DisplayRequestDuration();
                    options.DisplayOperationId();
                    options.EnableFilter();
                    options.ShowExtensions();

                    options.InjectStylesheet("/assets/themes/theme-material.css");
                });
            }


            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthorization();

            app.MapControllers();
            app.UseRouting();

            if (config.SerilogOptions?.UseSerilog == true)
            {
                app.UseSerilogRequestLogging(config.SerilogOptions?.Options);
            }

            return app;
        }

        private static void ValidateConfiguration(BaseConfiguration config)
        {
            if (string.IsNullOrEmpty(config?.AppName))
            {
                throw new Exception("Set AppName in the configuration");
            }
        }
    }
}
