using DotCommon.AspNetCore.Mvc.Formatters;
using DotCommon.Extensions;
using DotCommon.Reflecting;
using DotCommon.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    public class ServiceConvention : IServiceConvention
    {
        private const string DefaultRootPath = "app";

        private readonly string[] CommonPostfixes = { "AppService", "ApplicationService", "Service" };

        private readonly DotCommonAspNetCoreMvcOptions _options;
        private readonly MvcOptions _mvcOptions;
        public ServiceConvention(IOptions<MvcOptions> mvcOptions, IOptions<DotCommonAspNetCoreMvcOptions> options)
        {
            _mvcOptions = mvcOptions.Value;
            _options = options.Value;
        }

        public void Apply(ApplicationModel application)
        {
            ApplyForControllers(application);
        }

        protected virtual void ApplyForControllers(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var controllerType = controller.ControllerType.AsType();
                var configuration = GetControllerSettingOrNull(controllerType);

                //We can remove different behaviour for ImplementsRemoteServiceInterface. If there is a configuration, then it should be applied!
                //But also consider ConventionalControllerSetting.IsRemoteService method too..!

                if (ImplementsRemoteServiceInterface(controllerType))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(CommonPostfixes);
                    configuration?.ControllerModelConfigurer?.Invoke(controller);
                    ConfigureRemoteService(controller, configuration);
                }
                else
                {
                    var remoteServiceAttr = ReflectionUtil.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
                    if (remoteServiceAttr != null && remoteServiceAttr.IsEnabledFor(controllerType))
                    {
                        ConfigureRemoteService(controller, configuration);
                    }
                }
            }
        }

        protected virtual void ConfigureRemoteService(ControllerModel controller, ConventionalControllerSetting configuration)
        {
            ConfigureApiExplorer(controller);
            ConfigureControllerSelector(controller, configuration);
            ConfigureParameters(controller);
        }

        protected virtual void ConfigureParameters(ControllerModel controller)
        {
            /* Default binding system of Asp.Net Core for a parameter
             * 1. Form values
             * 2. Route values.
             * 3. Query string.
             */

            foreach (var action in controller.Actions)
            {
                foreach (var prm in action.Parameters)
                {
                    if (prm.BindingInfo != null)
                    {
                        continue;
                    }

                    //如果是基础类型,不能用Frombody,如果是string类型的,并且添加了RawRequestBodyFormatter可以用Frombody
                    if (!TypeUtil.IsPrimitiveExtended(prm.ParameterInfo.ParameterType))
                    //if (!TypeHelper.IsPrimitiveExtended(prm.ParameterInfo.ParameterType) || CanUseRawRequestBodyFormatter(prm.ParameterInfo.ParameterType))
                    {
                        if (CanUseFormBodyBinding(action, prm))
                        {
                            prm.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                        }
                    }
                }
            }
        }

        protected virtual bool CanUseRawRequestBodyFormatter(Type type)
        {
            return type == typeof(string) && _mvcOptions.InputFormatters.Any(x => x.GetType() == typeof(RawRequestBodyFormatter));
        }

        protected virtual bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (_options.ConventionalControllers.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {
                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected virtual void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                var controllerType = controller.ControllerType.AsType();
                var remoteServiceAtt = ReflectionUtil.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
                if (remoteServiceAtt != null)
                {
                    controller.ApiExplorer.IsVisible =
                        remoteServiceAtt.IsEnabledFor(controllerType) &&
                        remoteServiceAtt.IsMetadataEnabledFor(controllerType);
                }
                else
                {
                    controller.ApiExplorer.IsVisible = true;
                }
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }

        protected virtual void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                var remoteServiceAtt = ReflectionUtil.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
                if (remoteServiceAtt != null)
                {
                    action.ApiExplorer.IsVisible =
                        remoteServiceAtt.IsEnabledFor(action.ActionMethod) &&
                        remoteServiceAtt.IsMetadataEnabledFor(action.ActionMethod);
                }
            }
        }

        protected virtual void ConfigureControllerSelector(ControllerModel controller, ConventionalControllerSetting configuration)
        {
            RemoveEmptySelectors(controller.Selectors);

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }
           

            var rootPath = GetRootPathOrDefault(controller.ControllerType.AsType());

            foreach (var action in controller.Actions)
            {
                ConfigureActionSelector(rootPath, controller.ControllerName, action, configuration);
            }
        }

        protected virtual void ConfigureActionSelector(string rootPath, string controllerName, ActionModel action, ConventionalControllerSetting configuration)
        {
            RemoveEmptySelectors(action.Selectors);

            if (!action.Selectors.Any())
            {
                AddServiceSelector(rootPath, controllerName, action, configuration);
            }
            else
            {
                NormalizeSelectorRoutes(rootPath, controllerName, action, configuration);
            }
        }

        protected virtual void AddServiceSelector(string rootPath, string controllerName, ActionModel action, ConventionalControllerSetting configuration)
        {
            //根据方法名称生成Http请求的路径
            var httpMethod = SelectHttpMethod(action, configuration);
            var routeModel = CreateServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod, configuration);
            var serviceSelectorModel = new SelectorModel
            {
                AttributeRouteModel = routeModel,
                ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod }) },
                //  EndpointMetadata = { new RouteAttribute(routeModel.Template) }
            };

            action.Selectors.Add(serviceSelectorModel);
        }

        protected virtual string SelectHttpMethod(ActionModel action, ConventionalControllerSetting configuration)
        {
            return HttpMethodUtil.GetConventionalVerbForMethodName(action.ActionName);
        }

        protected virtual void NormalizeSelectorRoutes(string rootPath, string controllerName, ActionModel action, ConventionalControllerSetting configuration)
        {
            foreach (var selector in action.Selectors)
            {
                var httpMethod = selector.ActionConstraints.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods?.FirstOrDefault();
                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = CreateServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod, configuration);
                }
            }
        }

        protected virtual string GetRootPathOrDefault(Type controllerType)
        {
            var controllerSetting = GetControllerSettingOrNull(controllerType);
            if (controllerSetting?.RootPath != null)
            {
                return GetControllerSettingOrNull(controllerType)?.RootPath;
            }

            var areaAttribute = controllerType.GetCustomAttributes().OfType<AreaAttribute>().FirstOrDefault();
            if (areaAttribute.RouteValue != null)
            {
                return areaAttribute.RouteValue;
            }

            return DefaultRootPath;
        }

        protected virtual ConventionalControllerSetting GetControllerSettingOrNull(Type controllerType)
        {
            return _options.ConventionalControllers.ConventionalControllerSettings.GetSettingOrNull(controllerType);
        }

        protected virtual AttributeRouteModel CreateServiceAttributeRouteModel(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            return new AttributeRouteModel(
                new RouteAttribute(
                    CalculateRouteTemplate(rootPath, controllerName, action, httpMethod, configuration)
                )
            );
        }

        protected virtual string CalculateRouteTemplate(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            var controllerNameInUrl = NormalizeUrlControllerName(rootPath, controllerName, action, httpMethod, configuration);

            var url = $"api/{rootPath}/{controllerNameInUrl.ToCamelCase()}";

            //Add {id} path if needed
            if (action.Parameters.Any(p => p.ParameterName == "id"))
            {
                url += "/{id}";
            }

            //Add action name if needed
            var actionNameInUrl = NormalizeUrlActionName(rootPath, controllerName, action, httpMethod, configuration);
            if (!actionNameInUrl.IsNullOrEmpty())
            {
                url += $"/{actionNameInUrl.ToCamelCase()}";

                //Add secondary Id
                var secondaryIds = action.Parameters.Where(p => p.ParameterName.EndsWith("Id", StringComparison.Ordinal)).ToList();
                if (secondaryIds.Count == 1)
                {
                    url += $"/{{{secondaryIds[0].ParameterName}}}";
                }
            }

            return url;
        }

        protected virtual string NormalizeUrlActionName(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            var actionNameInUrl = HttpMethodUtil
                .RemoveHttpMethodPrefix(action.ActionName, httpMethod)
                .RemovePostFix("Async");

            if (configuration?.UrlActionNameNormalizer == null)
            {
                return actionNameInUrl;
            }

            return configuration.UrlActionNameNormalizer(
                new UrlActionNameNormalizerContext(
                    rootPath,
                    controllerName,
                    action,
                    actionNameInUrl,
                    httpMethod
                )
            );
        }

        protected virtual string NormalizeUrlControllerName(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            if (configuration?.UrlControllerNameNormalizer == null)
            {
                return controllerName;
            }

            return configuration.UrlControllerNameNormalizer(
                new UrlControllerNameNormalizerContext(
                    rootPath,
                    controllerName
                )
            );
        }

        protected virtual void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            selectors
                .Where(IsEmptySelector)
                .ToList()
                .ForEach(s => selectors.Remove(s));
        }

        protected virtual bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null && selector.ActionConstraints.IsNullOrEmpty();
        }

        protected virtual bool ImplementsRemoteServiceInterface(Type controllerType)
        {
            return typeof(IRemoteService).GetTypeInfo().IsAssignableFrom(controllerType);
        }


    }
}
