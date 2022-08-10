using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class LibraryContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .HasMany(e => e.Films)
                .WithMany(e => e.Actors)
                .Map(m => m.ToTable("FilmActors").MapLeftKey("Actor_ActorId").MapRightKey("Film_Id"));

            modelBuilder.Entity<Film>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.Films)
                .Map(m => m.ToTable("GenreFilms").MapLeftKey("Film_Id").MapRightKey("Genre_GenreId"));

            modelBuilder.Entity<Film>()
                .HasMany(e => e.Producers)
                .WithMany(e => e.Films)
                .Map(m => m.ToTable("ProducerFilms").MapLeftKey("Film_Id").MapRightKey("Producer_ProducerId"));

            modelBuilder.Entity<Film>()
                .HasMany(e => e.Users)
                .WithMany(e => e.FavoriteFilms)
                .Map(m => m.ToTable("UserFilms").MapLeftKey("Film_Id").MapRightKey("User_UserId"));
        }
        public LibraryContext() : base("name=LabraryOfNewFilms")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
