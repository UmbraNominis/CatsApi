namespace CatsApi.DTOs;

public class CatCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Likes { get; set; } = string.Empty;
    public string Dislikes { get; set; } = string.Empty;
    public int Age { get; set; }
    public int BreedId { get; set; }
}