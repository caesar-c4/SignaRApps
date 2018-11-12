using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignaRApps.Models
{
    public class DbModel : DbContext
    {
        public DbModel()
            : base("DbModel")
        {

        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<MessageInfo> MessageInfo { get; set; }
    }

    public class UserInfo
    {
        public int UserInfoID { get; set; }
        public string Name { get; set; }
        public string ConnectionTime { get; set; }

        public string ConnectionID { get; set; }

        public virtual List<MessageInfo> MessageInfo { get; set; }
    }

    public class MessageInfo
    {
        public int MessageInfoID { get; set; }

        public string MessageText { get; set; }
        public string Time { get; set; }
        [ForeignKey("UserInfo")]
        public int UserInfoID { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}