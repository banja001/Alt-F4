using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class AccomodationOverview : Window
    {
        public static ObservableCollection<AccommodationLocationDTO> Accommodations { get; set; }

        private readonly AccommodationRepository _repository;

        public AccomodationOverview()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<AccommodationLocationDTO>();
        }
    }
}
