public interface IDiscounts
{
    string Name { get; set; } // Namn På Rabbaten tex "BlackWeek 20"
    bool IsApplicable { get; set; } // jämnför så att rabbaten är applicerbar tex 3 för 2. krävs att du har 3 filmer etc
    decimal CalculateTotalPrice(decimal subtotal, decimal totalcost); // räknar ut hur mycket kunden skall betala
}
public class AdjustablePercent : IDiscounts
{
    public string Name { get; set; }


    public bool IsApplicable
    {
        get
        {
            return CheckApplicable();

        }
        set
        {
            CheckApplicable();
            ;
        }
    }

    public AdjustablePercent(string name)
    {
        Name = name;
    }
    public decimal CalculateTotalPrice(decimal subtotal, decimal totalcost)
    {
        decimal cost = 0;
        cost = subtotal * totalcost;
        return totalcost - cost;
    }

    public decimal ChooseDiscount() // fixa returns
    {
        System.Console.WriteLine("hur många procent rabbat vill du ge?");
        string input = Console.ReadLine();
        if (decimal.TryParse(input, out decimal result))
        {
            System.Console.WriteLine($"gilitigt nummer {result}");
            return result / 100m;

        }
        else System.Console.WriteLine($"felaktig inmatning"); return 0;
    }
    static bool CheckApplicable()
    {
        return true;
    }
}