using System.ComponentModel.DataAnnotations;

namespace LeThanhNhan_2280602169.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
