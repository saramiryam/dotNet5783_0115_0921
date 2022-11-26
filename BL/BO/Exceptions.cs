
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

    public class ProductAlreadyExistsException : Exception
    {
        public string ProductAlreadyExists { get; set; }

        public ProductAlreadyExistsException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    public class ProductNotExistsException : Exception
    {
        public string ProductNotExists { get; set; }

        public ProductNotExistsException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

     public class ProductInUseException : Exception
    {
        public string ProductInUse { get; set; }

        public ProductInUseException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }
    #endregion


    
    #region order item exceptions

          public class NotEnoughInStockException : Exception
    {
        public string NotEnoughInStock { get; set; }

        public NotEnoughInStockException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    
          public class ProductNotInStockException : Exception
    {
        public string ProductNotInStock { get; set; }

        public ProductNotInStockException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }

    #endregion

}
