using Microsoft.EntityFrameworkCore;
using Adea.Data;
using Adea.Models;

namespace Adea.Services.UserService;

public class UserRepository
{

	private readonly LoanLosDbContext _dbContext;

	public UserRepository(LoanLosDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task InsertUserAsync(User user)
	{
		_dbContext.Users.Add(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<User?> GetUserByUsernameAsync(string username)
	{
		return await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
	}
}