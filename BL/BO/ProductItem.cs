
using static BO.Enums;

namespace BO;
//להציג לקונה את הקטלוג שלו!!
public class ProductItem
{
    #region product properties
    public event Action<BO.ProductItem> AddNewProduct;
    public int ID { get; set; }
    public string? Name { get; set; }
    public ECategory? Category { get; set; }
    public double Price { get; set; }
    public int InStock { get; set; }
    public int AmoutInYourCart { get; set; }
    private BO.ProductItem newProduct;
    public BO.ProductItem NewProduct
    {
        get { return newProduct; }
        set
        {
            if (newProduct is not null)
            {
                if (AddNewProduct != null)
                {
                    AddNewProduct(value);
                }
            }
            newProduct = value;
        }

    }


    #endregion

    #region methods
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the product item class</returns>
    public override string ToString() => $@"
    Product item ID={ID}: {Name}, 
    category - {Category}
    Price: {Price}
    Amount in stock: {InStock}
    Amout in your cart:{AmoutInYourCart}
";
    #endregion

}
