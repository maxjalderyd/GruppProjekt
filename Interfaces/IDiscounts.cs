public interface IDiscounts
{
    string Name { get; set; } // Namn På Rabbaten tex "BlackWeek 20"
    bool IsApplicable { get; set; } // jämnför så att rabbaten är applicerbar tex 3 för 2. krävs att du har 3 filmer etc
    decimal CalculateTotalPrice(decimal subtotal); // räknar ut hur mycket kunden skall betala
}