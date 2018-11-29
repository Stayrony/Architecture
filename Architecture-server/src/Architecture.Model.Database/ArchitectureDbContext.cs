using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Architecture.Model.Database
{
    public class ArchitectureDbContext : DbContext
    {
        public ArchitectureDbContext(DbContextOptions<ArchitectureDbContext> options) : base(options)
        {
        }

#if DEBUG
        private static readonly LoggerFactory DebugLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var errorIds = new[]
            {
                RelationalEventId.QueryClientEvaluationWarning,
            };

            optionsBuilder.UseLoggerFactory(DebugLoggerFactory)
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(warnings => warnings.Throw(errorIds));

            base.OnConfiguring(optionsBuilder);
        }
#endif

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
