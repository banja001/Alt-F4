using booking.Commands;
using booking.Domain.DTO;
using booking.WPF.ViewModels;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using booking.Model;
using application.UseCases;
using System.Linq;
using Domain.DTO;

namespace WPF.ViewModels.Guest1
{
    public class Guest1ReportViewModel : BaseViewModel
    {
        public bool ReservedChecked { get; set; }
        public bool CanceledChecked { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private List<ReservedDates> CanceledDates { get; set; }
        private List<Guest1ReportDTO> Guest1ReportDTOs { get; set; }
        private List<ReservedDates> ReservedDates { get; set; }

        private User user;

        private string title = "REPORT ON ";

        private readonly ReservedDatesService _reservedDatesService;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;
        public ICommand CancelCommand => new RelayCommand(Cancel);
        public ICommand GenerateCommand => new RelayCommand(Generate);
        public Guest1ReportViewModel(User user)
        {
            _reservedDatesService = new ReservedDatesService();
            _accommodationService = new AccommodationService();
            _locationService = new LocationService();

            CanceledDates = _reservedDatesService.GetAllCanceled().Where(d => d.UserId == user.Id).ToList();
            ReservedDates = _reservedDatesService.GetByGuestId(user.Id);

            this.user = user;

            StartDate = new DateTime();
            EndDate = new DateTime();
            Guest1ReportDTOs = new List<Guest1ReportDTO>();
        }

        private void Cancel()
        {
            CloseCurrentWindow();
        }

        private void Generate()
        {
            Guest1ReportDTOs.Clear();
            title = "REPORT ON ";

            if (ReservedChecked)
                AddReservedToList();
            if (CanceledChecked)
            {
                title += "/";
                AddCanceledToList();
            }

            title += " RESERVATIONS";

            Document document = new Document();

            using (FileStream fileSteram = new FileStream($"../../../Resources/Reports/guest1Report_{Guid.NewGuid()}.pdf", FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fileSteram);
                document.Open();

                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                Chunk boldChunk = new Chunk(title, boldFont);

                Paragraph titleParagraph = new Paragraph();
                titleParagraph.Alignment = Element.ALIGN_CENTER;
                titleParagraph.Add(boldChunk);
                document.Add(titleParagraph);

                string pdfText = "\n\n\n\nRequested report: " + user.Username +
                    "\nWhen: " + DateTime.Now.ToString("dd/MM/yyyy") +
                    "\nProvided: ALT+F4\n\n\n";
 
                document.Add(new Paragraph(pdfText));

                titleParagraph.SpacingAfter = 20f;

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100f;

                table.AddCell(new PdfPCell(new Phrase("Accommodation name")) { HorizontalAlignment = Element.ALIGN_CENTER});
                table.AddCell(new PdfPCell(new Phrase("Location")) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Start date")) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("End date")) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Status")) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Date of reservation/cancelation")) { HorizontalAlignment = Element.ALIGN_CENTER });

                foreach (var item in Guest1ReportDTOs)
                {
                    table.AddCell(item.AccommodationName);
                    table.AddCell(item.Location);
                    table.AddCell(item.StartDate.ToString("dd/MM/yyyy"));
                    table.AddCell(item.EndDate.ToString("dd/MM/yyyy"));
                    table.AddCell(item.ReservationStatus.ToString());
                    table.AddCell(item.DateOfReserving.ToString("dd/MM/yyyy"));
                }
                document.Add(table);

                document.Close();
            }

            MessageBox.Show("Report successfully generated");
        }

        private void AddReservedToList()
        {
            foreach(var reservedDate in ReservedDates)
            {
                Accommodation accommodation = _accommodationService.GetById(reservedDate.AccommodationId);
                Location location = _locationService.GetById(accommodation.LocationId);

                if (reservedDate.DateOfReserving.Date >= StartDate.Date && reservedDate.DateOfReserving.Date <= EndDate.Date)
                    Guest1ReportDTOs.Add(new Guest1ReportDTO(accommodation.Name, location.State + "," + location.City, reservedDate.StartDate, reservedDate.EndDate, ReservationStatus.RESERVED, reservedDate.DateOfReserving));
            }

            title += "RESERVED";
        }

        private void AddCanceledToList()
        {
            foreach (var canceledDates in CanceledDates)
            {
                Accommodation accommodation = _accommodationService.GetById(canceledDates.AccommodationId);
                Location location = _locationService.GetById(accommodation.LocationId);

                if (canceledDates.DateOfReserving.Date >= StartDate.Date && canceledDates.DateOfReserving.Date <= EndDate.Date)
                    Guest1ReportDTOs.Add(new Guest1ReportDTO(accommodation.Name, location.State + "," + location.City, canceledDates.StartDate, canceledDates.EndDate, ReservationStatus.CANCELED, canceledDates.DateOfReserving));
            }

            title += "CANCELED";
        }
    }
}
