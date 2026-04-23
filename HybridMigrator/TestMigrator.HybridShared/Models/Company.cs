namespace TestMigrator.HybridShared.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public string TelphoneNumber { get; set; }
    public List<Employee> Employees { get; set; } = new();
}
