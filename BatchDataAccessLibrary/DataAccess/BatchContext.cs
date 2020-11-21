using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Models.ShiftLog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BatchDataAccessLibrary.DataAccess
{
    public class BatchContext : DbContext
    {
        public BatchContext(DbContextOptions<BatchContext> options) : base(options)
        {

        }
        public DbSet<BatchReport> BatchReports { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<BatchIssue> BatchIssues { get; set; }
        public DbSet<BatchIssue> RemovedIssues { get; set; }
        public DbSet<BatchConversionFault> ConversionFaults { get; set; }
        public DbSet<MaterialDetails> MaterialDetails { get; set; }
        public DbSet<IssuesScannedFor> IssuesScannedFor { get; set; }
        public DbSet<RecipeLimits> RecipeLimits { get; set; }
        public DbSet<GapInTimeReasons> GapInTimeReasons { get; set; }
        public DbSet<Formulation> Formulations { get; set; }
        public DbSet<FormulationMaterials> FormulationMaterials { get; set; }
        public DbSet<PcsWeightParameters> PcsParameters { get; set; }
        public DbSet<PcsReworkParameters> PcsReworkParameters { get; set; }
        public DbSet<PcsTempTargets> PcsTempsTargets { get; set; }
        public DbSet<PcsScoring> PcsScoringsTargets { get; set; }
        public DbSet<PcsToleranceParameters> PcsToleranceParameters { get; set; }
        public DbSet<OperatorShiftLog> ShiftLog { get; set; }
        public DbSet<ShiftTeam> ShiftTeams { get; set; }
        public DbSet<GoodStockToWaste> GoodStockToWaste { get; set; }
        public DbSet<ToteChange> ToteChanges { get; set; }
        public DbSet<BatchesForShift> BatchesForShift { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
