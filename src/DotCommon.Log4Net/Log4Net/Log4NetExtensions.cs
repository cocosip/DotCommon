using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotCommon.Log4Net
{
    /// <summary>Log4Net扩展
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>添加Log4Net
        /// </summary>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            return factory.AddLog4Net(new Log4NetProviderOptions());
        }

        /// <summary>添加Log4Net
        /// </summary>
        /// <param name="factory">工厂</param>
        /// <param name="configFile">配置文件</param>
        /// <returns></returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configFile)
        {
            return factory.AddLog4Net(new Log4NetProviderOptions(configFile));
        }

        /// <summary>添加Log4Net
        /// </summary>
        /// <param name="factory">工厂</param>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Log4NetProviderOptions options)
        {
            factory.AddProvider(new Log4NetProvider(options));
            return factory;
        }

        /// <summary>添加Log4Net
        /// </summary>
        /// <param name="builder">ILoggingBuilder</param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
        {
            var options = new Log4NetProviderOptions();
            return builder.AddLog4Net(options);
        }

        /// <summary>添加Log4Net
        /// </summary>
        /// <param name="builder">ILoggingBuilder</param>
        /// <param name="configFile">配置文件</param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string configFile)
        {
            var options = new Log4NetProviderOptions(configFile);
            return builder.AddLog4Net(options);
        }

        /// <summary>添加Log4Net
        /// </summary>
        /// <param name="builder">ILoggingBuilder</param>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, Log4NetProviderOptions options)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new Log4NetProvider(options));
            return builder;
        }


    }
}
