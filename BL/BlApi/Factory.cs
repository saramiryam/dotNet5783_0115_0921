using BlImplementation;
using BO;
using DalApi;
using DO;
using System.Reflection;


namespace BlApi;

public static class Factory
{
    public static IBl? Get()
    {
        Bl bl = new Bl();
        return bl;
    }
}
