// The Repository Pattern is a common construct to avoid duplication of data access logic throughout our application. 
// This includes direct access to a database, ORM, WCF dataservices, xml files and so on. 
// The sole purpose of the repository is to hide the nitty gritty details of accessing the data. 
// We can easily query the repository for data objects, without having to know how to provide things 
// like a connection string. The repository behaves like a freely available in-memory data collection 
// to which we can add, delete and update objects.

// The Repository pattern adds a separation layer between the data and domain layers of an application. 
// It also makes the data access parts of an application better testable.
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
    }
}
