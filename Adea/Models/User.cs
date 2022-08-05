

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

	public string Id { get; set; } = null!;
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public bool? IsOfficer { get; set; }
	public DateTime? CreatedDate { get; set; }

	public virtual ICollection<LoanApplication> LoanApplicationOfficers { get; set; }
	public virtual ICollection<LoanApplication> LoanApplicationUsers { get; set; }


}
