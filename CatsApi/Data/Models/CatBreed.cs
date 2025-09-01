namespace CatsApi.Data.Models;

public class CatBreed
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Cat> Cats { get; } = new();
}
