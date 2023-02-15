// 4. Testing the Program
// Before running a program, proofread aka "deskcheck" it...
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
// attribute ApiController
[ApiController]
// [controller] is a placeholder that looks for "UserEF" prefix in class DotnetAPI.Controllers.UserEFController
// will prefix [HttpGet/Put/Post/Delete(methods)] eg: url.com/UserEF/GetUsers
// this can be custom [Route("custom")] or empty [Route("")]. eg: url.com/custom/GetUsers, url.com/GetUsers
// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-7.0
[Route("[controller]")]
// class Microsoft.AspNetCore.Mvc.ControllerBase
// A base class for an MVC controller without view support\.
public class UserEFController : ControllerBase
{
    // class DotnetAPI.Data.DataContextEF
    DataContextEF _entityFramework;

    IUserRepository _userRepository;
    // UserRepository _userRepository;

    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    // public UserEFController(IConfiguration config, UserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);

        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            // Mapper for user doesn't work
            cfg.CreateMap<UserToAddDto, User>();
            // IMappingExpression<UserJobInfo, UserJobInfo> IProfileExpression.CreateMap<UserJobInfo, UserJobInfo>() (+ 1 overload)
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
            cfg.CreateMap<UserSalary, UserSalary>();
        }));
    }

    [HttpGet("GetUsers")]
    // return a list of users
    public IEnumerable<User> GetUsers()
    {
        //.Users: Microsoft.EntityFrameworkCore.DbSet<User> DataContextEF.Users { get; set; }
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }


    [HttpGet("GetUserJobInfo")]

    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        IEnumerable<UserJobInfo> users = _entityFramework.UserJobInfo.ToList<UserJobInfo>();
        return users;
    }


    [HttpGet("GetSingleUser/{userId}")]

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


    [HttpGet("GetSingleUserJobInfo/{userId}")]

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


    [HttpGet("GetSingleUserSalary/{userId}")]

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


    [HttpPut("EditUser")]
    // interface Microsoft.AspNetCore.Mvc.IActionResult
    // Defines a contract that represents the result of an action method\.
    // class Microsoft.AspNetCore.Mvc.FromBodyAttribute (+ 1 overload)
    // public IActionResult EditUser([FromBody]) // FromBody: taken from the req.body payload.
    // IActionResult UserController.EditUser(DotnetAPI.User user) // User from DotnetAPI/Models/Users.cs
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
          .Where(u => u.UserId == user.UserId)
        .FirstOrDefault();
        // .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            // if (_userRepository.SaveChanges())
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }

        throw new Exception("Failed to Get User");
    }


    [HttpPut("EditUserJobInfo")]

    public IActionResult EditUserJobInfo(UserJobInfo userForUpdate)
    {
        UserJobInfo? userDb = _entityFramework.UserJobInfo
          .Where(u => u.UserId == userForUpdate.UserId)
          // .FirstOrDefault<UserJobInfo>();
          .FirstOrDefault();

        if (userDb != null)
        {
            // userDb.JobTitle = userForUpdate.JobTitle;
            // userDb.Department = userForUpdate.Department;
            // UserJobInfo IMapperBase.Map<UserJobInfo, UserJobInfo>(UserJobInfo source, UserJobInfo destination) (+ 9 overloads)
            _mapper.Map(userForUpdate, userDb);
            if (_entityFramework.SaveChanges() > 0)
            // if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }

        throw new Exception("Failed to Get User");
    }


    [HttpPost("AddUser")]

    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        // _entityFramework.Add(userDb);
        // _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to Add User");
    }


    // class Microsoft.AspNetCore.Mvc.HttpDeleteAttribute (+ 2 overloads)
    // Identifies an action that supports the HTTP DELETE method\.
    [HttpDelete("DeleteUser/{userId}")]

    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
          .Where(u => u.UserId == userId)
          .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_userRepository.SaveChanges())
            {
                Console.WriteLine($"User {userDb.FirstName} {userDb.LastName} removed");
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }

        throw new Exception("Failed to Get User");
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
