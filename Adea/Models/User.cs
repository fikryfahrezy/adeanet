

namespace Adea.Models;
public partial class User
{
	public User()
	{
		Id = Guid.NewGuid().ToString();
		CreatedDate = DateTime.Now;
		LoanApplicationOfficers = new HashSet<LoanApplication>();
		LoanApplicationUsers = new HashSet<LoanApplication>();
	}

	public string Id { get; set; } = "";
	public string Username { get; set; } = "";
	public string Password { get; set; } = "";
	public bool IsOfficer { get; set; }
	public DateTime CreatedDate { get; set; }

	public virtual ICollection<LoanApplication> LoanApplicationOfficers { get; set; }
	public virtual ICollection<LoanApplication> LoanApplicationUsers { get; set; }
}
