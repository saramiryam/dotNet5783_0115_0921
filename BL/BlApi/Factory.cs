using DO;
using System.Reflection;

using static BlApi.BlConfig;
namespace BlApi;

public static class Factory
{
    public static IBl? Get()
    {
        string dalType = s_BlName
           ?? throw new DalConfigException($"DAL name is not extracted from the configuration");
        string dal = s_BlPackages[s_BlName]
           ?? throw new DalConfigException($"Package for {dalType} is not found in packages list");

        try
        {
            Assembly.Load(dal ?? throw new DalConfigException($"Package {dal} is null"));
        }
        catch (Exception)
        {
            throw new DalConfigException("Failed to load {dal}.dll package");
        }

        Type? type = Type.GetType($"Dal.{dal}, {dal}")
            ?? throw new DalConfigException($"Class Dal.{dal} was not found in {dal}.dll");

        return (IBl?)type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                   .GetValue(null) as IBl
            ?? throw new DalConfigException($"Class {dal} is not singleton or Instance property not found");
    }
}
