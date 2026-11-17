public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Customer(int id, string name, DateTime birthDate, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        BirthDate = DateOnly.FromDateTime(birthDate);
        Email = email;
        PhoneNumber = phoneNumber;
    }
}