using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Domain.Model
{
    public class ComplexRequest : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public User User { get; set; }
        public SimpleRequestStatus Status { get; set; }
        public List<SimpleRequest> SimpleRequests { get; set; } 

        public ComplexRequest()
        {
            this.User = new User();
            SimpleRequests = new List<SimpleRequest>(); 
        }
        public ComplexRequest(int userId, SimpleRequestStatus status, string name)
        {
            SimpleRequests = new List<SimpleRequest>();
            User = new User();

            Id = -1;
            User.Id = userId;
            Status = status;
            Name = name;
        }
        public ComplexRequest(int userId, SimpleRequestStatus status, List<SimpleRequest> simpleRequests)
        {
            User = new User();
            Id = -1;
            User.Id = userId;
            Status = status;
            SimpleRequests = new List<SimpleRequest>(simpleRequests);
        }
        public string[] ToCSV()
        {
            string simpleRequestsIds = "";
            foreach (var simpleRequest in SimpleRequests)
            {
                simpleRequestsIds += simpleRequest.Id.ToString() + ",";
            }
            simpleRequestsIds = simpleRequestsIds.Remove(simpleRequestsIds.Length - 1);
            string[] csvValues = { 
                                   Id.ToString(),
                                   User.Id.ToString(),
                                   Status.ToString(),
                                   Name.ToString(), 
                                   simpleRequestsIds
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            switch (values[2])
            {
                case "APPROVED":
                    Status = SimpleRequestStatus.APPROVED;
                    break;
                case "ON_HOLD":
                    Status = SimpleRequestStatus.ON_HOLD;
                    break;
                case "INVALID":
                    Status = SimpleRequestStatus.INVALID;
                    break;
                default:
                    throw new ArgumentException("Simple request Status in CSV does not exist");
            }
            Name = Convert.ToString(values[3]); 
            string[] simpleRequestsIds = values[4].Split(",");
            foreach(string id in simpleRequestsIds)
            {
                var simpleRequest = new SimpleRequest();
                simpleRequest.Id = Convert.ToInt32(id);
                SimpleRequests.Add(simpleRequest);
            }
        }
        public string GetStatusUri()
        {
            switch (Status)
            {
                case SimpleRequestStatus.APPROVED:
                    return "../../../Resources/Icons/approved.png";
                case SimpleRequestStatus.ON_HOLD:
                    return "../../../Resources/Icons/onhold.png";
                case SimpleRequestStatus.INVALID:
                    return "../../../Resources/Icons/invalid.png";
                default:
                    return "../../../Resources/Icons/unknown.png";
            }
        }
    }
}

