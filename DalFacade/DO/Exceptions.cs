
namespace DO
{
    internal class RequestedItemNotFoundException:Exception
    {
        public string RequestedItemNotFound { get; set; }

        public RequestedItemNotFoundException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //RequestedItemNotFound

            //throw new RequestedItemNotFoundException("ערך זה כבר נמצא")
            //{ RequestedItemNotFound = val.ToString()};

        }

    }


    internal class ItemAlreadyExistsException: Exception
    {
        public string ItemAlreadyExists { get; set; }

        public ItemAlreadyExistsException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }
}
