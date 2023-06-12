using booking.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace booking.Model
{
    public class ReservedDates : ISerializable, INotifyPropertyChanged
    {
        int numOfGuests;
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccommodationId { get; set; }

        public int NumOfGuests { get; set; }

        public int UserId { get; set; }
        public bool RatedByOwner { get; set; }

        public bool RatedByGuest { get; set; }

        public DateTime DateOfReserving { get; set; }

        public ReservedDates() { }

        public ReservedDates(DateTime startDate, DateTime endDate, int accommodationId, int userId = -1, bool ratedByOwner = false, int id = 0, int numOfGuests = 0, bool ratedByGuest = false, DateTime? dateOfReserving = null)
        { 
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
            UserId = userId;
            RatedByOwner = ratedByOwner;
            NumOfGuests = numOfGuests;
            UserId = userId;
            RatedByGuest = ratedByGuest;
            DateOfReserving = (dateOfReserving == null) ? new DateTime(0001, 01, 01): dateOfReserving.Value;
        }

        public ReservedDates(ReservedDates reservedDate)
        {
            Id = reservedDate.Id;
            StartDate = reservedDate.StartDate;
            EndDate = reservedDate.EndDate;
            AccommodationId = reservedDate.AccommodationId;
            UserId = reservedDate.UserId;
            RatedByOwner = reservedDate.RatedByOwner;
            NumOfGuests = reservedDate.NumOfGuests;
            UserId = reservedDate.UserId;
            RatedByGuest = reservedDate.RatedByGuest;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "StartDate")
                {
                    if (StartDate < DateTime.Today)
                    {
                        MessageBox.Show("Start date has to be some date after todays date");
                        return "greater than today's date";
                    }
                }

                else if(columnName == "EndDate")
                {
                    if ((EndDate < StartDate && EndDate.Date > DateTime.Today.Date) || (EndDate.Date < DateTime.Today.Date))
                    {
                        MessageBox.Show("End date has to be set to some date after the starting one");
                        return "greater than Start date";
                    }
                }
               
                return null;
            }
        }


        private readonly string[] _validatedProperties = { "NumOfGuests", "StartDate", "EndDate" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"), AccommodationId.ToString(), NumOfGuests.ToString(), UserId.ToString(),
            RatedByOwner.ToString().ToLower(), RatedByGuest.ToString().ToLower(), DateOfReserving.ToString("dd/MM/yyyy") };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateTime.ParseExact(values[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AccommodationId = Convert.ToInt32(values[3]);
            NumOfGuests = Convert.ToInt32(values[4]);
            UserId = Convert.ToInt32(values[5]);
            RatedByOwner = values[6] == "true" ? true : false;
            RatedByGuest = values[7] == "true" ? true : false;
            DateOfReserving = DateTime.ParseExact(values[8], "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
