using BO;
using DalApi;
using DO;
using System.Reflection;

using static BlApi.BlConfig;
namespace BlApi;

public static class Factory
{
    public static IBl? Get()
    {
        string blType = s_BlName
           ?? throw new BlConfigException($"DAL name is not extracted from the configuration");
        string bl = s_BlPackages[s_BlName]
           ?? throw new BlConfigException($"Package for {blType} is not found in packages list");

        try
        {
            Assembly.Load(bl ?? throw new BlConfigException($"Package {bl} is null"));
        }
        catch (Exception)
        {
            throw new BlConfigException("Failed to load {dal}.dll package");
        }
        Type? type = Type.GetType($"Dal.{bl}, {bl}")
        ?? throw new BlConfigException($"Class Dal.{bl} was not found in {bl}.dll");

        return type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                   .GetValue(null) as IBl
            ?? throw new BlConfigException($"Class {bl} is not singleton or Instance property not found");
    }
}
