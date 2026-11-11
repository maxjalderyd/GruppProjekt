public interface IDiscounts
{
    string Name { get; set; } // Namn På Rabbaten tex "BlackWeek 20"
    bool IsApplicable { get; set; } // jämnför så att rabbaten är applicerbar tex 3 för 2. krävs att du har 3 filmer etc
    decimal CalculateTotalPrice(decimal subtotal); // räknar ut hur mycket kunden skall betala
}
public class AdjustablePercent : IDiscounts
{
    public string Name { get; set; }

    private decimal _discountRate;
    public decimal DiscountRate
    {
        get
        {
            return _discountRate;
        }
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentException("Rabatt måste vara mellan 0 och 100%");
            }
            _discountRate = value;
        }
    }


    public bool IsApplicable { get; set; } = true;

    public AdjustablePercent(string name)
    {
        Name = name;
    }
    public decimal CalculateTotalPrice(decimal subtotal)
    {
        decimal Discount = subtotal * DiscountRate;
        return subtotal - Discount;
    }

    public decimal ChooseDiscount()
    {
        System.Console.WriteLine("hur många procent rabbat vill du ge?");
        string input = Console.ReadLine();
        if (decimal.TryParse(input, out decimal result))
        {
            System.Console.WriteLine($"gilitigt nummer {result}");
            DiscountRate = result / 100m;
            return DiscountRate;
        }
        else System.Console.WriteLine($"felaktig inmatning"); return 0;
    }

}