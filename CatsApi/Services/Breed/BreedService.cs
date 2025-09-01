namespace CatsApi.Services.Breed;

using CatsApi.Data;
using CatsApi.DTOs;
using CatsApi.FilterModels;
using CatsApi.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BreedService : IBreedService
{
    private readonly ApplicationDbContext _context;

    public BreedService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BreedDTO> CreateAsync(BreedCreateDTO DTO)
    {
        var breed = DTO.MapToBreed();

        await _context.CatBreeds.AddAsync(breed);
        await _context.SaveChangesAsync();

        return breed.MapToDTO();
    }

    public async Task CreateFromListAsync(List<BreedCreateDTO> DTOs)
    {
        var breeds = DTOs.MapToBreed();

        await _context.CatBreeds.AddRangeAsync(breeds);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BreedDTO>> GetAllAsync(BreedFilter filter)
    {
        var breeds = _context.CatBreeds
            .Include(b => b.Cats)
            .AsQueryable();

        if (filter.Id is not null)
        {
            breeds = breeds.Where(b => b.Id == filter.Id);
        }
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            breeds = breeds.Where(b => b.Name == filter.Name);
        }

        var filtered = await breeds.ToListAsync();

        return filtered.MapToDTO();
    }

    public async Task<BreedDTO?> GetAsync(int id)
    {
        var breed = await _context.CatBreeds
            .Include(b => b.Cats)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (breed is null) return null;

        return breed.MapToDTO();
    }

    public async Task<bool> UpdateAsync(int id, BreedCreateDTO DTO)
    {
        var breed = await _context.CatBreeds.FindAsync(id);

        if (breed is null) return false;

        DTO.MapToBreed(breed);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var breed = await _context.CatBreeds.FindAsync(id);

        if (breed is null) return false;

        _context.Remove(breed);
        await _context.SaveChangesAsync();

        return true;
    }
}
