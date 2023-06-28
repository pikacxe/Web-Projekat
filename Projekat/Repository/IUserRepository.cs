using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User FindById(int id);
        User FindByUsername(string username);
        User AddUser(User user);
        User UpdateUser(User updatedUser);
        User DeleteUser(int id);
    }
}
