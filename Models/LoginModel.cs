namespace ASPNetCoreJWTAuthentication.Models
{
	public class Login
	{
		/// <summary>
		/// Your Identity framework username
		/// </summary>
		public required string EmailAddress { get; set; }

		/// <summary>
		/// Your identity framework password
		/// </summary>
		public required string Password { get; set; }
	}
}
