using System;
using System.Text.Encodings.Web;
using DotCommon.Json.SystemTextJson;
using DotCommon.Json.SystemTextJson.JsonConverters;
using DotCommon.Json.SystemTextJson.Modifiers;
using DotCommon.ObjectMapping;
using DotCommon.Scheduling;
using DotCommon.Serialization;
using DotCommon.Threading;
using DotCommon.Timing;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service collection extensions for dependency injection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers DotCommon services
        /// </summary>
        public static IServiceCollection AddDotCommon(this IServiceCollection services)
        {
            // JSON serialization
            services
                .AddDotCommonSchedule()
                .AddDotCommonSerialization()
                .AddDotCommonObjectMapper()
                .AddDotCommonThreading()
                .AddDotCommonTiming()
                .AddDotCommonSystemTextJson();
            return services;
        }

        /// <summary>
        /// Adds scheduling services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonSchedule(this IServiceCollection services)
        {
            services.AddSingleton<IScheduleService, ScheduleService>();
            return services;
        }

        /// <summary>
        /// Adds serialization services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonSerialization(this IServiceCollection services)
        {
            services.AddTransient<IObjectSerializer, DefaultObjectSerializer>();
            return services;
        }

        /// <summary>
        /// Adds object mapping services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonObjectMapper(this IServiceCollection services)
        {
            services
                .AddTransient<IObjectMapper, DefaultObjectMapper>()
                .AddTransient(typeof(IObjectMapper<>), typeof(DefaultObjectMapper<>))
                .AddSingleton<IAutoObjectMappingProvider, NotImplementedAutoObjectMappingProvider>();
            return services;
        }

        /// <summary>
        /// Adds threading services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonThreading(this IServiceCollection services)
        {
            services
                .AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance)
                .AddSingleton<IAmbientDataContext, AsyncLocalAmbientDataContext>()
                .AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
            return services;
        }

        /// <summary>
        /// Adds timing services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonTiming(this IServiceCollection services)
        {
            services
                .AddTransient<ITimezoneProvider, TZConvertTimezoneProvider>()
                .AddTransient<IClock, Clock>()
                .AddSingleton<ICurrentTimezoneProvider, CurrentTimezoneProvider>()
                ;
            return services;
        }

        /// <summary>
        /// Adds System.Text.Json services with custom converters and configurations
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonSystemTextJson(this IServiceCollection services)
        {
            services
                .AddTransient<DotCommonDateTimeConverter>()
                .AddTransient<DotCommonNullableDateTimeConverter>()
                .AddTransient<DotCommon.Json.IJsonSerializer, DotCommonSystemTextJsonSerializer>()
                ;

            services.AddOptions<DotCommonSystemTextJsonSerializerOptions>()
                 .Configure<IServiceProvider>((options, rootServiceProvider) =>
                 {
                     // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
                     options.JsonSerializerOptions.Encoder ??= JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                     options.JsonSerializerOptions.Converters.Add(new DotCommonStringToEnumFactory());
                     options.JsonSerializerOptions.Converters.Add(new DotCommonStringToBooleanConverter());
                     options.JsonSerializerOptions.Converters.Add(new DotCommonStringToGuidConverter());
                     options.JsonSerializerOptions.Converters.Add(new DotCommonNullableStringToGuidConverter());
                     options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                     options.JsonSerializerOptions.TypeInfoResolver = new DotCommonDefaultJsonTypeInfoResolver(rootServiceProvider
                        .GetRequiredService<IOptions<DotCommonSystemTextJsonSerializerModifiersOptions>>());

                     var dateTimeConverter = rootServiceProvider.GetRequiredService<DotCommonDateTimeConverter>().SkipDateTimeNormalization();
                     var nullableDateTimeConverter = rootServiceProvider.GetRequiredService<DotCommonNullableDateTimeConverter>().SkipDateTimeNormalization();

                     options.JsonSerializerOptions.TypeInfoResolver.As<DotCommonDefaultJsonTypeInfoResolver>().Modifiers.Add(
                        new DotCommonDateTimeConverterModifier(dateTimeConverter, nullableDateTimeConverter)
                            .CreateModifyAction());
                 });

            return services;
        }

    }
}
