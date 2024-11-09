using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public void UpdateModifiedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
