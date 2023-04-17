using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public DateAndTime ObtainDate { get; set; }
        public int GuideId { get; set; }
        public int Guest2Id { get; set; }
        public DateAndTime ExpirationDate { get; set; }
        public bool IsUsed {get; set;}
        public Voucher() 
        {
            ObtainDate = new DateAndTime();
            ExpirationDate = new DateAndTime();
        }
        public Voucher(int id, DateAndTime obtainDate, int guideId, int guest2Id, DateAndTime expirationDate, bool isUsed)
        {
            Id = id;
            ObtainDate = obtainDate;
            GuideId = guideId;
            Guest2Id = guest2Id;
            ExpirationDate = expirationDate;
            IsUsed = isUsed;
        }
        public bool IsUsable()
        {
            return ObtainDate.Date.AddDays(180) >= DateTime.Now;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {Id.ToString(),
                                  ObtainDate.ToString(),
                                  GuideId.ToString(),
                                  Guest2Id.ToString(),
                                  ExpirationDate.ToString(),
                                  IsUsed.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            string[] dateAndTime = Convert.ToString(values[1]).Split(" ");
            ObtainDate.Date = Convert.ToDateTime(dateAndTime[0], CultureInfo.GetCultureInfo("es-ES"));
            ObtainDate.Time = dateAndTime[1];
            GuideId = int.Parse(values[2]);
            Guest2Id= int.Parse(values[3]);
            dateAndTime = Convert.ToString(values[4]).Split(" ");
            ExpirationDate.Date = Convert.ToDateTime(dateAndTime[0], CultureInfo.GetCultureInfo("es-ES"));
            ExpirationDate.Time = dateAndTime[1];
            IsUsed = bool.Parse(values[5]);
        }
    }
}
