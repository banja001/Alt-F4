using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    internal class CheckPointRepository
    {
        private List<CheckPoint> checkPoints;
        private Serializer<CheckPoint> serializer;

        private readonly string fileName = "../../../Resources/Data/checkpoint.csv";

        public CheckPointRepository()
        {
            serializer = new Serializer<CheckPoint>();
            checkPoints = serializer.FromCSV(fileName);
        }
        public List<CheckPoint> FindAll()
        {
            return checkPoints;
        }
        public void Add(CheckPoint checkPoint)
        {
            checkPoints.Add(checkPoint);
            serializer.ToCSV(fileName, checkPoints);
        }
        public void AddRange(List<CheckPoint> checkPointsFromListBox)
        {
            checkPoints.AddRange(checkPointsFromListBox);
            serializer.ToCSV(fileName, checkPoints);
        }

        public int MakeID()
        {
            return checkPoints[checkPoints.Count - 1].Id + 1;
        }
    }
}
