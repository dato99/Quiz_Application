using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Models
{
    public class Quiz
    {
        public string Author { get; set; }
        public string Name { get; set; }


        public List<Question> Questions { get; set; }


        public Quiz(string author, string name, List<Question> questions)
        {
            Author = author;
            Name = name;
            Questions = questions;
        }
        public class Question
        {
            public string Text { get; set; }
            public string[] Answers { get; set; }
            public byte CorrectAnswer { get; set; }

            public Question(string text, string[] answers, byte correctAnswer)
            {

                Text = text;
                Answers = answers;
                CorrectAnswer = correctAnswer;
            }
        }
    }
}
