namespace CatsApi.Services.Cat;

using CatsApi.Data;
using CatsApi.DTOs;
using CatsApi.FilterModels;
using CatsApi.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CatService : ICatService
{
    private readonly ApplicationDbContext _context;

    public CatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CatDTO> CreateAsync(CatCreateDTO DTO)
    {
        var cat = DTO.MapToCat();

        await _context.Cats.AddAsync(cat);
        await _context.SaveChangesAsync();

        // Get the Cat with Its breed
        cat = await _context.Cats
            .Include(c => c.CatBreed)
            .FirstOrDefaultAsync(c => c.Id == cat.Id);

        return cat!.MapToDTO();
    }

    public async Task CreateFromListAsync(List<CatCreateDTO> DTOs)
    {
        var cats = DTOs.MapToCat();

        await _context.Cats.AddRangeAsync(cats);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CatDTO>> GetAllAsync(CatFilter filter)
    {
        var cats = _context.Cats
            .Include(c => c.CatBreed)
            .AsQueryable();

        if (filter.Id is not null)
        {
            cats = cats.Where(c => c.Id == filter.Id);
        }
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            cats = cats.Where(c => c.Name.Contains(filter.Name));
        }
        if (!string.IsNullOrWhiteSpace(filter.Likes))
        {
            cats = cats.Where(c => c.Likes.Contains(filter.Likes));
        }
        if (!string.IsNullOrWhiteSpace(filter.Dislikes))
        {
            cats = cats.Where(c => c.Dislikes.Contains(filter.Dislikes));
        }
        if (filter.Age is not null)
        {
            cats = cats.Where(c => c.Age == filter.Age);
        }
        if (filter.BreedId is not null)
        {
            cats = cats.Where(c => c.CatBreedId == filter.BreedId);
        }

        var filtered = await cats.ToListAsync();

        return filtered.MapToDTO();
    }

    public async Task<CatDTO?> GetAsync(int id)
    {
        var cat = await _context.Cats
            .Include(c => c.CatBreed)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cat is null) return null;

        return cat.MapToDTO();
    }

    public async Task<bool> UpdateAsync(int id, CatCreateDTO DTO)
    {
        var cat = await _context.Cats.FindAsync(id);

        if (cat is null) return false;

        DTO.MapToCat(cat);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cat = await _context.Cats.FindAsync(id);

        if (cat is null) return false;

        _context.Remove(cat);
        await _context.SaveChangesAsync();

        return true;
    }
}
