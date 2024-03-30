using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo_Application.Model
{
    public class TodoItems        // declare the TodoItems table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; } = false;

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
