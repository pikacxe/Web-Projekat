using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public interface IUserDao
    {
        IEnumerable<User> GetAll();
        User FindById(int id);
        User FindByUsername(string username);
        User AddUser(User user);
        User UpdateUser(User updatedUser);
        User DeleteUser(int id);
    }
}
