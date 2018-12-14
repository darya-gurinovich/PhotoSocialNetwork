using Microsoft.EntityFrameworkCore;
using PhotoSocialNetwork.Models.Services;
using PhotoSocialNetwork.ViewModels;
using PhotoSocialNetwork.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.Models.Storage.EntityFramework
{
    public class SQLEntityFrameworkStorage : IStorage
    {
        private readonly PhotoSocialNetworkContext context;

        public SQLEntityFrameworkStorage(PhotoSocialNetworkContext context)
        {
            this.context = context;
        }

        public IQueryable<BlockingStatus> BlockingStatus => context.BlockingStatus;
        public IQueryable<Comment> Comment => context.Comment;
        public IQueryable<Post> Post => context.Post;
        public IQueryable<PostBlockingLogs> PostBlockingLogs => context.PostBlockingLogs;
        public IQueryable<Profile> Profile => context.Profile;
        public IQueryable<RemovalLogs> RemovalLogs => context.RemovalLogs;
        public IQueryable<UserAccessRights> UserAccessRights => context.UserAccessRights;
        public IQueryable<UserBlockingLogs> UserBlockingLogs => context.UserBlockingLogs;
        public IQueryable<UserPermissions> UserPermissions => context.UserPermissions;
        public IQueryable<UserRegistrationLogs> UserRegistrationLogs => context.UserRegistrationLogs;
        public IQueryable<UserRelationships> UserRelationships => context.UserRelationships;
        public IQueryable<UserRelationshipStatus> UserRelationshipStatus => UserRelationshipStatus;
        public IQueryable<Users> Users => Users;

        public bool IsUserAutenticationInfoCorrect(string login, string password)
        {
            var encryptedPassword = AuthenticationService.EncryptPassword(password);
            var authenticationUser = context.Users.FromSql("SELECT * FROM Users WHERE Login = @p0 AND Password = @p1", parameters: new[] { login, encryptedPassword }).FirstOrDefault();
            return authenticationUser != null;
        }

        public bool UserExists(string login, string email)
        {
            var foundUser = context.Users.FromSql("SELECT * FROM Users WHERE Login = @p0 OR Email = @p0", parameters: new[] { login }).FirstOrDefault();
            return foundUser != null;
        }

        public Users GetUser(string userName)
        {
            return context.Users.FromSql("SELECT * FROM Users WHERE Login = @p0 OR Email = @p0", parameters: new[] { userName }).FirstOrDefault();
        }

        public Users GetUser(int userId)
        {
            return context.Users.FromSql("SELECT * FROM Users WHERE Id = @p0", userId).FirstOrDefault();
        }

        public void AddUser(RegisterModel newUser)
        {
            var encryptedPassword = AuthenticationService.EncryptPassword(newUser.Password);

            context.Database.ExecuteSqlCommand("CreateProfile @p0, @p1, @p2, @p3, @p4, @p5", 
                parameters: new[] { newUser.Login, encryptedPassword, newUser.Email, newUser.Phone, newUser.Login, newUser.DateOfBirth.ToString() });
        }
        
        public ProfileModel GetProfileModel(string userName)
        {
            var profile = context.Profile.FromSql("SELECT * FROM Users, Profile WHERE Users.Id = Profile.UserId AND(Login = @p0 OR Email = @p0)",
                parameters: new[] { userName }).FirstOrDefault();
            var user = GetUser(userName);

            if (profile == null) { return null; }

            return new ProfileModel(profile, user.Email, user.Phone);            
        }

        public ProfileModel GetProfileModelById(int profileId)
        {
            var profile = context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0",
                profileId).FirstOrDefault();

            if (profile == null) { return null; }

            var user = context.Users.FromSql("SELECT * FROM Users WHERE Users.Id = @p0",
                profile.UserId).FirstOrDefault();

            return new ProfileModel(profile, user.Email, user.Phone);
        }

        public (string photoPath, string name) GetCurrentUserInfo(string userName)
        {
           var profileModel = GetProfileModel(userName);
           if (profileModel == null) return (photoPath: "", name: "");

            return (photoPath: profileModel.PhotoPath, name: profileModel.Name);
        }


        public List<ProfileModel> GetAllProfiles()
        {
            var profiles = new List<ProfileModel>();
            foreach (var user in Users)
            {
                var profile = context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0",
                    user.Id).FirstOrDefault();

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        public List<ProfileModel> GetAllProfilesWithoutCurrentUser(string userName)
        {
            var users = context.Users.FromSql("SELECT * FROM Users WHERE NOT (Login = @p0 OR Email = @p0)",
                parameters: new[] { userName });

            var profiles = new List<ProfileModel>();
            foreach (var user in users)
            {
                var profile = context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0",
                    user.Id).FirstOrDefault();

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        public List<ProfileModel> GetAllProfilesWithFilter(string filter)
        {
            var profiles = new List<ProfileModel>();
            foreach (var user in Users)
            {
                var profile = context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0 AND Profile.Name LIKE @p1",
                    user.Id, filter + "%").FirstOrDefault();

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        public List<ProfileModel> GetAllProfilesWithoutCurrentUserWithFilter(string userName, string filter)
        {
            var users = context.Users.FromSql("SELECT * FROM Users WHERE NOT (Login = @p0 OR Email = @p0)",
                parameters: new[] { userName });

            var profiles = new List<ProfileModel>();
            foreach (var user in users)
            {
                var profile = context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0 AND Profile.Name LIKE @p1",
                    user.Id, filter + "%").FirstOrDefault();

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        public bool CheckIfUsersAreFriends(string userName, int userId)
        {
            var user = GetUser(userName);
            if (user == null) return false;

            var usersRelationship = context.UserRelationships.FromSql("SELECT UR.UserRelationshipId, UR.UserRelationshipStatusId," +
                "UR.MainUserId, UR.DependentUserId FROM UserRelationships UR, UserRelationshipStatus URS " +
                "WHERE ((UR.MainUserId = @p0 AND UR.DependentUserId = @p1) OR (UR.DependentUserId = @p0 AND UR.MainUserId = @p1)) " +
                "AND UR.UserRelationshipStatusId = URS.UserRelationshipStatusId AND URS.UserRelationshipStatusName = 'Friend'", userId, user.Id).FirstOrDefault();

            return usersRelationship == null ? false : true;

        }

        #region Admin Methods

        public bool IsUserAdmin(string userName)
        {
            var user = GetUser(userName);
            var permissions = context.UserAccessRights.FromSql("SELECT * FROM UserAccessRights WHERE UserAccessRights.UserId = @p0",
                user.Id).FirstOrDefault();

            return permissions != null ? true : false;
        }

        public bool IsUserAdmin(int userId)
        {
            var permissions = context.UserAccessRights.FromSql("SELECT * FROM UserAccessRights WHERE UserAccessRights.UserId = @p0", 
                userId).FirstOrDefault();

            return permissions != null ? true : false;
        }

        public bool GiveAdminPermission(int userId, int permissionId)
        {
            try
            {
                context.Database.ExecuteSqlCommand("AddUserAccessRightById @p0, @p1",
                    userId, permissionId );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveAdminPermission(int userId, int permissionId)
        {
            try
            {
                context.Database.ExecuteSqlCommand("RemoveAccessRight @p0, @p1",
                    userId, permissionId );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckUserBlocking(string userName)
        {
            var user = GetUser(userName);
            return user.IsBlocked;
        }

        public bool CheckUserBlocking(int userId)
        {
            var user = GetUser(userId);
            return user.IsBlocked;
        }

        public bool BlockUser(int userId, int statusId, string blockedByUserName)
        {
            try
            {
                var user = GetUser(blockedByUserName);

                if (user != null)
                    context.Database.ExecuteSqlCommand("BlockUser @p0, @p1, @p2",
                        userId, statusId, user.Id);
                else
                    context.Database.ExecuteSqlCommand("BlockUser @p0, @p1",
                        userId, statusId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UnblockUser(int userId)
        {
            try
            {
                context.Database.ExecuteSqlCommand("UnblockUser @p0", userId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
