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
        public bool Visited { get; set; }

        public ForumCommentUserDTO() { }
        public ForumCommentUserDTO(string username, string userRole, string comment, int forumId, bool visited)
        {
            Username = username;
            UserRole = userRole;
            Comment = comment;
            ForumId = forumId;
            Visited = visited;
        }

        public override string ToString()
        {
            if(Visited)
                return Username + "[" + UserRole + "]_visited" + " - " + Comment;
            else
                return Username + "[" + UserRole + "]" + " - " + Comment;
        }
    }
}
