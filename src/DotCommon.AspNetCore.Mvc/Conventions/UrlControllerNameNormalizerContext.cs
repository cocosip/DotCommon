namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>Url���������Ƹ�ʽ��������
    /// </summary>
    public class UrlControllerNameNormalizerContext
    {
        /// <summary>��·��
        /// </summary>
        public string RootPath { get; }

        /// <summary>��������
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
