using Microsoft.AspNetCore.Http;
using MultipleChoiceTest.Domain.ApiModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MultipleChoiceTest.Domain.Helpper
{
    public static class Utilities
    {
        public static string StripHTML(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    return Regex.Replace(input, "<.*?>", String.Empty);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static int PAGE_SIZE = 20;
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");

            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }

                return true;

            }
            catch
            {

            }
            return false;

        }
        public static string GetRandomKey(int length = 5)
        {
            //chuỗi mẫu (pattern)
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }
        public static bool IsSubstring(string a, string b)
        {
            // Kiểm tra nếu a hoặc b chứa chuỗi còn lại
            return a.Trim().Contains(b) || b.Trim().Contains(a);
        }
        public static string RemoveDiacriticsAndToLower(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            // Chuyển đổi chuỗi thành chữ thường
            input = input.ToLowerInvariant();

            // Normalize the string to FormD, which separates the characters and diacritics
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                // Get the Unicode category of the character
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                // Append characters that are not diacritics
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Return the normalized string to FormC, which is the composed form
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static async Task<string> UploadFile(IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch
            {
                return null;
            }
        }

        public static ApiResponse<string> FormatOptions(string options)
        {
            ApiResponse<string> result = new ApiResponse<string>();
            if (string.IsNullOrWhiteSpace(options))
                return new ApiResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = "Đáp án không được bỏ trống."
                };
            var answerList = options
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(option => option.Trim())
                .Where(option => !string.IsNullOrEmpty(option))
                .ToList();

            if (answerList.Count < 4)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Data = string.Empty,
                    Message = "Cần ít nhất 4 lựa chọn."
                };
            }
            return new ApiResponse<string>
            {
                Success = true,
                Data = string.Join(";", answerList),
            };
        }

        public static string ConvertToTextArea(string options)
        {
            ApiResponse<string> result = new ApiResponse<string>();
            if (string.IsNullOrWhiteSpace(options))
                return null;
            var answerList = options
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(option => option.Trim())
                .Where(option => !string.IsNullOrEmpty(option))
                .ToList();

            return string.Join("\n", answerList);
        }
    }
}
