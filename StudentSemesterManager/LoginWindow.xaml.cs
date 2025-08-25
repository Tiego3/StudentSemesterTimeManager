using Microsoft.EntityFrameworkCore;
using SemesterCore.Data;
using SemesterCore.Models;
using SemesterCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentSemesterManager
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AppDbContext _db;

        public LoginWindow()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ModuleTrackerDb;Trusted_Connection=True;")
                .Options;
            _db = new AppDbContext(options);
            _db.Database.EnsureCreated();
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;
            await Task.Run(() =>
            {
                
                var hash = Security.HashPassword(password);

                var user = _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hash).Result;
                if (user != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var main = new MainWindow(user.Id);
                        main.Show();
                        Close();
                    });
                }
                else
                {
                    Dispatcher.Invoke(() => MessageBlock.Text = "Invalid credentials.");
                }
            });
        }

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;

            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Dispatcher.Invoke(() => MessageBlock.Text = "Username and password required.");
                    return;
                }

                if (_db.Users.Any(u => u.Username == username))
                {
                    Dispatcher.Invoke(() => MessageBlock.Text = "Username already exists.");
                    return;
                }

                var user = new User { Username = username, PasswordHash = Security.HashPassword(password) };
                _db.Users.Add(user);
                _db.SaveChanges();
                Dispatcher.Invoke(() => MessageBlock.Text = "Registration successful. Please login.");
            });
        }
    }
}
