using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ReportedComents:ISerializable
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }

        public ReportedComents(int id, int forumId, int commentId, int userId)
        {
            Id = id;
            ForumId = forumId;
            CommentId = commentId;
            UserId = userId;
        }

        public ReportedComents()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ForumId.ToString(), CommentId.ToString(), UserId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ForumId = Convert.ToInt32(values[1]);
            CommentId = Convert.ToInt32(values[2]);
            UserId = Convert.ToInt32(values[3]);
            
        }
    }
}
