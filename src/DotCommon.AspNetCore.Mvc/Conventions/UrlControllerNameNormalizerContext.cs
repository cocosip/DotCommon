namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>Url控制器名称格式化上下文
    /// </summary>
    public class UrlControllerNameNormalizerContext
    {
        /// <summary>根路径
        /// </summary>
        public string RootPath { get; }

        /// <summary>控制器名
        /// </summary>
        public string ControllerName { get; }

        /// <summary>Ctor
        /// </summary>
        public UrlControllerNameNormalizerContext(string rootPath, string controllerName)
        {
            RootPath = rootPath;
            ControllerName = controllerName;
        }
    }
}
