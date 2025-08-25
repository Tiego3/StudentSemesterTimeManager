using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SemesterCore.Data;
using SemesterCore.Models;
using SemesterCore.Services;
using SemesterCore.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StudentSemesterManager
{
    public partial class MainWindow : Window
    {
        private int _userId;
        private AppDbContext _db;

        public MainWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ModuleTrackerDb;Trusted_Connection=True;")
                .Options;
            _db = new AppDbContext(options);
            LoadData();
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                var semester = _db.SemesterInfos.FirstOrDefault(s => s.UserId == _userId);
                var modules = _db.Modules.Where(m => m.UserId == _userId).ToList();

                var modulesDisplay = modules.Select(m => new
                {
                    m.Code,
                    m.Name,
                    SelfStudyPerWeek = semester != null ? Calculations.GetSelfStudyHoursPerWeek(m, semester) : 0
                }).ToList();

                Dispatcher.Invoke(() =>
                {
                    ModulesList.ItemsSource = modulesDisplay;
                    ModuleSelect.ItemsSource = modules;
                });

                if (semester != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        WeeksBox.Text = semester.NumberOfWeeks.ToString();
                        StartDatePicker.SelectedDate = semester.StartDate;
                    });
                }

                // Calculate remaining self-study hours for current week
                var weekStart = semester != null ? GetCurrentWeekStart(semester.StartDate) : DateTime.Now;
                var weekEnd = weekStart.AddDays(7);

                var remainingDisplay = modules.Select(m =>
                {
                    var selfStudyPerWeek = semester != null ? Calculations.GetSelfStudyHoursPerWeek(m, semester) : 0;
                    var spent = _db.StudySessions
                        .Where(s => s.UserId == _userId && s.ModuleId == m.Id && s.Date >= weekStart && s.Date < weekEnd)
                        .Sum(s => s.Hours);
                    return new
                    {
                        m.Name,
                        RemainingHours = Math.Max(selfStudyPerWeek - spent, 0)
                    };
                }).ToList();

                Dispatcher.Invoke(() =>
                {
                    RemainingList.ItemsSource = remainingDisplay;
                });
            });
        }

        private async void SaveSemester_Click(object sender, RoutedEventArgs e)
        {
            // Read UI control values on UI thread FIRST
            int weeks;
            DateTime? startDate = StartDatePicker.SelectedDate;
            bool weeksValid = int.TryParse(WeeksBox.Text, out weeks);

            if (!weeksValid || !startDate.HasValue)
            {
                MessageBox.Show("Enter valid weeks and start date.");
                return;
            }

            await Task.Run(() =>
            {
                var semester = _db.SemesterInfos.FirstOrDefault(s => s.UserId == _userId);
                if (semester == null)
                {
                    semester = new SemesterInfos
                    {
                        UserId = _userId,
                        NumberOfWeeks = weeks,
                        StartDate = startDate.Value
                    };
                    _db.SemesterInfos.Add(semester);
                }
                else
                {
                    semester.NumberOfWeeks = weeks;
                    semester.StartDate = startDate.Value;
                }
                _db.SaveChanges();
                Dispatcher.Invoke(() => LoadData());
            });
        }

        private async void AddModule_Click(object sender, RoutedEventArgs e)
        {
            // Read UI values on UI thread first
            string code = CodeBox.Text;
            string name = NameBox.Text;
            bool creditsParsed = int.TryParse(CreditsBox.Text, out int credits);
            bool classHoursParsed = int.TryParse(ClassHoursBox.Text, out int classHours);

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name)
                || !creditsParsed || !classHoursParsed)
            {
                MessageBox.Show("Enter valid module details.");
                return;
            }

            await Task.Run(() =>
            {
                var module = new Module
                {
                    Code = code,
                    Name = name,
                    Credits = credits,
                    ClassHoursPerWeek = classHours,
                    UserId = _userId
                };
                _db.Modules.Add(module);
                _db.SaveChanges();
                Dispatcher.Invoke(() => LoadData());
            });
        }

        private async void RecordStudy_Click(object sender, RoutedEventArgs e)
        {
            // Read values on UI thread
            var selectedModule = ModuleSelect.SelectedItem as Module;
            DateTime? studyDate = StudyDatePicker.SelectedDate;
            bool hoursParsed = double.TryParse(HoursBox.Text, out double hours);

            if (selectedModule == null || !studyDate.HasValue || !hoursParsed)
            {
                MessageBox.Show("Select module, date and enter valid hours.");
                return;
            }

            await Task.Run(() =>
            {
                var session = new StudySession
                {
                    ModuleId = selectedModule.Id,
                    UserId = _userId,
                    Date = studyDate.Value,
                    Hours = hours
                };
                _db.StudySessions.Add(session);
                _db.SaveChanges();
                Dispatcher.Invoke(() => LoadData());
            });
        }

        private DateTime GetCurrentWeekStart(DateTime semesterStart)
        {
            var today = DateTime.Today;
            var daysSinceStart = (today - semesterStart).Days;
            var weekNum = Math.Max(0, daysSinceStart / 7);
            return semesterStart.AddDays(weekNum * 7);
        }
    }
}