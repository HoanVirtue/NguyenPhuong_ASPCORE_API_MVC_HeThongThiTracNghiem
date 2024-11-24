namespace MultipleChoiceTest.Domain.Models;

public partial class Lesson : BaseEntity
{
	public string? LessonName { get; set; }

	public int? SubjectId { get; set; }


	public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

	public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

	public virtual Subject? Subject { get; set; }
}
// e thấy đối tượng subject trong lesson không đó tức là anh muốn lấy cả đối tượng subject đó để làm gì
// lấy các đối tg trong bảng khác à, đroi bình thường là phải thêm 1 hàm lấy subject theo subjectId nhưng mà cái này hỗ trợ luôn
//chỗ kia có 2 hàm lấy subject à
// không em, 1 cái là lấy lesson theo subjectId
// còn 1 cái là lấy lesson all kiểu dạng là có cả SubjectName