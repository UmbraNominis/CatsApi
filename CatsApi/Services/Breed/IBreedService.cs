namespace CatsApi.Services.Breed;

using CatsApi.DTOs;
using CatsApi.FilterModels;

public interface IBreedService
{
    Task<BreedDTO> CreateAsync(BreedCreateDTO DTO);

    Task CreateFromListAsync(List<BreedCreateDTO> DTOs);

    Task<List<BreedDTO>> GetAllAsync(BreedFilter filter);

    Task<BreedDTO?> GetAsync(int id);

    Task<bool> UpdateAsync(int id, BreedCreateDTO DTO);

    Task<bool> DeleteAsync(int id);
}
