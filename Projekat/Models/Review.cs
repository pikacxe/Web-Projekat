using System.ComponentModel.DataAnnotations;

namespace Projekat.Models
{
    public class Review
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Product is invalid !")]
        public int Product { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Reviewer is invalid !")]
        public int Reviewer { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Title is invalid !")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Content is invalid !")]
        public string Content { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Image is invalid !")]
        public string Image { get; set; }
        public bool isDeleted { get; set; }
        public bool isApproved { get; set; }
        public bool isDenied { get; set; }
    }
}