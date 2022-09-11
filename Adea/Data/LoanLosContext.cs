using Microsoft.EntityFrameworkCore;
using Adea.Models;

namespace Adea.Data;

public partial class LoanLosDbContext : DbContext
{

	public LoanLosDbContext(DbContextOptions<LoanLosDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<LoanApplication> LoanApplications { get; set; } = null!;
	public virtual DbSet<User> Users { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<LoanApplication>(entity =>
		{
			entity.ToTable("loan_applications");

			entity.Property(e => e.Id)
				.HasMaxLength(200)
				.HasColumnName("id");

			entity.Property(e => e.ActiveFieldNumber)
				.HasColumnName("active_field_number")
				.HasDefaultValueSql("0");

			entity.Property(e => e.BirthDate)
				.HasMaxLength(200)
				.HasColumnName("birth_date")
				.HasDefaultValueSql("''");

			entity.Property(e => e.BusinessIncomePerMonthInIdr)
				.HasColumnName("business_income_per_month_in_idr")
				.HasDefaultValueSql("0");

			entity.Property(e => e.BusinessOutcomePerMonthInIdr)
				.HasColumnName("business_outcome_per_month_in_idr")
				.HasDefaultValueSql("0");

			entity.Property(e => e.CreatedDate)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("created_date")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			entity.Property(e => e.EstimatedPriceOfHarvestPerKg)
				.HasColumnName("estimated_price_of_harvest_per_kg")
				.HasDefaultValueSql("0");

			entity.Property(e => e.EstimatedYieldInKg)
				.HasColumnName("estimated_yield_in_kg")
				.HasDefaultValueSql("0");

			entity.Property(e => e.ExpInYear)
				.HasColumnName("exp_in_year")
				.HasDefaultValueSql("0");

			entity.Property(e => e.FullAddress)
				.HasMaxLength(200)
				.HasColumnName("full_address")
				.HasDefaultValueSql("''");

			entity.Property(e => e.FullName)
				.HasMaxLength(200)
				.HasColumnName("full_name")
				.HasDefaultValueSql("''");

			entity.Property(e => e.HarvestCycleInMonths)
				.HasColumnName("harvest_cycle_in_months")
				.HasDefaultValueSql("0");

			entity.Property(e => e.IdCardUrl)
				.HasMaxLength(200)
				.HasColumnName("id_card_url")
				.HasDefaultValueSql("''");

			entity.Property(e => e.IsPrivateField)
				.HasColumnName("is_private_field")
				.HasDefaultValueSql("false");

			entity.Property(e => e.LoanApplicationInIdr)
				.HasColumnName("loan_application_in_idr")
				.HasDefaultValueSql("0");

			entity.Property(e => e.NeededFertilizierPerCycleInKg)
				.HasColumnName("needed_fertilizier_per_cycle_in_kg")
				.HasDefaultValueSql("0");

			entity.Property(e => e.OfficerId)
				.HasMaxLength(200)
				.HasColumnName("officer_id");

			entity.Property(e => e.OtherBusiness)
				.HasMaxLength(200)
				.HasColumnName("other_business")
				.HasDefaultValueSql("''");

			entity.Property(e => e.Phone)
				.HasMaxLength(200)
				.HasColumnName("phone")
				.HasDefaultValueSql("''");

			entity.Property(e => e.SowSeedsPerCycle)
				.HasColumnName("sow_seeds_per_cycle")
				.HasDefaultValueSql("0");

			entity.Property(e => e.Status)
				.HasMaxLength(25)
				.HasColumnName("status")
				.HasDefaultValueSql("''");

			entity.Property(e => e.UpdatedDate)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updated_date")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			entity.Property(e => e.UserId)
				.HasMaxLength(200)
				.HasColumnName("user_id");

			entity.HasOne(d => d.Officer)
				.WithMany(p => p.LoanApplicationOfficers)
				.HasForeignKey(d => d.OfficerId)
				.HasConstraintName("loan_applications_officer_id_fkey");

			entity.HasOne(d => d.User)
				.WithMany(p => p.LoanApplicationUsers)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("loan_applications_user_id_fkey");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("users");

			entity.HasIndex(e => e.Username, "users_username_key")
				.IsUnique();

			entity.Property(e => e.Id)
				.HasMaxLength(200)
				.HasColumnName("id");

			entity.Property(e => e.CreatedDate)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("created_date")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			entity.Property(e => e.IsOfficer)
				.HasColumnName("is_officer")
				.HasDefaultValueSql("false");

			entity.Property(e => e.Password)
				.HasMaxLength(200)
				.HasColumnName("password");

			entity.Property(e => e.Username)
				.HasMaxLength(200)
				.HasColumnName("username");
		});
	}
}