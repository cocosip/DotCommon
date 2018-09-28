namespace DotCommon.Log4Net
{
    public class Log4NetProviderOptions
    {
        /// <summary>默认log4net配置文件名
		/// </summary>
		private const string DefaultLog4NetConfigFile = "log4net.config";
        private const string DefaultLoggerRepositoryName = "DotCommonRepository";


        public string Name { get; set; }

        public string Log4NetConfigFile { get; set; }

        public string LoggerRepositoryName { get; set; }
        public Log4NetProviderOptions() : this(DefaultLog4NetConfigFile)
        {

        }

        public Log4NetProviderOptions(string configFile) : this(configFile, DefaultLoggerRepositoryName)
        {
        }

        public Log4NetProviderOptions(string configFile, string loggerRepositoryName)
        {
            Log4NetConfigFile = configFile;
            LoggerRepositoryName = loggerRepositoryName;
        }

    }
}
