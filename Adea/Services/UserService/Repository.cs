using Adea.Models;
using Microsoft.EntityFrameworkCore;

namespace Adea.Services.UserService;

public class UserRepository
{

	private readonly LoanLosDbContext _dbContext;

	public UserRepository(LoanLosDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<User>> GetUsersAsync()
	{
		return await _dbContext.Users.ToListAsync();
	}

	public async Task InsertUserAsync(User user)
	{
		_dbContext.Users.Add(user);
		await _dbContext.SaveChangesAsync();
	}
}