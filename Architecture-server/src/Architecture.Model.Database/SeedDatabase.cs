using System;
namespace Architecture.Model.Database
{
    public class SeedDatabase
    {
        private readonly ArchitectureDbContext _dbContext;

        public SeedDatabase(ArchitectureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
        }
    }
}
