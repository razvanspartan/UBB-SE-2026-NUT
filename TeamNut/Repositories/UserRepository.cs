using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;

namespace TeamNut.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext _context = new AppDbContext();

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task Add(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync(); // EF Core handles the new ID for you!
    }

    public async Task Update(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetByUsernameAndPassword(string username, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // --- UserData Methods ---

    public async Task<UserData?> GetUserDataByUserId(int userId)
    {
        return await _context.UserDatas
            .FirstOrDefaultAsync(ud => ud.UserId == userId);
    }

    public async Task AddUserData(UserData data)
    {
        _context.UserDatas.Add(data);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserData(UserData data)
    {
        _context.UserDatas.Update(data);
        await _context.SaveChangesAsync();
    }
}