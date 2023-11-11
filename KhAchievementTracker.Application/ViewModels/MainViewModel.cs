using KhAchievementTracker.Application;
using KhAchievementTracker.Application.ViewModels;
using KhAchievementTracker.Application.Views;
using KhAchievementTracker.Tools.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace KhAchievementTracker.ViewModels
{
    internal class MainViewModel : BaseNotifyPropertyChanged
    {
        private Window Window => System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        private FileSystemWatcher _watcher;
        private DispatcherTimer _timer;
        //private Dictionary<GameInfo, List<AchievementModel>> _configuration;
        //private Dictionary<GameInfo, AchievementsViewModel> _viewModels;

        public Dictionary<GameInfo, AchievementsViewModel> ViewModels { get; private set; }
        public KeyValuePair<GameInfo, AchievementsViewModel> SelectedItem { get; set; }
        public int SelectedIndex { get; set; }
        public bool UseAutoSave
        {
            get => _timer.IsEnabled; set => _timer.IsEnabled = value;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand ResetSelectedCommand { get; set; }
        public DelegateCommand ResetAllCommand { get; set; }
        public DelegateCommand SingleViewCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public MainViewModel()
        {
            LoadGames("achievements");
            _watcher = new FileSystemWatcher(@"C:\temp\")
            {
                EnableRaisingEvents = true,
                Filter = "ach.txt"
            };
            _watcher.Created += _watcher_Created;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5),
                IsEnabled = false
            };
            _timer.Tick += _timer_Tick;

            SaveCommand = new DelegateCommand(x =>
            {
                SaveGames();
            });
            LoadCommand = new DelegateCommand(x =>
            {
                int sel = SelectedIndex;
                LoadGames("user");
                OnPropertyChanged(nameof(ViewModels));
                SelectedIndex = sel;
                OnPropertyChanged(nameof(SelectedIndex));
            });
            ResetSelectedCommand = new DelegateCommand(x =>
            {
                SelectedItem.Value.Reset();
            });
            ResetAllCommand = new DelegateCommand(x =>
            {
                foreach(var vm in ViewModels)
                    vm.Value.Reset();
            });
            SingleViewCommand = new DelegateCommand(x =>
            {
                SingleViewWindow singleViewWindow = new()
                {
                    DataContext = SelectedItem.Value
                };
                singleViewWindow.Show();
            });
            ExitCommand = new DelegateCommand(x => Window.Close());
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            SaveGames();
        }

        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
                return;

            var achievements = File.ReadAllLines(e.FullPath);
            foreach(var viewModel in ViewModels)
            {
                viewModel.Value.UnlockAchievements(achievements);
            }
        }

        private void SaveGames()
        {
            Dictionary<GameInfo, List<AchievementModel>> _configuration = new();
            foreach (var kvp in ViewModels)
            {
                var models = ViewModels[kvp.Key].ToAchievementModels();
                _configuration[kvp.Key] = models.ToList();
            }
            File.WriteAllText("user.json", JsonSerializer.Serialize(_configuration));
        }

        private void LoadGames(string fileName)
        {
            var _configuration = JsonSerializer.Deserialize<Dictionary<GameInfo, List<AchievementModel>>>(File.ReadAllText($"{fileName}.json"));
            ViewModels = new Dictionary<GameInfo, AchievementsViewModel>();
            foreach (var kvp in _configuration)
            {
                ViewModels[kvp.Key] = new AchievementsViewModel(kvp.Value);
            }
        }
    }
}
