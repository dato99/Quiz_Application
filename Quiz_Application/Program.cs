using Quiz_Application.Repository;
using Quiz_Application.Models;
namespace Quiz_Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QuizJsonRepository quizRep = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
            UserJsonRepository userRep = new UserJsonRepository(@"../../../Repository/Data/User.json");
            UIRepository Ui = new UIRepository();
            userRep.GetTopUsers();
            Ui.Autorization();
        }
    }
}
