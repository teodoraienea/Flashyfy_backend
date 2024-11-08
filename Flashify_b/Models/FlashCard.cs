using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flashify_b.Models
{
    [Table("flashcard")] 
    public class FlashCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
