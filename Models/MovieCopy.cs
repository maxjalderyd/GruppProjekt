public class MovieCopy
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public bool IsAvailable { get; set; }
    public Movie Movie { get; set; }
    public MovieCopy(int id, string barcode, bool isAvailable, Movie movie)
    {
        Id = id;
        Barcode = barcode;
        IsAvailable = isAvailable;
        Movie = movie;
    }
}