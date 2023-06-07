using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;

namespace Domain.Model
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }

        public int Reports { get; set; }
        public ForumComment() { }
        public ForumComment(int id, string comment, int forumId, int userId)
        {
            Id = id;
            Comment = comment;
            ForumId = forumId;
            UserId = userId;
            Reports = 0;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Comment, ForumId.ToString(), UserId.ToString(),Reports.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment = values[1];
            ForumId = Convert.ToInt32(values[2]);
            UserId = Convert.ToInt32(values[3]);
            Reports = Convert.ToInt32(values[4]);
        }
    }
}
