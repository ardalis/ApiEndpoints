using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class MvcOptionsExtensions
  {
    /// <summary>
    /// Allows to use "[namespace]" as part of a route.
    /// </summary>
    public static MvcOptions UseNamespaceRouteToken(this MvcOptions options)
    {
      options.Conventions.Add(new CustomRouteToken(
          "namespace",
          c => c.ControllerType.Namespace?.Split('.').Last()
      ));

      return options;
    }

    private class CustomRouteToken : IApplicationModelConvention
    {
      private readonly string _tokenRegex;
      private readonly Func<ControllerModel, string?> _valueGenerator;

      public CustomRouteToken(string tokenName, Func<ControllerModel, string?> valueGenerator)
      {
        _tokenRegex = $@"(\[{tokenName}])(?<!\[\1(?=]))";
        _valueGenerator = valueGenerator;
      }

      public void Apply(ApplicationModel application)
      {
        foreach (var controller in application.Controllers)
        {
          string? tokenValue = _valueGenerator(controller);
          UpdateSelectors(controller.Selectors, tokenValue);
          UpdateSelectors(controller.Actions.SelectMany(a => a.Selectors), tokenValue);
        }
      }

      private void UpdateSelectors(IEnumerable<SelectorModel> selectors, string? tokenValue)
      {
        foreach (var selector in selectors.Where(s => s.AttributeRouteModel != null))
        {
          selector.AttributeRouteModel.Template = InsertTokenValue(selector.AttributeRouteModel.Template, tokenValue);
          selector.AttributeRouteModel.Name = InsertTokenValue(selector.AttributeRouteModel.Name, tokenValue);
        }
      }

      private string? InsertTokenValue(string? template, string? tokenValue)
      {
        if (template is null)
        {
          return template;
        }

        return Regex.Replace(template, _tokenRegex, tokenValue);
      }
    }
  }
}
