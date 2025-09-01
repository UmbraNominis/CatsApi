namespace CatsApi.Data.Models;

public class Cat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Likes { get; set; } = string.Empty;
    public string Dislikes { get; set; } = string.Empty;
    public int Age { get; set; }

    public int CatBreedId { get; set; }
    public CatBreed CatBreed { get; set; } = null!;
}
