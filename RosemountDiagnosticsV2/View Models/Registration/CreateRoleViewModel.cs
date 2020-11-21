using System.ComponentModel.DataAnnotations;

namespace RosemountDiagnosticsV2.View_Models.Registration
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
