using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;

namespace ClientPlugin.Settings.Elements
{
    internal interface IBase
    {
        List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter);
    }
}
