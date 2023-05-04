using booking.Domain.Model;
using booking.Domain.RepositoryInterfaces;
using booking.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.application.UseCases.Guest2
{
    
    public class GuideRatingService
    {
        private readonly IGuideRatingRepository _guideRatingRepository;
        
        public GuideRatingService()
        {
            _guideRatingRepository = Injector.Injector.CreateInstance<IGuideRatingRepository>();
        }
        public GuideRating AddRating(int tourKnowledge, int languageKnowledge, int tourEnjoyment, int appointmentId, string comment, int guest2Id)
        {
            GuideRating guideRating = new GuideRating(_guideRatingRepository.MakeID(),
                                                        tourKnowledge,
                                                        languageKnowledge,
                                                        tourEnjoyment,
                                                        appointmentId,
                                                        comment,
                                                        guest2Id,
                                                        true);
            _guideRatingRepository.Add(guideRating);
            return guideRating;
        }
        public int GetGuideRatingId(GuideRating guideRating)
        {
            return _guideRatingRepository.GetIdOf(guideRating);
        }
    }
}
