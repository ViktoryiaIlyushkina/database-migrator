using FluentMigrator;

namespace TestMigrator.DbMigrator.Migrations;

[Migration(2023102702)]
public class AddEmailToCompanies : Migration
{
    public override void Up()
    {
        Alter.Table("companies")
            .AddColumn("email").AsString(255).Nullable();
    }

    public override void Down()
    {
        Delete.Column("email").FromTable("companies");
    }
}