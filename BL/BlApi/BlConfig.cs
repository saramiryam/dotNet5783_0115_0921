using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlApi;

internal class BlConfig
{
    internal static string? s_BlName;
    internal static Dictionary<string, string> s_BlPackages;

    static BlConfig()
    {
        XElement BlConfig = XElement.Load(@"..\xml\dal-config.xml")
            ?? throw new DalConfigException("dal-config.xml file is not found");
        s_BlName = BlConfig?.Element("dal")?.Value
            ?? throw new DalConfigException("<dal> element is missing");
        var packages = BlConfig?.Element("dal-packages")?.Elements()
            ?? throw new DalConfigException("<dal-packages> element is missing");
        s_BlPackages = packages.ToDictionary(p => "" + p.Name, p => p.Value);
    }
}
