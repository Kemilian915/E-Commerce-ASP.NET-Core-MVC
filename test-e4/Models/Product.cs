using System.ComponentModel.DataAnnotations;

namespace test_e4.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be a positive number")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Camera is required")]
        [StringLength(10, ErrorMessage = "Camera specification cannot be longer than 10 characters")]
        public string Camera {  get; set; }
        [Required(ErrorMessage = "Storage is required")]
        [StringLength(10, ErrorMessage = "Storage specification cannot be longer than 10 characters")]
        public string Storage { get; set; }
        [Required(ErrorMessage = "Battery is required")]
        [StringLength(10, ErrorMessage = "Battery specification cannot be longer than 10 characters")]
        public string Battery { get; set; }
        [Required(ErrorMessage = "Color is required")]
        [StringLength(20, ErrorMessage = "Color cannot be longer than 20 characters")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string Description { get; set; }
        [Display(Name = "Image")]
        [Required(ErrorMessage = "Image URL is required")]
        public string ImageURL { get; set; }

    }
}
