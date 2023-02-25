using BlApi;
using BlImplementation;
using BO;
using System.Diagnostics;
namespace Simulator;
public static class Simulator
{
    private static string? previousState;
    private static string? afterState;
    static bool finishFlag = false;
    public static event EventHandler StopSimulator;
    public static event EventHandler ProgressChange;
    public static void DoStop()
    {
        finishFlag = true;
        if (StopSimulator != null)
            StopSimulator("", EventArgs.Empty);
    }
    /// <summary>
    /// the function runs the program using maun thread
    /// </summary>
    public static void run()
    {
        Thread mainThreads = new Thread(new ThreadStart(chooseOrder));
        mainThreads.Start();
        return;
    }
    /// <summary>
    /// the function choose the order that has to be cared now.
    /// </summary>
    public static void chooseOrder()
    {
        IBl bl = new BlImplementation.Bl();
        int? id;
        while (!finishFlag)
        {
            id = bl.Order.getOrderToPromote();
            if (id == null)
                DoStop();
            else
            {
                try
                {
                    BO.Order o = bl.Order.GetOrderDetails((int)id);


                    previousState = o.Status.ToString();
                    Random rand = new Random();
                    int num = rand.Next(1000, 5000);
                    Details details = new Details(o, num);
                    if (ProgressChange != null)
                    {
                        ProgressChange(null, details);
                    }
                    Thread.Sleep(num);
                    afterState = (previousState == "Done" ? bl.Order.UpdateShipDate((int)id) : bl.Order.UpdateDeliveryDate((int)id)).Status.ToString();
                }
                catch (NegativeIdException u)
                {
                    return;

                }
            }
        }
        return;
    }
}

/// <summary>
/// class to define the things that are sended from the Simulator.cs to the window.
/// </summary>
public class Details : EventArgs
{
    public BO.Order order;
    public int seconds;
    public Details(BO.Order ord, int sec)
    {
        order = ord;
        seconds = sec;
    }
}