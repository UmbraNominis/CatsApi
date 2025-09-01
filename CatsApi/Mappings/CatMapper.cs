namespace CatsApi.Mappings;

using CatsApi.Data.Models;
using CatsApi.DTOs;

public static class CatMapper
{
    public static CatDTO MapToDTO(this Cat cat)
    {
        return new CatDTO()
        {
            Id = cat.Id,
            Name = cat.Name,
            Likes = cat.Likes,
            Dislikes = cat.Dislikes,
            Age = cat.Age,
            Breed = cat.CatBreed.Name
        };
    }

    public static List<CatDTO> MapToDTO(this List<Cat> cats)
    {
        var DTOs = new List<CatDTO>();

        foreach (var cat in cats)
        {
            var DTO = new CatDTO()
            {
                Id = cat.Id,
                Name = cat.Name,
                Likes = cat.Likes,
                Dislikes = cat.Dislikes,
                Age = cat.Age,
                Breed = cat.CatBreed.Name
            };

            DTOs.Add(DTO);
        }

        return DTOs;
    }

    public static Cat MapToCat(this CatCreateDTO DTO)
    {
        return new Cat() 
        {
            Name = DTO.Name,
            Likes = DTO.Likes,
            Dislikes = DTO.Dislikes,
            Age = DTO.Age,
            CatBreedId = DTO.BreedId
        };
    }

    public static Cat MapToCat(this CatCreateDTO DTO, Cat cat)
    {
        cat.Name = DTO.Name;
        cat.Likes = DTO.Likes;
        cat.Dislikes = DTO.Dislikes;
        cat.Age = DTO.Age;

        return cat;
    }

    public static List<Cat> MapToCat(this List<CatCreateDTO> DTOs)
    {
        var cats = new List<Cat>();

        foreach(var DTO in DTOs)
        {
            cats.Add(DTO.MapToCat());
        }

        return cats;
    }
}
