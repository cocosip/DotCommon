using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotCommon.Log4Net
{
    public static class Log4NetExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            return factory.AddLog4Net(new Log4NetProviderOptions());
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configFile)
        {
            return factory.AddLog4Net(new Log4NetProviderOptions(configFile));
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Log4NetProviderOptions options)
        {
            factory.AddProvider(new Log4NetProvider(options));
            return factory;
        }


        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
        {
            var options = new Log4NetProviderOptions();
            return builder.AddLog4Net(options);
        }

        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string configFile)
        {
            var options = new Log4NetProviderOptions(configFile);
            return builder.AddLog4Net(options);
        }

        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, Log4NetProviderOptions options)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new Log4NetProvider(options));
            return builder;
        }


    }
}
