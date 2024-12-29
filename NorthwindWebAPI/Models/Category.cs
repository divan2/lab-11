// Models/Category.cs

using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindApi.Models
{
    public class Category
    {
        public int categoryID { get; set; }
        public string? categoryName { get; set; }
        public string? description { get; set; }
        public byte[]? picture { get; set; }
    }
}



