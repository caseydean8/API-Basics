// The Repository Pattern is a common construct to avoid duplication of data access logic throughout our application. 
// This includes direct access to a database, ORM, WCF dataservices, xml files and so on. 
// The sole purpose of the repository is to hide the nitty gritty details of accessing the data. 
// We can easily query the repository for data objects, without having to know how to provide things 
// like a connection string. The repository behaves like a freely available in-memory data collection 
// to which we can add, delete and update objects.

// The Repository pattern adds a separation layer between the data and domain layers of an application. 
// It also makes the data access parts of an application better testable.
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    // public class UserRepository // testing w/out IUser... 
    {

        DataContextEF _entityFramework;

        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        // This can return a boolean also
        // public bool AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                // return true;
            }
            // return false;
        }

        public void RemoveEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            //.Users: Microsoft.EntityFrameworkCore.DbSet<User> DataContextEF.Users { get; set; }
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            IEnumerable<UserJobInfo> users = _entityFramework.UserJobInfo.ToList<UserJobInfo>();
            return users;
        }

        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users
              .Where(u => u.UserId == userId)
             .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get User");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? user = _entityFramework.UserJobInfo
              .Where(u => u.UserId == userId)
             .FirstOrDefault<UserJobInfo>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get User");
        }

        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? user = _entityFramework.UserSalary
              .Where(u => u.UserId == userId)
             .FirstOrDefault<UserSalary>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get User");
        }
    }
}
