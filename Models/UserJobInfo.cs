namespace DotnetAPI.Models
{
    // it's good practice to make Models partial in case you need to add to them later from other files
    public partial class UserJobInfo
    {
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public UserJobInfo()
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

