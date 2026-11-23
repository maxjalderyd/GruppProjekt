public class MovieService
{
    private readonly Repository repo;
    public MovieService(Repository repo)
    {
        this.repo = repo;
    }

    public void CheckDateAndPrice()
    {
        repo.CheckDateAndPrice();
    }
    public List<Movie> GetAllMovies()
    {
        return repo.GetAllMovies();
    }
    public string DeleteMovie(string title)
    {
        return repo.DeleteMovie(title);
    }
    public void AddNewMovie(string name, DateTime releaseDatum, decimal price)
    {
        repo.AddNewMovie(name, releaseDatum, price);
    }
}