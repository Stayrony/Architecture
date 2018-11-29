using System;
namespace Architecture.Model.Database
{
    public class TestSeedDatabase
    {
        private readonly ArchitectureDbContext _dbContext;

        public TestSeedDatabase(ArchitectureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
        }
    }
}
