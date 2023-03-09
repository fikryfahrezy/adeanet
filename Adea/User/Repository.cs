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

    private static Member UserDAOtoModel(UserDAO userDao) => new(
        id: userDao.Id,
        username: userDao.Username,
        password: userDao.Password,
        isOfficer: userDao.IsOfficer
    );

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

    public async Task<Member?> GetUserByUsernameAsync(string username)
    {
        var userDao = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (userDao is null)
        {
            return null;
        }

        return UserDAOtoModel(userDao);
    }

    public async Task<Member?> GetUserByUserIdAsync(string userId)
    {
        var userDao = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (userDao is null)
        {
            return null;
        }

        return UserDAOtoModel(userDao);
    }
}
