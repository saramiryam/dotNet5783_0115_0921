using static BO.Enums;
namespace BO;
//מוצר כללי בקטלוג ראשי
public class ProductForList
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ECategory Category { get; set; }
    public double Price { get; set; }
    
}
