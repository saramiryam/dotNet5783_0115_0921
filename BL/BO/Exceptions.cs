
namespace BO
{
    #region product exceptions

    public class NegativeIdException : Exception
    {
        public string NegativeId { get; set; }

        public NegativeIdException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    public class EmptyNameException : Exception
    {
        public string EmptyName { get; set; }

        public EmptyNameException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    public class NegativePriceException : Exception
    {
        public string NegativePrice { get; set; }

        public NegativePriceException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    public class NegativeStockException : Exception
    {
        public string NegativeStock { get; set; }

        public NegativeStockException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    #endregion

}
