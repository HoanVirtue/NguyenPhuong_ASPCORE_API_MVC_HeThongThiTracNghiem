namespace MultipleChoiceTest.Domain.ModelViews
{
    public class SubjectItem
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? SubjectName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
