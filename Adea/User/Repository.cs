using Microsoft.EntityFrameworkCore;
using Adea.Data;
using Adea.Models;

namespace Adea.User;

public class UserRepository
{

    private readonly LoanLosDbContext _dbContext;

    public UserRepository(LoanLosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> InsertUserAsync(RegisterUser user)
    {
        var userDAO = new UserDAO
        {
            Username = user.Username,
            Password = user.Password,
            IsOfficer = user.IsOfficer
        };

        _dbContext.Users.Add(userDAO);
        await _dbContext.SaveChangesAsync();

        return userDAO.Id.ToString();
    }

    public async Task<UserDAO?> GetUserByUsernameAsync(string username)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserDAO?> GetUserByUserIdAsync(string userId)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }
}
