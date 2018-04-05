using Microsoft.EntityFrameworkCore;

namespace DojoMyMusic.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Musica> Musicas { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artista>().ToTable("Artistas");
            modelBuilder.Entity<Musica>().ToTable("Musicas");
            modelBuilder.Entity<Playlist>().ToTable("Playlists");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            modelBuilder.Entity<PlaylistMusica>()
                .HasKey(pm => new { pm.PlaylistId, pm.MusicaId });
        }
    }
}