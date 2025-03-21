using Microsoft.AspNetCore.Mvc.ApplicationModels;

public class RoutePrefixConvention : IControllerModelConvention
{
    private readonly string _prefix;

    public RoutePrefixConvention(string prefix)
    {
        _prefix = prefix;
    }

    public void Apply(ControllerModel controller)
    {
        // Add the prefix to each controller's route template
        foreach (var selector in controller.Selectors)
        {
            var template = $"{_prefix}/{controller.ControllerName}";
            selector.AttributeRouteModel = new AttributeRouteModel
            {
                Template = template
            };
        }
    }
}
