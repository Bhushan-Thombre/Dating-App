// DTO stands for Data Transfer Objects
// The ApiController is responsible for matching the query parameters with the arguments passed to the action/method
// It binds the arguments with the query parameters but does not bind them with the request body.
// DTOs are used for the same purpose. They bind the arguments with the request body.

using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}