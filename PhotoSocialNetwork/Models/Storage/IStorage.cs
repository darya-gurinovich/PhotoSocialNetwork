using PhotoSocialNetwork.ViewModels;
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

        bool IsUserAdmin(string userName);
        bool IsUserAdmin(int userId);
        bool GiveAdminPermission(int userId, int permissionId);
        bool RemoveAdminPermission(int userId, int permissionId);

        ProfileModel GetProfileModel(string userName);
        ProfileModel GetProfileModelById(int profileId);

        List<ProfileModel> GetAllProfiles();
        List<ProfileModel> GetAllProfilesWithoutCurrentUser(string userName);
        List<ProfileModel> GetAllProfilesWithFilter(string filter);
        List<ProfileModel> GetAllProfilesWithoutCurrentUserWithFilter(string userName, string filter);

        bool CheckIfUsersAreFriends(string userName, int userId);

        (string photoPath, string name) GetCurrentUserInfo(string userName);

        bool CheckUserBlocking(string userName);
        bool CheckUserBlocking(int userId);
        bool BlockUser(int userId, int statusId, string blockedByUserName);
        bool UnblockUser(int userId);
        List<UserPermissions> GetUserPermissions();
        List<BlockingStatus> GetBlockingStatuses();
    }
}
