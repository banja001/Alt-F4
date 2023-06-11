using Domain.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WPF.ViewModels.Owner;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Page
    {
        public OwnerViewModel OwnerModel;
        public OwnerWindow(int id)
        {
            InitializeComponent();
            OwnerModel = new OwnerViewModel(id);
            DataContext = OwnerModel;
            
        }
        
       
        
        
        
        


    }
}
