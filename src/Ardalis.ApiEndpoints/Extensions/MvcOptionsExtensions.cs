using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcOptionsExtensions
    {
        public static void UseNamespaceRouteToken(this MvcOptions options)
        {
            options.Conventions.Add(new NamespaceRoutingConvention());
        }

        private class NamespaceRoutingConvention : IApplicationModelConvention
        {
            public void Apply(ApplicationModel application)
            {
                foreach (var controller in application.Controllers)
                {
                    string namespaceValue = controller.ControllerType.Namespace?.Split('.').Last() ?? string.Empty;
                    UpdateSelectors(controller.Selectors, namespaceValue);
                    UpdateSelectors(controller.Actions.SelectMany(a => a.Selectors), namespaceValue);
                }
            }

            private static void UpdateSelectors(IEnumerable<SelectorModel> selectors, string namespaceValue)
            {
                foreach (var selector in selectors.Where(s => s.AttributeRouteModel != null))
                {
                    selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template
                        .Replace("[namespace]", namespaceValue);
                }
            }
        }
    }
}
