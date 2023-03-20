using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class AnswerRepository
    {
        private List<Answer> answers;
        private Serializer<Answer> serializer;

        private readonly string fileName = "../../../Resources/Data/answer.csv";

        public AnswerRepository()
        {
            serializer = new Serializer<Answer>();
            answers = serializer.FromCSV(fileName);
        }
        public List<Answer> FindAll()
        {
            return answers;
        }
        public void Add(Answer answer)
        {
            answers.Add(answer);
            serializer.ToCSV(fileName, answers);
        }
        public Answer FindById(int id) 
        {
            return answers.Find(i => i.Id == id);
        }
        public int MakeID()
        {
            if (answers.Count != 0)
                return answers[answers.Count - 1].Id + 1;
            return 1;
        }
        public void SaveOneInFile(Answer answer)
        {
            Answer ap = answers.Find(a => a.Id == answer.Id);
            ap.HaveToAnswer = answer.HaveToAnswer;
            serializer.ToCSV(fileName, answers);
        }
    }
}
