using booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Model;

namespace booking.application.UseCases
{
    public class TourService
    {
        private readonly TourRepository _tourRepository;
        public TourService()
        {
            _tourRepository = new TourRepository();
        }

        public List<Tour> FindAll()
        {
            return _tourRepository.FindAll();
        }
    }
}
