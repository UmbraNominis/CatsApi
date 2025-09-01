namespace CatsApi.DTOs;

public class CatDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Likes { get; set; } = string.Empty;
    public string Dislikes { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Breed { get; set; } = string.Empty;
}
