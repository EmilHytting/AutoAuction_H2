using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var vmType = param.GetType();
        var name = vmType.FullName!
            .Replace(".ViewModels.", ".Views.", StringComparison.Ordinal)
            .Replace("ViewModel", "View", StringComparison.Ordinal);

        var type = Type.GetType(name);

        if (type is null)
        {
            var contentPanelsName = name.Replace(".Views.", ".Views.ContentPanels.", StringComparison.Ordinal);
            type = Type.GetType(contentPanelsName);
        }

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = param;           // ✅ VIGTIGT!
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
