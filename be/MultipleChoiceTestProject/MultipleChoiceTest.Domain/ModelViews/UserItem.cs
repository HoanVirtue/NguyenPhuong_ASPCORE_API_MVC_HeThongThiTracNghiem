using MultipleChoiceTest.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class UserItem
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Phone { get; set; }

        public string? AccountName { get; set; }

        public string? PasswordHash { get; set; }

        public bool? IsAdmin { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
