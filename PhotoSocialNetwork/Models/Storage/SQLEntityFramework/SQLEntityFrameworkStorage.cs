using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotoSocialNetwork.Models.Services;
using PhotoSocialNetwork.ViewModels;
using PhotoSocialNetwork.ViewModels.Account;
using PhotoSocialNetwork.ViewModels.Logs;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Post GetPost(int postId)
        {
            return context.Post.FromSql("SELECT * FROM Post WHERE PostId = @p0", parameters: new[] { postId }).FirstOrDefault();
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

        public Profile GetProfile(string name)
        {
            return context.Profile.FromSql("SELECT * FROM Users, Profile WHERE Users.Id = Profile.UserId AND(Login = @p0 OR Email = @p0)",
                parameters: new[] { name }).FirstOrDefault();
        }

        public Profile GetProfile(int id)
        {
            return context.Profile.FromSql("SELECT * FROM Profile WHERE Profile.UserId = @p0", id).FirstOrDefault();
        }

        public ProfileModel UpdateProfilePhoto(string userName, IFormFile photo)
        {
            var profile = context.Profile.FromSql("SELECT * FROM Users, Profile WHERE Users.Id = Profile.UserId AND(Login = @p0 OR Email = @p0)",
                parameters: new[] { userName }).FirstOrDefault();
            if (profile == null) return null;

            var user = GetUser(userName);

            if (photo == null || photo.Length <= 0) return new ProfileModel(profile, user.Email, user.Phone);

            byte[] photoBytes = new byte[1];
            using (var stream = new MemoryStream())
            {
                photo.CopyTo(stream);
                photoBytes = stream.ToArray();
            }

            profile.Photo = photoBytes;

            context.Database.ExecuteSqlCommand("UpdateProfilePhoto @p0, @p1", user.Id, photoBytes);
            return new ProfileModel(profile, user.Email, user.Phone);
        }

        public int GetUserId(string userName)
        {
            var user = context.Users.FirstOrDefault(u => u.Login == userName || u.Email == userName);
            return user == null ? 0 : user.Id;
        }

        public void UpdateProfile(string userName, ProfileModel newProfile)
        {
            var user = GetUser(userName);
            
            context.Database.ExecuteSqlCommand("UpdateUserEmail @p0, @p1", user.Id, newProfile.Email);
            context.Database.ExecuteSqlCommand("UpdateUserPhone @p0, @p1", user.Id, newProfile.Phone);
            context.Database.ExecuteSqlCommand("UpdateProfileName @p0, @p1", user.Id, newProfile.Name);
            context.Database.ExecuteSqlCommand("UpdateProfileDateOfBirth @p0, @p1", user.Id, newProfile.DateOfBirth);     
        }

        private List<Users> GetUserFriends(string userName)
        {
            var user = GetUser(userName);

            if (user == null) return null;

            var friends = context.UserRelationships.Where(ur => ur.UserRelationshipStatusId == 1 && ur.DependentUserId == user.Id).Select(ur => ur.MainUser).ToList();
            friends.AddRange(context.UserRelationships.Where(ur => ur.UserRelationshipStatusId == 1 && ur.MainUserId == user.Id).Select(ur => ur.DependentUser).ToList());

            return friends;
        }


        public List<ProfileModel> GetUserFriendsProfiles(string userName)
        {
            var friends = GetUserFriends(userName);

            var friendsProfiles = new List<ProfileModel>();
            foreach (var friend in friends)
            {
                var friendProfile = GetProfile(friend.Id);

                if (friendProfile != null)
                    friendsProfiles.Add(new ProfileModel(friendProfile, friend.Email, friend.Phone));
            }

            return friendsProfiles;

        }
        
        public bool AddFriend(string userName, int friendUserId)
        {
            if (!context.Users.Select(u => u.Id).Contains(friendUserId)) return false;

            try
            {
                var user = GetUser(userName);
                if (user == null) return false;

                context.Database.ExecuteSqlCommand("AddUserRelationshipByIds @p0, @p1, @p2", user.Id, friendUserId, 1);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveFriend(string userName, int friendUserId)
        {
            if (!context.Users.Select(u => u.Id).Contains(friendUserId)) return false;

            try
            {
                var user = GetUser(userName);
                if (user == null) return false;

                var userRelationship = context.UserRelationships.FirstOrDefault(ur => ur.UserRelationshipStatusId == 1 &&
                (ur.MainUserId == user.Id && ur.DependentUserId == friendUserId) || (ur.DependentUserId == user.Id && ur.MainUserId == friendUserId));

                if (userRelationship == null) return false;

                context.Database.ExecuteSqlCommand("RemoveUserRelationship @p0, @p1", userRelationship.MainUserId, userRelationship.DependentUserId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ProfileModel> GetProfileModelsWithoutFriendsWithFilter(string userName, string filter)
        {
            var friends = GetUserFriends(userName);
            var users = context.Users.Where(u => !(u.Login == userName || u.Email == userName) && !(friends.Contains(u)));

            var profiles = new List<ProfileModel>();
            foreach (var user in users)
            {
                var profile = context.Profile.FirstOrDefault(pr => pr.UserId == user.Id && pr.Name.StartsWith(filter));

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        public List<ProfileModel> GetAllProfileModelsWithoutFriends(string userName)
        {
            var friends = GetUserFriends(userName);
            var users = context.Users.Where(u => !(u.Login == userName || u.Email == userName) && !(friends.Contains(u)));

            var profiles = new List<ProfileModel>();
            foreach (var user in users)
            {
                var profile = context.Profile.FirstOrDefault(pr => pr.UserId == user.Id);

                if (profile != null)
                    profiles.Add(new ProfileModel(profile, user.Email, user.Phone));
            }

            return profiles;
        }

        #region Post Methods
        public void CreatePost(PostViewModel postModel, string userName, IFormFile photo)
        {
            var image = GetImageFromFile(photo);
            var user = GetUser(userName);
            context.Database.ExecuteSqlCommand("CreatePost @p0, @p1, @p2", user.Id, image, postModel.Text);
        }

        public List<PostViewModel> GetUserPosts(int userId)
        {
            var user = GetUser(userId);
            var posts = context.Post.FromSql("SELECT * FROM Post P WHERE P.UserId = @p0 ORDER BY CreationDate DESC", user.Id);

            var postsModels = new List<PostViewModel>();
            foreach (var post in posts)
            {
                var profile = GetProfileModelById(post.UserId);
                postsModels.Add(new PostViewModel(post, profile));
            }

            return postsModels;
        }

        public List<PostViewModel> GetUserPosts(string userName)
        {
            var user = GetUser(userName);
            var posts = context.Post.FromSql("SELECT * FROM Post P WHERE P.UserId = @p0 ORDER BY CreationDate DESC", user.Id);

            var postsModels = new List<PostViewModel>();
            foreach(var post in posts)
            {
                var profile = GetProfileModelById(post.UserId);
                postsModels.Add(new PostViewModel(post, profile));
            }

            return postsModels;
        }


        public List<CommentViewModel> GetPostComments(int postId)
        {
            var comments = context.Comment.FromSql("SELECT * FROM Comment C WHERE C.PostId = @p0 ORDER BY CreationDate DESC", postId).ToList();
            var commentsModels = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
                var profile = GetProfileModelById(comment.UserId);
                commentsModels.Add(new CommentViewModel(comment, profile));
            }

            return commentsModels;
        }


        public bool AddComment(int postId, string userName, string text)
        {
            try
            {
                var user = GetUser(userName);

                context.Database.ExecuteSqlCommand("AddCommentToPost @p0, @p1, @p2", user.Id, text, postId);

                return true;
            }
            catch
            {
                return false;
            }
            
        }
        #endregion

        private byte[] GetImageFromFile(IFormFile image)
        {
            if (image == null || image.Length <= 0) return null;

            using (var stream = new MemoryStream())
            {
                image.CopyTo(stream);
                return stream.ToArray();
            }
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

        public List<UserPermissions> GetUserPermissions()
        {
            return context.UserPermissions.FromSql("SELECT * FROM UserPermissions").ToList();
        }

        public List<BlockingStatus> GetBlockingStatuses()
        {
            return context.BlockingStatus.FromSql("SELECT * FROM BlockingStatus").ToList();
        }
        
        public List<UserBlockingLogViewModel> GetUserBlockingLogs()
        {
            var logs = context.UserBlockingLogs.FromSql("SELECT * FROM UserBlockingLogs ORDER BY BlockingDate DESC");

            var logsModels = new List<UserBlockingLogViewModel>();
            foreach (var log in logs)
            {
                var user = GetUser(log.BlockedUserId);
                var blockedByUser = log.BlockedByUserId == null ? null : GetUser(log.BlockedByUserId.Value);
                var blockingStatus = context.BlockingStatus.FromSql("SELECT * FROM BlockingStatus BS WHERE BS.BlockingStatusId = @p0", log.BlockingStatusId).FirstOrDefault();
                
                if (blockingStatus == null) continue;

                logsModels.Add(new UserBlockingLogViewModel(user.Login, blockingStatus.BlockingName, blockedByUser == null ? "" : blockedByUser.Login, log.BlockingDate));
            }

            return logsModels;
        }

        public List<PostBlockingLogViewModel> GetPostBlockingLogs()
        {
            var logs = context.PostBlockingLogs.FromSql("SELECT * FROM PostBlockingLogs ORDER BY BlockingDate DESC");

            var logsModels = new List<PostBlockingLogViewModel>();
            foreach (var log in logs)
            {
                var post = GetPost(log.BlockedPostId);
                var blockedByUser = log.BlockedByUserId == null ? null : GetUser(log.BlockedByUserId.Value);
                var blockingStatus = context.BlockingStatus.FromSql("SELECT * FROM BlockingStatus BS WHERE BS.BlockingStatusId = @p0", log.BlockingStatusId).FirstOrDefault();

                if (blockingStatus == null) continue;

                logsModels.Add(new PostBlockingLogViewModel(post.PostId, blockingStatus.BlockingName, blockedByUser == null ? "" : blockedByUser.Login, log.BlockingDate));
            }

            return logsModels;
        }

        public List<RegistrationLogViewModel> GetRegistrationLogs()
        {
            var logs = context.UserRegistrationLogs.FromSql("SELECT * FROM UserRegistrationLogs ORDER BY RegistrationDate DESC");

            var logsModels = new List<RegistrationLogViewModel>();
            foreach (var log in logs)
            {
                var user = GetUser(log.UserId);

                logsModels.Add(new RegistrationLogViewModel(user.Login, log.RegistrationDate, log.Phone));
            }

            return logsModels;
        }
        #endregion
    }
}
