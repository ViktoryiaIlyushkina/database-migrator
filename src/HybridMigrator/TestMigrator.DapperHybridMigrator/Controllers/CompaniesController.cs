using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TestMigrator.DapperMigrator.DTOs;

namespace TestMigrator.DapperMigrator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly string _connectionString;

    public CompaniesController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string is missing");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
            SELECT c.""Id"", c.""Name"", c.""Email"", e.""Id"", e.""FullName""
            FROM ""Companies"" c
            LEFT JOIN ""Employees"" e ON c.""Id"" = e.""CompanyId""";

        var companyDict = new Dictionary<int, CompanyDto>();

            var result = await connection.QueryAsync<CompanyDto, EmployeeDto, CompanyDto>(
            sql,
            (company, employee) =>
            {
                if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                {
                    currentCompany = company;
                    companyDict.Add(currentCompany.Id, currentCompany);
                }

                if (employee != null)
                {
                    currentCompany.Employees.Add(employee);
                }

                return currentCompany;
            },
            splitOn: "id" 
        );

        return Ok(companyDict.Values);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(string name, string email)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = "INSERT INTO \"Companies\" (\"Name\", \"Email\") VALUES (@Name, @Email) RETURNING \"Id\"";

        var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = name, Email = email });
        return Ok(new { Id = id, Name = name, Email = email });
    }
}

