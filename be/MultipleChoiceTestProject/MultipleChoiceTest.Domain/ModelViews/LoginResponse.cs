using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Domain.ModelViews
{
	public class LoginResponse
	{
		public LoginResponse()
		{
		}
		public LoginResponse(string accessToken, User user)
		{
			AccessToken = accessToken;
			User = user;
		}

		public string AccessToken { get; set; }
		public User User { get; set; }
	}
}
