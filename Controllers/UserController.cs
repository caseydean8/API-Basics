using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
// attribute ApiController
[ApiController]
// "[controller]" looks for the word before Controller in the class name, User in this instance
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

    [HttpGet("GetUsers")]

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
    }

    [HttpGet("GetUserJobInfo")]

    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        string sql = @"
            SELECT [UserId],
                [JobTitle],
                [Department]
            FROM TutorialAppSchema.UserJobInfo";
        IEnumerable<UserJobInfo> userJobInfo = _dapper.LoadData<UserJobInfo>(sql);
        return userJobInfo;
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

    [HttpGet("GetSingleUserJobInfo/{userId}")]

    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sql = @"
            SELECT 
                [UserId],
                [JobTitle],
                [Department]
            FROM TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId.ToString();

        UserJobInfo user = _dapper.LoadDataSingle<UserJobInfo>(sql);
        return user;
    }


    [HttpPut("EditUser")]

    // interface Microsoft.AspNetCore.Mvc.IActionResult
    // Defines a contract that represents the result of an action method\.
    // class Microsoft.AspNetCore.Mvc.FromBodyAttribute (+ 1 overload)
    // public IActionResult EditUser([FromBody]) // FromBody: taken from the req.body payload.
    // IActionResult UserController.EditUser(DotnetAPI.User user) // User from DotnetAPI/Models/Users.cs
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


    [HttpPut("EditUserJobInfo")]

    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserJobInfo
            SET
                [JobTitle] = '" + userJobInfo.JobTitle +
                "', [Department] = '" + userJobInfo.Department +
            "' WHERE UserId = " + userJobInfo.UserId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }


    [HttpPost("AddUser")]

    public IActionResult AddUser(UserToAddDto userToAdd)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.Users
            (
            [FirstName],
            [LastName],
            [Email],
            [Gender]
            [Active])
        VALUES
            (
             '" + userToAdd.FirstName +
             "', '" + userToAdd.LastName +
             "', '" + userToAdd.Email +
             "', '" + userToAdd.Gender +
             "', '" + userToAdd.Active +
            "')";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Add User");
    }


    [HttpPost("AddUserJobInfo")]

    public IActionResult AddUserJobInfo(UserJobInfoDto userJobInfo)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.UserJobInfo
            (
            [JobTitle],
            [Department])
        VALUES
            (
             '" + userJobInfo.JobTitle +
             "', '" + userJobInfo.Department +
            "')";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Add User Job Info");
    }
    // class Microsoft.AspNetCore.Mvc.HttpDeleteAttribute (+ 2 overloads)
    // Identifies an action that supports the HTTP DELETE method\.
    [HttpDelete("DeleteUser/{userId}")]

    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
           DELETE FROM TutorialAppSchema.Users 
              WHERE UserId = " + userId.ToString();

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete User");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]

    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
           DELETE FROM TutorialAppSchema.UserJobInfo 
              WHERE UserId = " + userId.ToString();

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete User");
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


