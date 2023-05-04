using booking.Domain.Model;
using booking.Domain.RepositoryInterfaces;
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
        private readonly IGuideRatingImageRepository _guideRatingImageRepository;
        public GuideRatingImageService()
        {
            _guideRatingImageRepository = Injector.Injector.CreateInstance<IGuideRatingImageRepository>();
        }
        public void AddImagesByGuideRatingId(List<GuideRatingImage> guideRatingImages, int guideRatingId)
        {
            foreach (var guideRatingImage in guideRatingImages)
            {
                _guideRatingImageRepository.Add(new GuideRatingImage(_guideRatingImageRepository.MakeID(), guideRatingImage.Url, guideRatingId));
            }
        }
    }
}
