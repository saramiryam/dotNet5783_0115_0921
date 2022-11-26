namespace DO;

public class RequestedItemNotFoundException:Exception
{
    public string RequestedItemNotFound { get; set; }

    public RequestedItemNotFoundException(string msg) : base(msg)
    { 
        //RequestedItemNotFound

        //throw new RequestedItemNotFoundException("ערך זה לא נמצא")
        //{ RequestedItemNotFound = val.ToString()};

    }

}


public class ItemAlreadyExistsException: Exception
{
    public string ItemAlreadyExists { get; set; }

    public ItemAlreadyExistsException(string msg) : base(msg)
    {
        //לשאול בנות מה הן הוסיפו פה
        //ItemAlreadyExists
    }
}
