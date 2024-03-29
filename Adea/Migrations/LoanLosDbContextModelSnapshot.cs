﻿// <auto-generated />
using System;
using Adea.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Adea.Migrations
{
    [DbContext(typeof(LoanLosDbContext))]
    partial class LoanLosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Adea.Models.LoanApplicationDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("id");

                    b.Property<short>("ActiveFieldNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("active_field_number")
                        .HasDefaultValueSql("0");

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("birth_date")
                        .HasDefaultValueSql("''");

                    b.Property<long>("BusinessIncomePerMonthInIdr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("business_income_per_month_in_idr")
                        .HasDefaultValueSql("0");

                    b.Property<long>("BusinessOutcomePerMonthInIdr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("business_outcome_per_month_in_idr")
                        .HasDefaultValueSql("0");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<short>("EstimatedPriceOfHarvestPerKg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("estimated_price_of_harvest_per_kg")
                        .HasDefaultValueSql("0");

                    b.Property<short>("EstimatedYieldInKg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("estimated_yield_in_kg")
                        .HasDefaultValueSql("0");

                    b.Property<short>("ExpInYear")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("exp_in_year")
                        .HasDefaultValueSql("0");

                    b.Property<string>("FullAddress")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("full_address")
                        .HasDefaultValueSql("''");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("full_name")
                        .HasDefaultValueSql("''");

                    b.Property<short>("HarvestCycleInMonths")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("harvest_cycle_in_months")
                        .HasDefaultValueSql("0");

                    b.Property<string>("IdCardUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("id_card_url")
                        .HasDefaultValueSql("''");

                    b.Property<bool>("IsPrivateField")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasColumnName("is_private_field")
                        .HasDefaultValueSql("false");

                    b.Property<long>("LoanApplicationInIdr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("loan_application_in_idr")
                        .HasDefaultValueSql("0");

                    b.Property<short>("NeededFertilizerPerCycleInKg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("needed_fertilizier_per_cycle_in_kg")
                        .HasDefaultValueSql("0");

                    b.Property<string>("OfficerId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("officer_id");

                    b.Property<string>("OtherBusiness")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("other_business")
                        .HasDefaultValueSql("''");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("phone")
                        .HasDefaultValueSql("''");

                    b.Property<short>("SowSeedsPerCycle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("sow_seeds_per_cycle")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("''");

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.HasIndex("UserId");

                    b.ToTable("loan_applications", (string)null);
                });

            modelBuilder.Entity("Adea.Models.UserDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool>("IsOfficer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasColumnName("is_officer")
                        .HasDefaultValueSql("false");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Username" }, "users_username_key")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Adea.Models.LoanApplicationDAO", b =>
                {
                    b.HasOne("Adea.Models.UserDAO", "Officer")
                        .WithMany("LoanApplicationOfficers")
                        .HasForeignKey("OfficerId")
                        .HasConstraintName("loan_applications_officer_id_fkey");

                    b.HasOne("Adea.Models.UserDAO", "User")
                        .WithMany("LoanApplicationUsers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("loan_applications_user_id_fkey");

                    b.Navigation("Officer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Adea.Models.UserDAO", b =>
                {
                    b.Navigation("LoanApplicationOfficers");

                    b.Navigation("LoanApplicationUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
