
namespace DotnetAPI
{
    // it's good practice to make Models partial in case you need to add to them later from other files
    public partial class UserJobInfoDto
    {
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public UserJobInfoDto()
        {
            if (JobTitle  == null)
            {
                JobTitle = "";
            }
            if (Department == null)
            {
                Department = "";
            }
        }
    }

}

