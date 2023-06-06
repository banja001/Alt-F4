using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class ForumCommentUserDTO
    {
        public string Username { get; set; }
        public string UserRole { get; set; }
        public string Comment { get; set; }
        public int ForumId { get; set; }

        public ForumCommentUserDTO() { }
        public ForumCommentUserDTO(string username, string userRole, string comment, int forumId)
        {
            Username = username;
            UserRole = userRole;
            Comment = comment;
            ForumId = forumId;
        }

        public override string ToString()
        {
            return Username + "(" + UserRole + ")" + " - " + Comment;
        }
    }
}
