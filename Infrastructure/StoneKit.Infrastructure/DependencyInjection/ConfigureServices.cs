using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

using Newtonsoft.Json;

using Serilog;

using System.Caching;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder, AppConfigurationOptions config)
        {
            var isDevelopment = builder.Environment.IsDevelopment();

            if (config.SerilogOptions?.UseSerilog == true)
            {
                builder.Services.AddSerilog();
            }

            ValidateConfiguration(config);

            builder.ConfigureHttpService();
            builder.ConfigureRedis(config.AppName);

            builder.ConfigureOpenApi(config.AppName, config.OpenApiOptions, config.ValidatorAssembly);

            builder.Services.AddEndpointsApiExplorer();
            builder.WebHost.UseUrls();

            builder.Services.AddTransient<InitializeMiddleware>();
            builder.Services.AddTransient<ExceptionHandlerMiddleware>();

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddSignalR(options => { options.EnableDetailedErrors = isDevelopment; });

            builder.Services.AddFluentValidators(config);

            if (config.Cors != null)
            {
                builder.Services.AddCors(options => { options.AddPolicy("DEFAULT_CORD_POLICY", config.Cors); });
            }

            builder.Services.AddSingleton<IHasher, Hasher>();

            builder.Services.AddScoped<IOperationResultFactory, OperationResultFactory>();
            builder.Services.AddSingleton<OperationResultFactoryExample>();

        }

        private static void ValidateConfiguration(BaseConfiguration config)
        {
            if (string.IsNullOrEmpty(config?.AppName))
            {
                throw new Exception("Set AppName in the configuration");
            }
        }

        private static void AddFluentValidators(this IServiceCollection services, BaseConfiguration config)
        {
            if (config?.ValidatorAssembly == null || config?.UseFluentValidators != true)
            {
                return;
            }

            IEnumerable<Type> validatorTypes = from type in config.ValidatorAssembly.GetTypes()
                                               let info = type.GetTypeInfo()
                                               let baseType = info.BaseType
                                               where !info.IsAbstract && info.IsClass &&
                                                     baseType != null && baseType.GetTypeInfo().IsGenericType &&
                                                     (baseType.GetGenericTypeDefinition() == typeof(ValidatorBase<>) ||
                                                     baseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                                               select type;

            foreach (Type validatorType in validatorTypes)
            {
                services.AddScoped(validatorType);
            }


            //foreach (Type validatorType in validatorTypes)
            //{
            //    Type modelType = validatorType.BaseType.GetGenericArguments()[0];
            //    services.AddScoped < Func < Type, ValidatorBase < typeof(validatorType) >>> (provider => key =>
            //    {
            //        if (key == modelType)
            //        {
            //            return provider.GetService(validatorType);
            //        }

            //        return null;
            //    });
            //}
        }
    }
}
