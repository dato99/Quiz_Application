using Quiz_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Repository
{
    public class UIRepository
    {
        UserJsonRepository userRepos = new UserJsonRepository(@"../../../Repository/Data/User.json");

        QuizJsonRepository quizRepos = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
        public delegate User GetcurrentUser();

        public static Timer _timer;
        public static bool _isWorking = true;

        public static User _activeUser = new();
        public void Autorization()
        {

            while (true)
            {
                Console.WriteLine("1. Register\n2. Sign In\nX. Exit");
                var choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "1")
                {
                    User newUser = SignUpFunction();
                }
                else if (choice == "2")
                {
                    User newUser = SignInFunction();
                }

                else if (choice == "X") break;
                else Console.WriteLine("Invalid option. Try again.");
            }
        }
        public void Actions(User activeUser)
        {
            Console.WriteLine("Choose option:\n 1. Create Quiz \n 2. Edit Quiz \n 3. Take a Quiz \n 4. Delete Quiz");

            string choice = Console.ReadLine();


            if (choice == "1")
            {
                newQuizInputs(activeUser);
            }
            else if (choice == "2")
            {
                EditQuiz(activeUser);
            }
            else if (choice == "3")
            {
                TakeQuiz(activeUser);
            }
            else if (choice == "4")
            {
                DeleteQuiz(activeUser);
            }
        }
        public void EditQuiz(User activeUser)
        {
            var userQuizes = quizRepos.GetQuizzesByAuthor(activeUser.UserName);

            if (userQuizes.Count == 0)
            {
                Console.WriteLine("There are no quizes available for editing.");
                return;
            }

            Console.WriteLine("Your quizzes:");
            for (int i = 0; i < userQuizes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizes[i].Name}");
            }

            Console.WriteLine("Enter the number of the quiz you want to edit or 'X' to return to the menu:");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToUpper() == "X")
                {
                    Console.WriteLine("Back to the menu now...");
                    Actions(activeUser);
                    return;
                }

                if (int.TryParse(input, out int quizIndex) && quizIndex >= 1 && quizIndex <= userQuizes.Count)
                {
                    Quiz quizToEdit = userQuizes[quizIndex - 1];
                    Console.WriteLine($"Editing Quiz: {quizToEdit.Name}");

                    string editChoice;
                    do
                    {
                        Console.WriteLine("\nChoose an option:");
                        Console.WriteLine("1. Edit Question");
                        Console.WriteLine("2. Edit Answer");
                        Console.WriteLine("3. Change Correct Answer");
                        Console.WriteLine("X. Exit Editing");

                        editChoice = Console.ReadLine();

                        if (editChoice == "1")
                        {
                            EditQuestion(quizToEdit);
                        }
                        else if (editChoice == "2")
                        {
                            EditAnswer(quizToEdit);
                        }
                        else if (editChoice == "3")
                        {
                            EditCorrectAnswer(quizToEdit);
                        }
                        else if (editChoice.ToUpper() != "X")
                        {
                            Console.WriteLine("Oops, that’s not correct. Please select a valid option.");
                        }

                    } while (editChoice.ToUpper() != "X");

                    quizRepos.SaveData();
                    Console.WriteLine("Quiz updates saved. Navigating back to the menu...");
                    Actions(activeUser);
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid entry. Please provide a valid quiz number or type 'X' to return to the menu.");
                }
            }
        }

        public void EditQuestion(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to edit (1..5):");
            int question_number = Convert.ToInt32(Console.ReadLine()) - 1;

            if (question_number >= 0 && question_number < quizToEdit.Questions.Count)
            {
                Console.WriteLine($"Current Question: {quizToEdit.Questions[question_number].Text}");
                Console.WriteLine("Enter new question text:");
                string newQuestionText = Console.ReadLine();
                quizToEdit.Questions[question_number].Text = newQuestionText;
            }
            else
            {
                Console.WriteLine("Please enter a valid question number.");
            }
        }

        public void EditAnswer(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to edit answers for (1..5):");
            int question_number = Convert.ToInt32(Console.ReadLine()) - 1;

            if (question_number >= 0 && question_number < quizToEdit.Questions.Count)
            {
                var question = quizToEdit.Questions[question_number];
                Console.WriteLine("Current Answers:");
                for (int i = 0; i < question.Answers.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i]}");
                }

                Console.WriteLine("Enter the answer number (1..4) you want to change:");
                int answerNumber = Convert.ToInt32(Console.ReadLine()) - 1;

                if (answerNumber >= 0 && answerNumber < question.Answers.Length)
                {
                    Console.WriteLine($"Current Answer: {question.Answers[answerNumber]}");
                    Console.WriteLine("Enter new answer:");
                    string newAnswer = Console.ReadLine();
                    question.Answers[answerNumber] = newAnswer;
                }
                else
                {
                    Console.WriteLine("Please enter a valid answer index.");
                }
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }
        }

        public void EditCorrectAnswer(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to change the correct answer for (1..5):");
            int question_number = Convert.ToInt32(Console.ReadLine()) - 1;

            if (question_number >= 0 && question_number < quizToEdit.Questions.Count)
            {
                var question = quizToEdit.Questions[question_number];
                Console.WriteLine($"Current Correct Answer: {question.Answers[question.CorrectAnswer - 1]}");
                Console.WriteLine("Enter the number of the correct answer (1..4):");
                byte correctAnswerIndex;
                while (!byte.TryParse(Console.ReadLine(), out correctAnswerIndex) || correctAnswerIndex < 1 || correctAnswerIndex > 4)
                {
                    Console.WriteLine("Invalid input! Please enter a number between 1..4");
                }

                question.CorrectAnswer = correctAnswerIndex;
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }
        }

        public void newQuizInputs(User activeUser)
        {
            Console.WriteLine("Enter the Quiz name");
            string quizName = Console.ReadLine();
            List<Quiz.Question> questions = new List<Quiz.Question>();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Enter Question N{i + 1} :");
                string questionText = Console.ReadLine();
                string[] answers = new string[4];
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine($"Enter Answer {j + 1}:");
                    answers[j] = Console.ReadLine();
                }
                Console.WriteLine("Enter the number of the correct answer (1..4):");
                byte correctAnswerIndex;
                while (!byte.TryParse(Console.ReadLine(), out correctAnswerIndex) || correctAnswerIndex < 1 || correctAnswerIndex > 4)
                {
                    Console.WriteLine("Invalid input! Please enter a number between 1..4.");
                }
                questions.Add(new Quiz.Question(questionText, answers, correctAnswerIndex));
            }

            Console.WriteLine("Quiz created successfully!!!");
            Quiz newQuiz = new Quiz(activeUser.UserName, quizName, questions);
            quizRepos.CreateNewQuiz(newQuiz);
            Actions(activeUser);
        }

        public User SignInFunction()
        {
            Console.WriteLine("Enter your Username:");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                return null;
            }

            if (userRepos.UserExists(userName))
            {
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();

                if (userRepos.SignIn(userName, password))
                {
                    User currentUser = userRepos.Get_user(userName);
                    _activeUser = currentUser;
                    Actions(_activeUser);
                    return currentUser;
                }
                else
                {
                    Console.WriteLine("Wrong password!");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("User not found.");
                return null;
            }
        }

        public User SignUpFunction()
        {
            Console.WriteLine("Enter your Username:");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                User newUser = new(userName, password);
                userRepos.RegisterNewUser(newUser);
                _activeUser = newUser;
                Console.WriteLine("Registration successfully.");
                Actions(_activeUser);
                return newUser;

            }
            else
            {
                Console.WriteLine("Username and/or password cannot be left empty.");
                return null;
            }
        }

        public void DeleteQuiz(User activeUser)
        {
            var userQuizes = quizRepos.GetQuizzesByAuthor(activeUser.UserName);

            if (userQuizes.Count == 0)
            {
                Console.WriteLine("You have not quizzes to delete.");
                return;
            }

            Console.WriteLine("Your quizzes:");
            for (int i = 0; i < userQuizes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizes[i].Name}");
            }

            Console.WriteLine("Enter the number of the quiz you want to delete or 'X' to return to the menu:");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToUpper() == "X")
                {
                    Console.WriteLine("Back to the menu now...");
                    Actions(activeUser);
                    return;
                }

                if (int.TryParse(input, out int quizIndex) && quizIndex >= 1 && quizIndex <= userQuizes.Count)
                {
                    Quiz quizToDelete = userQuizes[quizIndex - 1];

                    Console.WriteLine($"Are you sure you want to delete the quiz? '{quizToDelete.Name}'? (Y/N)");
                    string approval = Console.ReadLine();

                    if (approval.ToUpper() == "Y")
                    {
                        quizRepos.DeleteQuiz(quizToDelete.Name);
                        Console.WriteLine($"Quiz '{quizToDelete.Name}' has been deleted.");
                        Actions(activeUser);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Deletion canceled. Back to the menu now...");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid quiz number or 'X' to return to the menu.");
                }
            }
        }





        public void TakeQuiz(User activeUser)
        {
            Console.WriteLine("Available Quizzes:");
            var quizzes = quizRepos.GetQuizzes().Where(q => q.Author != activeUser.UserName).ToList();

            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizes available!");
                return;
            }

            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quizzes[i].Name}");
            }

            Console.WriteLine("Select the Quiz Number:");
            var EnteredquizIndex = Console.ReadLine();
            if (int.TryParse(EnteredquizIndex, out int quizIndex))
            {
                quizIndex -= 1;
                if (quizIndex >= 0 && quizIndex < quizzes.Count)
                {
                    Console.WriteLine($"You have selected quiz number {quizIndex + 1}.");
                    var quiz = quizzes[quizIndex];
                    Console.WriteLine($"Starting Quiz: {quiz.Name}");
                    StartQuiz(activeUser, quiz);
                }
                else
                {
                    Console.WriteLine("Invalid quiz number. Please choose a valid number.");
                    Actions(activeUser);
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
                Actions(activeUser);
            }
        }
        private void StartQuiz(User user, Quiz quiz)
        {
            var timer = new System.Timers.Timer(12000); 
            var questions = quiz.Questions;
            int score = 0;
            bool timeUp = false;

            timer.Elapsed += (sender, e) =>
            {
                timeUp = true;
                timer.Stop();


                Console.WriteLine("\nTime's up! Hit any key to proceed");
            };


            timer.Start();

            foreach (var question in questions)
            {
                if (timeUp) break;

                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Answers.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i]}");
                }

                Console.WriteLine("Enter your answer (1..4):");

                if (timeUp) break;
                string input = Console.ReadLine();
                if (timeUp) break;

                if (int.TryParse(input, out int answer) && answer >= 1 && answer <= 4)
                {
                    if (answer == question.CorrectAnswer) score += 20;
                }
                else
                {
                    Console.WriteLine("That’s not a valid answer. Skipping to the next question.");
                }
            }

            timer.Stop();


            user.Score += score;
            Console.WriteLine($"Quiz finished. Current Score: {score}; Your total score: {_activeUser.Score}");
            Actions(_activeUser);
        }
    }
}
