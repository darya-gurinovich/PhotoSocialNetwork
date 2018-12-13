using PhotoSocialNetwork.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.Models.Storage
{
    public interface IStorage
    {
        bool IsUserAutenticationInfoCorrect(string login, string password);
        bool UserExists(string login, string email);
        void AddUser(RegisterModel newUser);
    }
}
