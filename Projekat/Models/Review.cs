namespace Projekat.Models
{
    public class Review
    {
        public int ID { get; set; }
        public int Product { get; set; }
        public int Reviewer { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool isDeleted { get; set; }
    }
}