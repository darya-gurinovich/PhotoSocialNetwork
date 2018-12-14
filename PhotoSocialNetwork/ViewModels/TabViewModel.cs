using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels
{
    public class TabViewModel
    {
        public Tab ActiveTab { get; set; } 
    }

    public enum Tab
    {
        Friends,
        UsersPermissions,
        UsersBlockings,
        PostsBlockings
    }
}
