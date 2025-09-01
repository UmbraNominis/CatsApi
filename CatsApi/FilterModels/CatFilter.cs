namespace CatsApi.FilterModels;

public class CatFilter
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Likes { get; set; }
    public string? Dislikes { get; set; }
    public int? Age { get; set; }
    public int? BreedId { get; set; }
}
