using MultipleChoiceTest.Web.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MultipleChoiceTest.Web.Api
{
	public static class ApiClient
	{
		private static readonly HttpClient _httpClient = new HttpClient();

		public static void Initialize()
		{
			if (_httpClient.BaseAddress != new Uri(ApiSettings.BaseAddress))
				_httpClient.BaseAddress = new Uri(ApiSettings.BaseAddress);
			_httpClient.DefaultRequestHeaders.Clear();
		}


		public static string GetCookie(HttpRequest request, string key)
		{
			if (request.Cookies.TryGetValue(key, out var value))
			{
				return value;
			}
			return null;
		}

		public static void SetCookie(HttpResponse Response, string key, string value)
		{
			Response.Cookies.Append(key, value);
		}
		public static async Task<ApiResponse<T>> GetAsync<T>(HttpRequest request, string requestUri)
		{
			var token = GetCookie(request, UserConstant.AccessToken);
			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			var response = await _httpClient.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString);
		}

		public static async Task<ApiResponse<T>> PostAsync<T>(HttpRequest request, string requestUri, string jsonContent)
		{
			var token = GetCookie(request, UserConstant.AccessToken);
			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(requestUri, content);
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString);
		}

		public static async Task<ApiResponse<T>> PutAsync<T>(HttpRequest request, string requestUri, string jsonContent)
		{
			var token = GetCookie(request, UserConstant.AccessToken);
			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(requestUri, content);
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString);
		}

		public static async Task<ApiResponse<T>> DeleteAsync<T>(HttpRequest request, string requestUri)
		{
			var token = GetCookie(request, UserConstant.AccessToken);
			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			var response = await _httpClient.DeleteAsync(requestUri);
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString);
		}
	}
}
