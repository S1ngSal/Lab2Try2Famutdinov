using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab2Try2Famutdinov.Models;

namespace Lab2Try2Famutdinov.Data
{
    public class Lab2Try2FamutdinovContext : DbContext
    {
        public Lab2Try2FamutdinovContext (DbContextOptions<Lab2Try2FamutdinovContext> options)
            : base(options)
        {
        }

        public DbSet<Lab2Try2Famutdinov.Models.Dish> Dish { get; set; } = default!;
        public DbSet<Lab2Try2Famutdinov.Models.Order> Order { get; set; } = default!;
        public DbSet<Lab2Try2Famutdinov.Models.User> User { get; set; } = default!;
    }
}
