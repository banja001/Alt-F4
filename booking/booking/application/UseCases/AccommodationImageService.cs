using booking.Injector;
using booking.Model;
using booking.View;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace application.UseCases
{
    public class AccommodationImageService
    {
        private IAccommodationImageRepository accommodationImageRepository;
        private OwnerWindow ownerWindow;

        public AccommodationImageService(OwnerWindow ow)
        {
            this.accommodationImageRepository = Injector.CreateInstance<IAccommodationImageRepository>();
            this.ownerWindow = ow;
        }

        public List<AccommodationImage> GetAll()
        {
            return accommodationImageRepository.GetAll();
        }
        public void Add(AccommodationImage acci)
        {
            accommodationImageRepository.Add(acci);
        }

        public void AddImages(Accommodation a, List<string> accommodationImagesUrl)
        {
            foreach (string url in accommodationImagesUrl)
            {
                AccommodationImage image;
                if (ownerWindow.accommodationImages.Count() == 0)
                {

                    image = new AccommodationImage(0, url, a.Id);
                }
                else
                {
                    image = new AccommodationImage(ownerWindow.accommodationImages.Max(a => a.Id) + 1, url, a.Id);
                }
                accommodationImageRepository.Add(image);
            }
        }

    }
}
