using Microsoft.EntityFrameworkCore;
using PhotoSocialNetwork.Models.Services;
using PhotoSocialNetwork.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.Models.Storage.EntityFramework
{
    public class EntityFrameworkStorage : IStorage
    {
        private readonly PhotoSocialNetworkContext context;

        public EntityFrameworkStorage(PhotoSocialNetworkContext context)
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

        public void AddUser(RegisterModel newUser)
        {
            var encryptedPassword = AuthenticationService.EncryptPassword(newUser.Password);

            context.Database.ExecuteSqlCommand("CreateProfile @p0, @p1, @p2, @p3, @p4, @p5", 
                parameters: new[] { newUser.Login, encryptedPassword, newUser.Email, newUser.Phone, newUser.Login, newUser.DateOfBirth.ToString() });
        }

    }
}
