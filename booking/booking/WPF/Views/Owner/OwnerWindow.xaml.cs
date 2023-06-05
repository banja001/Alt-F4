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
        
       
        private bool ownerTypeTooltip = false;
        private void ClickTypeOfOwner(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GlobalVariables.tt == true)
            {
                if (ownerTypeTooltip)
                {
                    OwnerTypePopup.IsOpen = false;
                    ownerTypeTooltip = false;
                }
                else
                {
                    OwnerTypePopup.IsOpen = true;
                    ownerTypeTooltip = true;
                }
            }
        }
        private bool rateGuestsTooltip = false;
        private void ClickRateGuests(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GlobalVariables.tt == true)
            {
                if (rateGuestsTooltip)
                {
                    RateGuestsPopup.IsOpen = false;
                    rateGuestsTooltip = false;
                }
                else
                {
                    RateGuestsPopup.IsOpen = true;
                    rateGuestsTooltip = true;
                }
            }
        }
        
        private bool downloadPDFTooltip = false;
        private void ClickDownloadPDF(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GlobalVariables.tt == true)
            {
                if (downloadPDFTooltip)
                {
                    DownloadPDFPopup.IsOpen = false;
                    downloadPDFTooltip = false;
                }
                else
                {
                    DownloadPDFPopup.IsOpen = true;
                    downloadPDFTooltip = true;
                }
            }
        }


    }
}
