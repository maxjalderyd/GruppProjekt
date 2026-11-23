public class CustomerService
{
    private readonly Repository repo;
    public CustomerService(Repository repo)
    {
        this.repo = repo;
    }

    public Customer? GetCustomer(string name, DateOnly birthDate, string mail, string phone)
    {
        return repo.GetCustomer(name, birthDate, mail, phone);
    }
    public int CreateNewCustomer(string name, DateOnly birthDate, string email, string phoneNumber)
    {
        return repo.CreateNewCustomer(name, birthDate, email, phoneNumber);
    }
}