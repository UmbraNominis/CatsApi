using CatsApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatsApi.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Cat> Cats { get; set; }
    public DbSet<CatBreed> CatBreeds { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}
