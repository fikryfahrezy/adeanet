namespace Adea.Services.Models;

public partial class UserDAO
{
	public UserDAO()
	{
		Id = Guid.NewGuid().ToString();
		CreatedDate = DateTime.Now;
		LoanApplicationOfficers = new HashSet<LoanApplicationDAO>();
		LoanApplicationUsers = new HashSet<LoanApplicationDAO>();
	}

	public string Id { get; set; } = "";
	public string Username { get; set; } = "";
	public string Password { get; set; } = "";
	public bool IsOfficer { get; set; }
	public DateTime CreatedDate { get; set; }

	public virtual ICollection<LoanApplicationDAO> LoanApplicationOfficers { get; set; }
	public virtual ICollection<LoanApplicationDAO> LoanApplicationUsers { get; set; }
}
