using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vets.Models;

namespace Vets.Data
{
    /// <summary>
    /// esta classe funciona como a base de dados do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        
        //defenir as 'tabelas'
        public DbSet<Animais> Animais { get; set; }
        public DbSet<Veterinarios> Veterinarios { get; set; }
        public DbSet<Donos> Donos { get; set; }
        public DbSet<Consultas> Consultas { get; set; }
    }
}