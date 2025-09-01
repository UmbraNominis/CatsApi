namespace CatsApi.Services.Cat;

using CatsApi.DTOs;
using CatsApi.FilterModels;

public interface ICatService
{
    Task<CatDTO> CreateAsync(CatCreateDTO DTO);

    Task CreateFromListAsync(List<CatCreateDTO> DTOs);

    Task<List<CatDTO>> GetAllAsync(CatFilter filter);

    Task<CatDTO?> GetAsync(int id);

    Task<bool> UpdateAsync(int id, CatCreateDTO DTO);

    Task<bool> DeleteAsync(int id);
}
