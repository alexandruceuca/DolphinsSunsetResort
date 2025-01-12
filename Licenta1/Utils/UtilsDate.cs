namespace DolphinsSunsetResort.Utils
{
	public class UtilsDate
	{
		/// <summary>
		/// Parses the provided check-in and check-out dates and sets the time for each.
		/// Check-in time is set to 13:00 (1 PM), and check-out time is set to 09:00 (9 AM).
		/// </summary>
		/// <param name="checkInDate">The check-in date as a string.</param>
		/// <param name="checkOutDate">The check-out date as a string.</param>
		/// <returns>A tuple containing the parsed and adjusted start and end dates.</returns>
		/// <exception cref="FormatException">Thrown if the provided date strings are not in a valid format.</exception>
		public static (DateTime StartDate, DateTime EndDate) ParseAndSetBookingDates(string checkInDate, string checkOutDate)
		{
			//// Parse the dates
			DateTime parsedStartDate = string.IsNullOrEmpty(checkInDate) ? DateTime.MinValue : DateTime.Parse(checkInDate);
			DateTime parsedEndDate = string.IsNullOrEmpty(checkOutDate) ? DateTime.MaxValue : DateTime.Parse(checkOutDate);

			////Set time
			parsedStartDate = new DateTime(parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day, 13, 0, 0);
			parsedEndDate = new DateTime(parsedEndDate.Year, parsedEndDate.Month, parsedEndDate.Day, 9, 0, 0);

			return (parsedStartDate, parsedEndDate);
		}


	}
}
