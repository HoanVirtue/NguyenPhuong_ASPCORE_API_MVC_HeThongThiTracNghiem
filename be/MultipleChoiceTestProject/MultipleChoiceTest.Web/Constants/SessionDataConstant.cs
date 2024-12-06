namespace MultipleChoiceTest.Web.Constants
{
    public class SessionDataConstant
    {
        public static string ListQuestion = "ListQuestion";
        public static string QuestionAnswer = "QuestionAnswer";

        public static string FormatKey(string key, string value)
        {
            return $"{key}_{value}";
        }
    }
}
