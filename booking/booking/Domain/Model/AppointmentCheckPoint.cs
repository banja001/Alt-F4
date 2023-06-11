using booking.Commands;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using booking.Repository;
using System.ComponentModel;
using LiveCharts.Wpf;

namespace booking.Model
{
    public class AppointmentCheckPoint:ISerializable,INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool active;
        private bool notChecked;
        public bool Active 
        {
            get { return active; }
            set
            {
                active = value;
                OnPropertyChanged(nameof(Active));
            }
        }
        public bool NotChecked
        {
            get { return notChecked; }
            set
            {
                notChecked = value;
                OnPropertyChanged(nameof(NotChecked));
            }
        }
        public int AppointmentId { get; set; }
        public int Order { get; set; }

        public AppointmentCheckPoint()
        {

        }

        public AppointmentCheckPoint(int id, string name, bool active,bool notchecked, int appointmentId, int order)
        {
            Id = id;
            Name = name;
            Active = active;
            NotChecked= notchecked;
            AppointmentId = appointmentId;
            Order = order;
            NotChecked = !active;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                                    Id.ToString(),
                                    Name.ToString(),
                                    Active.ToString(),
                                    NotChecked.ToString(),
                                    AppointmentId.ToString(),
                                    Order.ToString(),
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = Convert.ToString(values[1]);
            Active = Convert.ToBoolean(values[2]);
            NotChecked= Convert.ToBoolean(values[3]);
            AppointmentId = Convert.ToInt32(values[4]);
            Order = Convert.ToInt32(values[5]);
        }

        public ICommand CheckPointCommand => new RelayCommand(CheckPointClick,CanClick);
        public void CheckPointClick()
        {
            this.NotChecked = false;
            this.Active = true;
            AppointmentCheckPointRepository _appointmentCheckPointRepository = new AppointmentCheckPointRepository();
            _appointmentCheckPointRepository.SaveOneInFile(this);
        }

        private bool CanClick()
        {
            return this.NotChecked;
        }
    }
}
