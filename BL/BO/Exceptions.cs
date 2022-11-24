
namespace BO
{

    public class NegativeIdException : Exception
    {
        public string NegativeId { get; set; }

        public NegativeIdException(string msg) : base(msg)
        {
            //לשאול בנות מה הן הוסיפו פה
            //ItemAlreadyExists
        }
    }
}
