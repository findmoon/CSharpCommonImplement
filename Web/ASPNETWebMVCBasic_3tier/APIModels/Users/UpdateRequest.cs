using System.ComponentModel.DataAnnotations;
using WebAPI_CURD.Entities;

namespace WebAPI_CURD.Models.Users
{
    public class UpdateRequest:CreateRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
