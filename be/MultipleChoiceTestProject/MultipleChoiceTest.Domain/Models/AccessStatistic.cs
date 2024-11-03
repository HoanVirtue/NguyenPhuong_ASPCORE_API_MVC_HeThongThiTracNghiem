namespace MultipleChoiceTest.Domain.Models;

public partial class AccessStatistic : BaseEntity
{

	public DateOnly? StatisticsDate { get; set; }

	public int? DailyAccessCount { get; set; }

	public int? MonthlyAccessCount { get; set; }

	public int? YearlyAccessCount { get; set; }

	public int? TotalAccessCount { get; set; }

}
