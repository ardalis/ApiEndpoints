using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Allows to use "[namespace]" as part of a route.
        /// </summary>
        public static void UseNamespaceRouteToken(this MvcOptions options)
        {
            options.Conventions.Add(new CustomRouteToken(
                "namespace",
                c => c.ControllerType.Namespace?.Split('.').Last() ?? string.Empty)
            );
        }

        private class CustomRouteToken : IApplicationModelConvention
        {
            private readonly string _tokenName;
            private readonly Func<ControllerModel, string> _valueGenerator;

            public CustomRouteToken(string tokenName, Func<ControllerModel, string> valueGenerator)
            {
                _tokenName = $"[{tokenName}]";
                _valueGenerator = valueGenerator;
            }

            public void Apply(ApplicationModel application)
            {
                foreach (var controller in application.Controllers)
                {
                    string tokenValue = _valueGenerator(controller);
                    UpdateSelectors(controller.Selectors, tokenValue);
                    UpdateSelectors(controller.Actions.SelectMany(a => a.Selectors), tokenValue);
                }
            }

            private void UpdateSelectors(IEnumerable<SelectorModel> selectors, string tokenValue)
            {
                foreach (var selector in selectors.Where(s => s.AttributeRouteModel != null))
                {
                    selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template
                        .Replace(_tokenName, tokenValue);
                }
            }
        }
    }
}
