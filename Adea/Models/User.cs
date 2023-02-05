namespace Adea.Models;

public class UserDAO
{
	public UserDAO()
	{
		Id = Guid.NewGuid().ToString();
		CreatedDate = DateTime.Now;
	}

	public string Id { get; set; } = "";
	public string Username { get; set; } = "";
	public string Password { get; set; } = "";
	public bool IsOfficer { get; set; }
	public DateTime CreatedDate { get; set; }

	public ICollection<LoanApplicationDAO>? LoanApplicationOfficers { get; set; } = null;
	public ICollection<LoanApplicationDAO>? LoanApplicationUsers { get; set; } = null;
}
