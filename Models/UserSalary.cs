
namespace DotnetAPI.Models
{
    // it's good practice to make Models partial in case you need to add to them later from other files
    public partial class UserSalary
    {
        public int UserId { get; set; }

        public decimal Salary { get; set; }

        public decimal AvgSalary { get; set; }

    }

}
