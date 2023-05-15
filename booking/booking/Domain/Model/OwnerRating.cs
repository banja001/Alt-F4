﻿using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public enum Urgency { Level1, Level2, Level3, Level4, Level5};
    public class OwnerRating : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CleanRating { get; set; }
        public int KindRating { get; set; }
        public string Comment { get; set; }

        public int ReservationId { get; set; }
        public string RenovationDescription { get; set; }
        public Urgency Urgency { get; set; }

        public OwnerRating() { }
        public OwnerRating(int id, int guestid, int cleanRating, int rulesrating, string comment, string renovationDescription, string urgency)
        {
            Id = id;
            OwnerId = guestid;
            CleanRating = cleanRating;
            KindRating = rulesrating;
            Comment = comment;
            RenovationDescription = renovationDescription;
            Urgency = ConvertToUrgencyFromString(urgency);
        }

        private Urgency ConvertToUrgencyFromString(string s)
        {
            switch (s)
            {
                case "Level1":
                    return Urgency.Level1;
                    break;
                case "Level2":
                    return Urgency.Level2;
                    break;
                case "Level3":
                    return Urgency.Level3;
                    break;
                case "Level4":
                    return Urgency.Level4;
                    break;
                case "Level5":
                    return Urgency.Level5;
                    break;
            }

            return Urgency.Level1;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), CleanRating.ToString(), KindRating.ToString(), Comment, ReservationId.ToString(), RenovationDescription, Urgency.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            CleanRating = Convert.ToInt32(values[2]);
            KindRating = Convert.ToInt32(values[3]);
            Comment = values[4];
            ReservationId = Convert.ToInt32(values[5]);
            RenovationDescription = values[6];
            Urgency = ConvertToUrgencyFromString(values[7]);
        }
    }
}