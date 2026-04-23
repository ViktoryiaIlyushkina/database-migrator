namespace TestMigrator.DapperMigrator.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public List<EmployeeDto> Employees { get; set; } = new();
}
