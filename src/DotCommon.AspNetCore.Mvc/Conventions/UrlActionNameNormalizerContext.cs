using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>UrlAction名称格式化上下文
    /// </summary>
    public class UrlActionNameNormalizerContext
    {
        /// <summary>根路径
        /// </summary>
        public string RootPath { get; }

        /// <summary>控制器名称
        /// </summary>
        public string ControllerName { get; }

        /// <summary>Action
        /// </summary>
        public ActionModel Action { get; }

        /// <summary>Url中的Action名称
        /// </summary>
        public string ActionNameInUrl { get; }

        /// <summary>Http方法
        /// </summary>
        public string HttpMethod { get; }

        /// <summary>Ctor
        /// </summary>
        public UrlActionNameNormalizerContext(string rootPath, string controllerName, ActionModel action, string actionNameInUrl, string httpMethod)
        {
            RootPath = rootPath;
            ControllerName = controllerName;
            Action = action;
            ActionNameInUrl = actionNameInUrl;
            HttpMethod = httpMethod;
        }
    }
}
