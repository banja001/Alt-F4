using booking.Domain.Model;
using booking.Injector;
using booking.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.application.UseCases.Guest2
{
    public class GuideRatingImageService
    {
        private readonly GuideRatingImage _guideRatingImage;
        private readonly GuideRatingImageRepository _guideRatingImageRepository;
        public GuideRatingImageService()
        {
            _guideRatingImageRepository = Injector.Injector.CreateInstance<GuideRatingImageRepository>();
            _guideRatingImage = new GuideRatingImage();
        }
        public void AddImage(string url, GuideRating guideRating)
        {
            _guideRatingImage.Id = _guideRatingImageRepository.MakeID();
            _guideRatingImage.Url = url;
            _guideRatingImage.GuideRatingId = guideRating.Id;
            _guideRatingImageRepository.Add(_guideRatingImage);
        }
    }
}
