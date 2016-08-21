using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleForum.Data.Models;
using SimpleForum.Web.Infrastructure.Mapping;

namespace SimpleForum.Web.ViewModels
{
    public class IndexForumPostViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }
    }
}