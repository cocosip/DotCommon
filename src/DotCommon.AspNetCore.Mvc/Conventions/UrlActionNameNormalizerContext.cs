using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>UrlAction���Ƹ�ʽ��������
    /// </summary>
    public class UrlActionNameNormalizerContext
    {
        /// <summary>��·��
        /// </summary>
        public string RootPath { get; }

        /// <summary>����������
        /// </summary>
        public string ControllerName { get; }

        /// <summary>Action
        /// </summary>
        public ActionModel Action { get; }

        /// <summary>Url�е�Action����
        /// </summary>
        public string ActionNameInUrl { get; }

        /// <summary>Http����
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
