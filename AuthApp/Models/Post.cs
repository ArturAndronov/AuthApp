using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        // Явное указание внешнего ключа
        [Required]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}
