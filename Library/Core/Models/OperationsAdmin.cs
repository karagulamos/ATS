using System.ComponentModel.DataAnnotations;

namespace Library.Core.Models
{
    public class OperationsAdmin
    {
        [Key]
        public string OperationsAdminId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
