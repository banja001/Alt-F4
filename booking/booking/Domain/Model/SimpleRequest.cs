using booking.Model;
using booking.Serializer;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace Domain.Model
{
    public enum SimpleRequestStatus
    {
        APPROVED,
        ON_HOLD,
        INVALID
    }

    public class SimpleRequest : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }  
        public string Language { get; set; }    
        public int NumberOfGuests { get; set; } 
        public DateRange DateRange { get; set; }
        public SimpleRequestStatus Status { get; set; }
        public bool IsPartOfComplex { get; set; }

        public SimpleRequest()
        {
            this.User = new User();
            DateRange = new DateRange();
            Location = new Location();
        }
        public SimpleRequest( int userId, string description, int locationId, string language, int numberOfGuests,DateTime startDate, DateTime endDate, SimpleRequestStatus status)
        {
            Location = new Location();
            User = new User();

            Id = -1;
            User.Id = userId;
            Description = description;
            Location.Id = locationId;
            Language = language;
            NumberOfGuests = numberOfGuests;
            DateRange = new DateRange(startDate, endDate);
            Status = status;
            IsPartOfComplex = false;
        }
        public SimpleRequest(int id, SimpleRequest simpleRequest)
        {
            Id = id;
            User = simpleRequest.User;
            Description = simpleRequest.Description;
            Location = simpleRequest.Location;
            Language = simpleRequest.Language;
            NumberOfGuests = simpleRequest.NumberOfGuests;
            DateRange = simpleRequest.DateRange;
            Status = simpleRequest.Status;
            IsPartOfComplex = false;
        }
        public SimpleRequest(User user, SimpleRequestDTO simpleRequestDTO)
        {
            Id = simpleRequestDTO.Id;
            User = user;
            Description = simpleRequestDTO.Description;
            Location = simpleRequestDTO.Location;
            Language = simpleRequestDTO.Language;
            NumberOfGuests = simpleRequestDTO.NumberOfGuests;
            DateRange = new DateRange(simpleRequestDTO.StartDate, simpleRequestDTO.EndDate);
            IsPartOfComplex = false;

            switch (simpleRequestDTO.StatusUri)
            {
                case "../../../Resources/Icons/approved.png":
                    {
                        Status = SimpleRequestStatus.APPROVED; break;
                    }
                case "../../../Resources/Icons/onhold.png":
                    {
                        Status = SimpleRequestStatus.ON_HOLD; break;
                    }
                case "../../../Resources/Icons/invalid.png":
                    {
                        Status = SimpleRequestStatus.INVALID; break;
                    }
                default:
                    throw new ArgumentException("Exception thrown when converting from StatusURI to SimpleRequeststatus.");
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),
                                   User.Id.ToString(),
                                   Description,            
                                   Location.Id.ToString(),
                                   Language,
                                   NumberOfGuests.ToString(),
                                   DateRange.StartDate.ToString("d", CultureInfo.GetCultureInfo("es-ES")),
                                   DateRange.EndDate.ToString("d", CultureInfo.GetCultureInfo("es-ES")),
                                   Status.ToString(),
                                   IsPartOfComplex.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            Description = Convert.ToString(values[2]);  
            Location.Id = Convert.ToInt32(values[3]);   
            Language = Convert.ToString(values[4]);
            NumberOfGuests = Convert.ToInt32(values[5]);
            DateRange.StartDate = Convert.ToDateTime(values[6], CultureInfo.GetCultureInfo("es-ES"));
            DateRange.EndDate = Convert.ToDateTime(values[7], CultureInfo.GetCultureInfo("es-ES"));
            switch(values[8])
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
            IsPartOfComplex = Convert.ToBoolean(values[9]);
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
