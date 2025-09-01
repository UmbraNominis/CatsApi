namespace CatsApi.Mappings;

using CatsApi.Data.Models;
using CatsApi.DTOs;

public static class BreedMapper
{
    public static BreedDTO MapToDTO(this CatBreed breed)
    {
        var DTO = new BreedDTO() 
        {
            Id = breed.Id,
            Name = breed.Name
        };

        DTO.Cats.AddRange(breed.Cats.MapToDTO());

        return DTO;
    }

    public static List<BreedDTO> MapToDTO(this List<CatBreed> breeds)
    {
        var DTOs = new List<BreedDTO>() { };

        foreach (var breed in breeds)
        {
            var DTO = new BreedDTO() 
            { 
                Id = breed.Id,
                Name = breed.Name 
            };

            DTO.Cats.AddRange(breed.Cats.MapToDTO());

            DTOs.Add(DTO);
        }

        return DTOs;
    }

    public static CatBreed MapToBreed(this BreedCreateDTO DTO, CatBreed breed)
    {
        breed.Name = DTO.Name;

        return breed;
    }

    public static CatBreed MapToBreed(this BreedCreateDTO DTO)
    {
        return new CatBreed()
        {
            Name = DTO.Name
        };
    }

    public static List<CatBreed> MapToBreed(this List<BreedCreateDTO> DTOs)
    {
        var breeds = new List<CatBreed>();

        foreach (var DTO in DTOs)
        {
            breeds.Add(DTO.MapToBreed());
        }

        return breeds;
    }
}
