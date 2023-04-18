using System;
using System.Collections.Generic;
using System.Text;
using booking.Domain.DTO;
using booking.Model;

namespace WPF.ViewModels
{
    public class SelectedCommentViewModel
    {
        public User Guide { get; set; }
        public TourRatingDTO Comment { get; set; }
        public SelectedCommentViewModel(User guide, TourRatingDTO rating)
        {
            Guide = guide;
            Comment = rating;
        }
    }
}
