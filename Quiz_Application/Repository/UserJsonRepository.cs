using Quiz_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quiz_Application.Repository
{
    public class UserJsonRepository
    {
        private readonly string _filePath;
        private readonly List<User> _users;

        public UserJsonRepository(string filePath)
        {
            _filePath = filePath;
            _users = LoadData();
        }
        public List<User> GetUsers() => _users;
        public void GetTopUsers()
        {
            List<User> topUsers = _users.OrderByDescending(user => user.Score).Take(10).ToList();
            Console.WriteLine("-*-*-TOP USERS-*-*-");
            foreach (var user in topUsers)
            {
                Console.WriteLine(user.UserName);
            }
            Console.WriteLine("--------------------");
        }
        public User Get_user(string username)
        {
            var user = _users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                Console.WriteLine("User not found!");
            }
            return user;
        }
        public bool UserExists(string userName)
        {
            if (_users.Any(x => x.UserName == userName))
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public bool SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username and password cannot be empty!");
                return false;
            }
            User activeUser = _users.FirstOrDefault(acc => acc.UserName == userName);

            if (activeUser == null)
            {
                Console.WriteLine("User not found!");
                return false;
            }
            if (activeUser.Password != password)
            {
                return false;
            }
            Console.WriteLine($"Welcome {userName}!!!");
            return true;
        }

        public void RegisterNewUser(User user)
        {
            if (!_users.Any(x => x.UserName == user.UserName))
            {
                _users.Add(user);
                SaveData();
            }
            else Console.WriteLine($"This username already exists!");
        }
        public void SaveData()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });

            using (var writer = new StreamWriter(_filePath, false))
            {
                writer.Write(json);
            }
        }


        private List<User> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    var json = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return new List<User>();
                    }
                    else
                        return JsonSerializer.Deserialize<List<User>>(json);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error loading JSON data: {ex.Message}");
                return new List<User>();
            }
        }
    }
}
