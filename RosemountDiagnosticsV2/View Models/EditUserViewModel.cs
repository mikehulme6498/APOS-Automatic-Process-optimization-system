using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RosemountDiagnosticsV2.View_Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string ShortName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }

        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
    }
}
