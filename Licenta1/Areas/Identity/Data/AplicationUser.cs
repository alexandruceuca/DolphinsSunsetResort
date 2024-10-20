using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Licenta1.Models;
using Microsoft.AspNetCore.Identity;

namespace Licenta1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AplicationUser class
public class AplicationUser : IdentityUser
{
	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string FirstName { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string LastName { get; set; }

	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}

