namespace CatsApi.DTOs;

public class BreedDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<CatDTO> Cats { get; } = new();
}
