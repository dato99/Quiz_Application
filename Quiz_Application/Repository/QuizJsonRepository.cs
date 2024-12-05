using Quiz_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quiz_Application.Repository
{
    public class QuizJsonRepository
    {
        private readonly string _filePath;
        private readonly List<Quiz> _quizzes;

        public QuizJsonRepository(string filePath)
        {
            _filePath = filePath;
            _quizzes = LoadData();
        }
        public List<Quiz> GetQuizzes() => _quizzes;
        public Quiz GetQuiz(string quizName) => _quizzes.FirstOrDefault(acc => acc.Name == quizName);
        public List<Quiz> GetQuizzesByAuthor(string author) => _quizzes.Where(x => x.Author == author).ToList();



        public void CreateNewQuiz(Quiz quiz)
        {
            if (!_quizzes.Any(x => x.Name == quiz.Name))
            {
                _quizzes.Add(quiz);
                SaveData();
            }
            else Console.WriteLine("A quiz with this title is already available.");
        }
        public void DeleteQuiz(string quizName)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Name == quizName);

            if (quiz != null)
            {
                _quizzes.Remove(quiz);
                SaveData();
            }
        }

        public void SaveData()
        {

            var json = JsonSerializer.Serialize(_quizzes, new JsonSerializerOptions { WriteIndented = true });

            using (var writer = new StreamWriter(_filePath, false))
                writer.Write(json);
        }
        private List<Quiz> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<Quiz>();
            using (var reader = new StreamReader(_filePath))
            {
                var json = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(json))
                    return new List<Quiz>();
                try
                {
                    return JsonSerializer.Deserialize<List<Quiz>>(json) ?? new List<Quiz>();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Error loading quizzes.");
                    return new List<Quiz>();
                }
            }
        }
    }
}
