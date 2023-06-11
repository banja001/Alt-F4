using booking.application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using application.UseCases;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
using Domain.Model;
using System.Collections.ObjectModel;
using iTextSharp.text.pdf;
using System.Windows.Documents;
using System.Windows;
using iTextSharp.text;
using System.Diagnostics;

namespace WPF.ViewModels
{
    public class ReportGuideViewModel:BaseViewModel, INotifyPropertyChanged
    {
        private SimpleRequestService _simpleRequestService;
        private string selectedType;
        private UserService _userService;

        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }
        private AxesCollection axis;
        public AxesCollection Axis
        {
            get { return axis; }
            set
            {
                axis = value;
                OnPropertyChanged(nameof(Axis));
            }
        }
        private SeriesCollection series;
        public SeriesCollection Series
        {
            get { return series; }
            set
            {
                series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public User Guide { get; set; }
        public List<string> Types { get; set; }
        public CartesianChart YearsChart { get; set; }
        public ObservableCollection<SimpleRequest> AllRequests { get; set; }
        public ReportGuideViewModel(User guide)
        {
            AllRequests = new ObservableCollection<SimpleRequest>();
            Types = new List<string> { "Approved tour requests", "Invalid tour requests", "All tour requests"};
            Guide = guide;
            _simpleRequestService = new SimpleRequestService();
            _userService= new UserService();
            LoadRequests();
        }
        private void LoadRequests()
        {
            AllRequests.Clear();
            foreach (var request in _simpleRequestService.GetAllWithLocation())
            {
                AllRequests.Add(request);
            }
        }

        public ICommand CreateCommand => new RelayCommand(CreateReport, CanCreate);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CreateReport()
        {
            string uniqueId = Guid.NewGuid().ToString();
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            string projectFolderPath = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string folderPath = System.IO.Path.Combine(projectFolderPath, "Resources");
            string filePath = System.IO.Path.Combine(folderPath, "Reports/ReportOnTourRequests"+  DateTime.Now.ToString("_dd_MM_yyyy_HH_mm_ss") + ".pdf");
            List<string> data = new List<string>();
            List<SimpleRequest> AllRequests=new List<SimpleRequest>();
            if (SelectedType == "All tour requests")
                AllRequests = _simpleRequestService.GetAllWithLocation();
            if(SelectedType == "Approved tour requests")
                AllRequests = _simpleRequestService.GetAllAcceptedWithLocation();
            if(SelectedType== "Invalid tour requests")
                AllRequests = _simpleRequestService.GetAllDeclinedWithLocation();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();
                
                Image agencyImage = Image.GetInstance(folderPath + "/Icons/taskbarIcon.png");
                agencyImage.ScaleToFit(70f, 70f); // Adjust the size as needed
                agencyImage.Alignment = Image.TEXTWRAP | Image.ALIGN_LEFT;
                agencyImage.IndentationLeft = 9f;
                agencyImage.SpacingAfter = 9f;
                document.Add(agencyImage);
                
                iTextSharp.text.Paragraph par = new iTextSharp.text.Paragraph("Tourist agency\n Alt+F4", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD));
                par.Alignment = Element.ALIGN_LEFT;
                document.Add(par);
                /*
                iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph("\nReport for scheduled tours in periof from " + SelectedFirst.ToString("d") + " to " + SelectedSecond.ToString("d"), new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);
                */
                iTextSharp.text.Paragraph dataParagraph = new iTextSharp.text.Paragraph();
                foreach (string item in data)
                {
                    dataParagraph.Add(new Chunk(item + "\n", new Font(Font.FontFamily.HELVETICA, 12)));
                }
                dataParagraph.Add("\n");
                document.Add(dataParagraph);

                iTextSharp.text.Paragraph text = new iTextSharp.text.Paragraph("\n"+SelectedType+"\n\n", new Font(Font.FontFamily.HELVETICA, 18));
                text.Alignment = Element.ALIGN_CENTER;
                document.Add(text);

                PdfPTable table = new PdfPTable(9);
                table.WidthPercentage = 100f;
                float[] columnWidths = { 8f, 15.5f, 15.5f, 15.5f, 15.5f, 15.5f, 15.5f, 15.5f, 15.5f };
                table.SetWidths(columnWidths);

                for (int i = 0; i < 1; i++)
                {
                    table.AddCell(new PdfPCell(new Phrase("No", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Description", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Location", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("First date", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Last date", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Language", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Number of guests", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Guest", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                    table.AddCell(new PdfPCell(new Phrase("Status", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))));
                }

                int no = 1;
                foreach (SimpleRequest sr in AllRequests)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        table.AddCell(new PdfPCell(new Phrase(no.ToString()+".", new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.Description, new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.Location.CityState, new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.DateRange.StartDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.DateRange.EndDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.Language, new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.NumberOfGuests.ToString(), new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(_userService.GetById(sr.User.Id).Username, new Font(Font.FontFamily.HELVETICA, 8))));
                        table.AddCell(new PdfPCell(new Phrase(sr.Status.ToString(), new Font(Font.FontFamily.HELVETICA, 8))));
                    }
                    no++;
                }

                document.Add(table);

                

                iTextSharp.text.Paragraph stats = new iTextSharp.text.Paragraph();
                stats.Add("\n\nNumber of "+SelectedType.ToLower()+": " + AllRequests.Count.ToString());
                stats.Add("\nGuest most requesting tours: " + _simpleRequestService.GuestMostRequestedTours(AllRequests));
                stats.Add("\nMost requesting language: " + _simpleRequestService.MostSpokenLanguageOnRequestedTours(AllRequests));
                stats.Add("\nMost requesting state: " + _simpleRequestService.MostReguestedStateOnRequestedTours(AllRequests));
                stats.Add("\nMost requesting city: " + _simpleRequestService.MostReguestedLocationOnRequestedTours(AllRequests));
                stats.Add("\nMost requesting month: " + _simpleRequestService.MostReguestedMonthOnRequestedTours(AllRequests));
                stats.Add("\nMost requesting year: " + _simpleRequestService.MostReguestedYearOnRequestedTours(AllRequests));
                stats.Add("\nAvgrage number of guests: " + _simpleRequestService.AverageNumberOfGuestsOnRequestedTours(AllRequests));
                document.Add(stats);

                iTextSharp.text.Paragraph agencyParagraph = new iTextSharp.text.Paragraph("\n\n\nReport generated on: \n" + DateTime.Now.ToString(), new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD));
                agencyParagraph.Alignment = Element.ALIGN_RIGHT;
                iTextSharp.text.Paragraph userParagraph = new iTextSharp.text.Paragraph("\n\n\nReport generated by: "+ Guide.Username, new Font(Font.FontFamily.HELVETICA, 13,Font.BOLD));
                userParagraph.Alignment = Element.ALIGN_RIGHT;


                Image agencyImage1 = Image.GetInstance(folderPath + "/Icons/proa.png");
                agencyImage1.ScaleToFit(230f, 160f); 

                float imageWidth = agencyImage1.ScaledWidth;
                float imageHeight = agencyImage1.ScaledHeight;

                Chunk dummyChunk = new Chunk("Dummy Text", agencyParagraph.Font);
                float paragraphWidth = dummyChunk.GetWidthPoint();

                float imageX, imageY, paragraphX, paragraphY;



                if (SelectedType == "All tour requests")
                {
                    imageX = document.Right - imageWidth;
                     imageY = document.Top - imageHeight - 50f;
                     paragraphX = document.Right - paragraphWidth;
                     paragraphY = imageY - agencyParagraph.SpacingBefore;
                }
                else
                {
                    imageX = document.Right - imageWidth;
                    imageY = document.Bottom + paragraphWidth;
                    paragraphX = document.Right - paragraphWidth;
                    paragraphY = imageY - agencyParagraph.SpacingBefore;
                }
                agencyImage1.SetAbsolutePosition(imageX, imageY);
                
                document.Add(agencyImage1);
                document.Add(userParagraph);
                document.Add(agencyParagraph);

                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(agencyParagraph), paragraphX, paragraphY, 0);



                document.Close();

                MessageBox.Show("Successfully generated!");


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CreateStatisticForGraphYears()
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries YearsColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#AA96DA")) };
            Axis yearsAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            yearsAxis.Labels = new List<string>();
            yearsAxis.Foreground = new SolidColorBrush(Colors.Black);
            yearsAxis.FontSize = 15;
            var yearRequestCountPairs = _simpleRequestService.GetYearsForChart(AllRequests.ToList());
            foreach (var pair in yearRequestCountPairs)
            {
                yearsAxis.Labels.Add(pair.Key);
                YearsColumns.Values.Add(pair.Value);
            }
            Series.Add(YearsColumns);
            Axis.Add(yearsAxis);
            CartesianChart chart=new CartesianChart();
            chart.Series = Series;
            chart.AxisX = Axis;
            YearsChart= chart;
        }
        private bool CanCreate()
        {
            return SelectedType != null;
        }
    }
}
