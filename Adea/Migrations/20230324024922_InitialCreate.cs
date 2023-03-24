using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adea.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    isofficer = table.Column<bool>(name: "is_officer", type: "boolean", nullable: false, defaultValueSql: "false"),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loan_applications",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    userid = table.Column<string>(name: "user_id", type: "character varying(200)", maxLength: 200, nullable: false),
                    officerid = table.Column<string>(name: "officer_id", type: "character varying(200)", maxLength: 200, nullable: true),
                    fullname = table.Column<string>(name: "full_name", type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    birthdate = table.Column<string>(name: "birth_date", type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    fulladdress = table.Column<string>(name: "full_address", type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    phone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    idcardurl = table.Column<string>(name: "id_card_url", type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    otherbusiness = table.Column<string>(name: "other_business", type: "character varying(200)", maxLength: 200, nullable: false, defaultValueSql: "''"),
                    status = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, defaultValueSql: "''"),
                    isprivatefield = table.Column<bool>(name: "is_private_field", type: "boolean", nullable: false, defaultValueSql: "false"),
                    expinyear = table.Column<short>(name: "exp_in_year", type: "smallint", nullable: false, defaultValueSql: "0"),
                    activefieldnumber = table.Column<short>(name: "active_field_number", type: "smallint", nullable: false, defaultValueSql: "0"),
                    sowseedspercycle = table.Column<short>(name: "sow_seeds_per_cycle", type: "smallint", nullable: false, defaultValueSql: "0"),
                    neededfertilizierpercycleinkg = table.Column<short>(name: "needed_fertilizier_per_cycle_in_kg", type: "smallint", nullable: false, defaultValueSql: "0"),
                    estimatedyieldinkg = table.Column<short>(name: "estimated_yield_in_kg", type: "smallint", nullable: false, defaultValueSql: "0"),
                    estimatedpriceofharvestperkg = table.Column<short>(name: "estimated_price_of_harvest_per_kg", type: "smallint", nullable: false, defaultValueSql: "0"),
                    harvestcycleinmonths = table.Column<short>(name: "harvest_cycle_in_months", type: "smallint", nullable: false, defaultValueSql: "0"),
                    loanapplicationinidr = table.Column<long>(name: "loan_application_in_idr", type: "bigint", nullable: false, defaultValueSql: "0"),
                    businessincomepermonthinidr = table.Column<long>(name: "business_income_per_month_in_idr", type: "bigint", nullable: false, defaultValueSql: "0"),
                    businessoutcomepermonthinidr = table.Column<long>(name: "business_outcome_per_month_in_idr", type: "bigint", nullable: false, defaultValueSql: "0"),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updateddate = table.Column<DateTime>(name: "updated_date", type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_applications", x => x.id);
                    table.ForeignKey(
                        name: "loan_applications_officer_id_fkey",
                        column: x => x.officerid,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "loan_applications_user_id_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_officer_id",
                table: "loan_applications",
                column: "officer_id");

            migrationBuilder.CreateIndex(
                name: "IX_loan_applications_user_id",
                table: "loan_applications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loan_applications");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
