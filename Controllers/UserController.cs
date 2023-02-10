using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
// attribute ApiController
[ApiController]
// "[controller]" looks for the word before Controller in the clas name, User in this instance
[Route("[controller]")]
// class Microsoft.AspNetCore.Mvc.ControllerBase
// A base class for an MVC controller without view support\.
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        // Console.WriteLine(config.GetConnectionString("DefaultConnection"));
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]

    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }
    // [HttpGet]
    [HttpGet("GetUsers")]
    // public IEnumerable<User> GetUsers()

    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
        // return new string[] { "user1", "user2"};
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
    }

    [HttpGet("GetSingleUser/{userId}")]
    // public IEnumerable<User> GetUser()

    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT 
                [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();
        // concatenation (above) is preferable to interpolation due to dapper 4000 character restraint
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]

    // interface Microsoft.AspNetCore.Mvc.IActionResult
    // Defines a contract that represents the result of an action method\.
    // class Microsoft.AspNetCore.Mvc.FromBodyAttribute (+ 1 overload)
    // Specifies that a parameter or property should be bound using the request body\.
    // public IActionResult EditUser([FromBody]) // taken from the payload/ req.body
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET
                [FirstName] = '" + user.FirstName +
                "', [LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email +
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            // OkResult ControllerBase.Ok() (+ 1 overload)
            // Creates [and returns] `OkResult` object that produces an empty `StatusCodes.Status200OK` response\.
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost("AddUser")]

    public IActionResult AddUser(User user)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.Users
            (
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active])
        VALUES
            (
             '" + user.FirstName +
             "', '" + user.LastName +
             "', '" + user.Email +
             "', '" + user.Gender +
             "', '" + user.Active +
            "')";
        // "' WHERE UserId = " + user.UserId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Add User");
    }
}

// In HTTP, an endpoint is a specific URL that represents a resource, such as a server, database, or other service. 
// When a client sends an HTTP request to an endpoint, the server responds with an HTTP response, 
// which can contain data or other information.

// For example, if you have a web API that provides access to a database of books, you might have an endpoint like "/books" 
// that represents the collection of all books, and individual endpoints like "/books/123" to represent specific books. 
// When a client sends a GET request to the "/books" endpoint, the server might respond with a list of all books in the database, 
// while a GET request to "/books/123" would return information about the book with ID 123.

// Endpoints are often used in RESTful web services, where each endpoint represents a different resource and supports 
// different HTTP methods, such as GET, POST, PUT, and DELETE, for accessing and modifying the resource.


