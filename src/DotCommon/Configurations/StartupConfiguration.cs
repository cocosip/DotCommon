namespace DotCommon.Configurations
{
    public class StartupConfiguration : DictionaryBasedConfig
    {
        public BackgroundWorkerConfiguration BackgroundWorker { get; set; } = new BackgroundWorkerConfiguration();
    }
}
