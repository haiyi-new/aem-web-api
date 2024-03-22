using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyWebApi.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<PlatformWellActualResponse> PlatformWellActualResponse { get; set; }
        public DbSet<PlatformWellActualWell> PlatformWellActualWell { get; set; }
        public DbSet<PlatformWellDummyResponse> PlatformWellDummyResponse { get; set; }
        public DbSet<PlatformWellActualWell> PlatformWellDummyWell { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseMySQL(configuration.GetConnectionString("MyDbConnection"));
            }
        }
    }

    // Model for the response of http://localhost/api/DataSync/GetPlatformWellActual
    public class PlatformWellActualResponse
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PlatformWellActualWell> Well { get; set; } = new List<PlatformWellActualWell>();
    }

    // Model for the response of http://localhost/api/DataSync/GetPlatformWellDummy
    public class PlatformWellDummyResponse
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<PlatformWellDummyWell> Well { get; set; } = new List<PlatformWellDummyWell>();
    }

    // Database table model for PlatformWellActualWell
    public class PlatformWellActualWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    // Database table model for PlatformWellDummyWell
    public class PlatformWellDummyWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
