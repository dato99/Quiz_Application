using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Score { get; set; } = 0;
        public User() { }
        public User(string username, string password)
        {
            UserName = username;
            Password = password;

            Score = Score;
        }
    }
}
