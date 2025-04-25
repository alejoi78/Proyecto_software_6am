using Microsoft.EntityFrameworkCore;
using Proyecto_software_6am.Entidades;

namespace Proyecto_software_6am.DAOs;

public class AppDbContext : DbContext {
    public DbSet<Pelicula> Peliculas

{
    get;
    set;
}

public DbSet<Serie > Series {
    get;
    set;
}

public DbSet<Usuario > Usuarios {
    get;
    set;
}

public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
}

protected override void OnModelCreating(ModelBuilder modelBuilder) {
    // Configuración para herencia (Table-Per-Type) modelBuilder.Entity<Contenido>().ToTable("Contenidos");
    modelBuilder .Entity<Pelicula>().ToTable("Peliculas");
    modelBuilder .Entity<Serie>().ToTable("Series");
}
}
