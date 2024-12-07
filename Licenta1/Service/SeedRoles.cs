using DolphinsSunsetResort.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace DolphinsSunsetResort.Service
{
	public class SeedRoles
	{
		public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AplicationUser> userManager)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			var roleNames = new[] { "Admin", "Manager", "Reception", "RoomCleaner" };

			foreach (var roleName in roleNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			await CreateAdminUser(userManager);
		}

		private static async Task CreateAdminUser(UserManager<AplicationUser> userManager)
		{
			var adminEmail = "admin@admin.com";  // Admin email
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				adminUser = new AplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					FirstName = "admin",
					LastName = "admin"

				};

				var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

				if (result.Succeeded)
				{

					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
		}
	}
}
