using FluentMigrator;

namespace TestMigrator.DbMigrator.Migrations;

[Migration(2023102701)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("companies")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(255).NotNullable();

        Create.Table("employees")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("fullname").AsString(255).NotNullable()
            .WithColumn("companyid").AsInt32().ForeignKey("companies", "id");
    }

    public override void Down()
    {
        Delete.Table("employees");
        Delete.Table("companies");
    }
}
