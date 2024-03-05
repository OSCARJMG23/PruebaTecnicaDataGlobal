using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class PeliculaConfiguration : IEntityTypeConfiguration<Pelicula>
    {
        public void Configure(EntityTypeBuilder<Pelicula> builder)
        {
            // Configuración de la entidad

            builder.ToTable("pelicula");

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Id).IsUnique();

            builder.Property(e=>e.Id)
                .HasColumnType("int");

            builder.Property(e => e.Titulo)
                .HasMaxLength(50);
            
            builder.Property(e=>e.Director)
                .HasMaxLength(50);

            builder.Property(e => e.Anio)
                .HasColumnType("int");

            builder.Property(e=> e.Genero)
                .HasMaxLength(25);    
        }
    }
}