namespace DotCommon.Log4Net
{
    /// <summary>Log4Net配置信息
    /// </summary>
    public class Log4NetProviderOptions
    {
        /// <summary>默认log4net配置文件名
		/// </summary>
		private const string DefaultLog4NetConfigFile = "log4net.config";
        private const string DefaultLoggerRepositoryName = "DotCommonRepository";

        /// <summary>名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>配置文件
        /// </summary>
        public string Log4NetConfigFile { get; set; }

        /// <summary>仓库名
        /// </summary>
        public string LoggerRepositoryName { get; set; }

        /// <summary>Ctor
        /// </summary>
        public Log4NetProviderOptions() : this(DefaultLog4NetConfigFile)
        {

        }

        /// <summary>Ctor
        /// </summary>
        public Log4NetProviderOptions(string configFile) : this(configFile, DefaultLoggerRepositoryName)
        {

        }

        /// <summary>Ctor
        /// </summary>
        public Log4NetProviderOptions(string configFile, string loggerRepositoryName)
        {
            Log4NetConfigFile = configFile;
            LoggerRepositoryName = loggerRepositoryName;
        }

    }
}
