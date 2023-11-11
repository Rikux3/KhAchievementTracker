using KhAchievementTracker.Tools.MVVM;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KhAchievementTracker.Application.ViewModels
{
    public class AchievementViewModel : BaseNotifyPropertyChanged
    {
        public AchievementModel Achievement { get; set; }
        public string Id { get => Achievement.Id; set => Achievement.Id = value; }
        public string Name { get => Achievement.Name; set=> Achievement.Name = value; }
        public string Description { get => Achievement.Description; set => Achievement.Description = value; }
        public bool Unlocked
        {
            get => Achievement.Unlocked;
            set { Achievement.Unlocked = value; OnPropertyChanged(); }
        }
        public ImageSource Image => new BitmapImage(new System.Uri($"pack://application:,,,/KhAchievementTracker.Application;component/Icons/{Id}.PNG"));

        public DelegateCommand OnUnlockedChanged { get; set; }

        public AchievementViewModel(AchievementModel model)
        {
            Achievement = model;
            OnUnlockedChanged = new DelegateCommand(x =>
            {
                Unlocked = !Unlocked;
            });
        }
    }
}
