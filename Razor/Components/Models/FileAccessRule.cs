using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Components.Models
{
    public class FileAccessRule
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? UserId { get; set; } 
        public IdentityUser? User { get; set; }
        public string Permission { get; set; } = string.Empty;
    }

}
