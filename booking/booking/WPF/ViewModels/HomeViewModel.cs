﻿using application.UseCases;
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
        private User _user;
        public RelayCommand GenerateReportCommand => new RelayCommand(OnGenerateReport);
        public RelayCommand CheckGenerabilityCommand => new RelayCommand(CanGenerateReport);
        public HomeViewModel(User user)
        {
            ReportFromDate = DateTime.Now;
            ReportToDate = DateTime.Now;
            this._user = user;
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
                string filePath = "C:\\Users\\Marko\\Desktop\\" + _user.Username + "_report_" + uniqueId.Substring(uniqueId.Count()-4) + ".pdf";
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                document.AddTitle("Visited tours report");

                Font titleFont = new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD);
                Font contentFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);

                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Visited tours report", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Add some content to the document
                iTextSharp.text.Paragraph content = new iTextSharp.text.Paragraph($"Guest {_user.Username}, in period of {ReportFromDate.Date} - {ReportToDate.Date}, has visited next tours:", contentFont);
                content.SpacingBefore = 20f;
                content.SpacingAfter = 20f;
                document.Add(content);

                // Create a table
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;

                // Add table headers
                table.AddCell("Name");
                table.AddCell("Age");
                table.AddCell("Country");

                // Add table rows
                table.AddCell("John Doe");
                table.AddCell("30");
                table.AddCell("USA");
                table.AddCell("Jane Smith");
                table.AddCell("25");
                table.AddCell("Canada");

                document.Add(table);

                document.Close();
                MessageBox.Show("PDF generated successfully!");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during PDF generation
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
        }
    }
}
