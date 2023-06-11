using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
using booking.application.UseCases;
using System.Windows.Input;

namespace booking.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public DateTime ReportToDate {  get; set; }
        public DateTime ReportFromDate { get; set; }
        public bool ValidGeneration { get; set; }   
        public ObservableCollection<SimpleRequestTourDTO> SuggestionNotifications { get; set; }
        public ObservableCollection<SimpleRequestTourDTO> ApprovedNotifications { get; set; }
        private SimpleRequestService _simpleRequestService;
        private TourService _tourService;
        private User _user;
        public RelayCommand GenerateReportCommand => new RelayCommand(OnGenerateReport);
        public RelayCommand CheckGenerabilityCommand => new RelayCommand(CanGenerateReport);
        public HomeViewModel(User user)
        {
            ReportFromDate = DateTime.Now;
            ReportToDate = DateTime.Now;
            this._user = user;
            _tourService = new TourService();   
            _simpleRequestService = new SimpleRequestService();
            SuggestionNotifications = new ObservableCollection<SimpleRequestTourDTO>(_simpleRequestService.CreateSuggestionNotificationsByGuest2(user));
            ApprovedNotifications = new ObservableCollection<SimpleRequestTourDTO>(_simpleRequestService.CreateApprovedNotificationsByGuest2(user));
        }
        private void CanGenerateReport()
        {
            ValidGeneration = !ReportFromDate.Equals(ReportToDate) && ReportToDate > ReportFromDate;
            OnPropertyChanged(nameof(ValidGeneration));
        }
        private void OnGenerateReport()
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            try
            {
                string uniqueId = Guid.NewGuid().ToString();
                string filePath = "..\\..\\..\\Resources\\Reports\\" + _user.Username + "_report_" + uniqueId.Substring(uniqueId.Count()-4) + ".pdf";
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                document.AddTitle("Visited tours report");

                Font titleFont = new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD);
                Font contentFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);

                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Visited tours report", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                iTextSharp.text.Paragraph content = new iTextSharp.text.Paragraph($"Guest {_user.Username}, in period of {ReportFromDate.Date.ToShortDateString()} - {ReportToDate.Date.ToShortDateString()}, has visited next tours:", contentFont);
                content.SpacingBefore = 20f;
                content.SpacingAfter = 20f;
                document.Add(content);

                iTextSharp.text.List unorderedList = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED);
                unorderedList.SetListSymbol("\u2022");

                foreach (var tour in _tourService.GetVisitedToursByInterval(ReportFromDate, ReportToDate, _user))
                {
                    unorderedList.Add(new iTextSharp.text.ListItem($"{tour.Name} - {tour.StartTime.Date.ToShortDateString()}"));
                }

                document.Add(unorderedList);

                iTextSharp.text.Paragraph footer = new iTextSharp.text.Paragraph($"Report generated on: {DateTime.Now}\n by {_user.Username}", contentFont);
                footer.Alignment = Element.ALIGN_RIGHT;
                document.Add(footer);

                document.Close();
                MessageBox.Show("PDF generated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
        }
    }
}
