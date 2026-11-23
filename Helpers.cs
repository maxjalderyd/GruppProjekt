using System.Text.RegularExpressions;

class Helpers
{
    // PLACEHOLDER LOL
    public static bool IsValidBarcode(string barcode)
    {
        return Regex.IsMatch(barcode, @"^BC-[A-Z]+-\d{3}$");
    }

        public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public static bool IsValidName(string name)
    {
        return Regex.IsMatch(name.Trim(), @"\A[\p{L}]+([ '-][\p{L}]+)+\z");
    }

    public static bool IsValidPhoneNumber(string phone)
    {
        return Regex.IsMatch(phone, @"^(\+46|0)7[0-9]{8}$");
    }
}