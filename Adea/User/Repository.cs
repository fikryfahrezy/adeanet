using Microsoft.EntityFrameworkCore;
using Adea.Data;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.User;

public class UserRepository
{

    private readonly LoanLosDbContext _dbContext;

    public UserRepository(LoanLosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertUserAsync(UserDAO user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CheckUsernameExistAndThrowAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            throw new UnprocessableEntityException($"Username {username} already exist");
        }
    }

    public async Task<UserDAO> GetUserByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new NotFoundException($"Username {username} not exist");
        }

        return user;
    }

    public async Task<UserDAO> GetUserByIdAsync(string userId)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException($"User with {userId} not exist");
        }

        return user;
    }
}