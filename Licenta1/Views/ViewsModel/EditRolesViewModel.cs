using Microsoft.AspNetCore.Identity;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class EditRolesViewModel
	{
		public string UserId { get; set; }
		public string FristName { get; set; }
        public string LastName { get; set; }
        public IList<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
