using Dapper;
using Npgsql;
using TestMigrator.HybridShared.Models;

namespace TestMigrator.DapperMigrator.Repositories;

public class CompanyRepository
{
    private readonly string _connectionString;
    public CompanyRepository(IConfiguration config) =>
        _connectionString = config.GetConnectionString("DefaultConnection");

    public async Task<IEnumerable<Company>> GetCompaniesWithEmployees()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
            SELECT c.*, e.* FROM Companies c 
            LEFT JOIN Employees e ON c.Id = e.CompanyId";

        var companyDict = new Dictionary<int, Company>();

        var result = await connection.QueryAsync<Company, Employee, Company>(
            sql, (company, employee) =>
            {
                if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                {
                    currentCompany = company;
                    companyDict.Add(currentCompany.Id, currentCompany);
                }
                if (employee != null) currentCompany.Employees.Add(employee);
                return currentCompany;
            });

        return result.Distinct();
    }
}
