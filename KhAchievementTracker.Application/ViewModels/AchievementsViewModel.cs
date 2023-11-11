using KhAchievementTracker.Tools.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KhAchievementTracker.Application.ViewModels
{
    public class AchievementsViewModel : GenericListModel<AchievementViewModel>
    {
        public AchievementsViewModel(IEnumerable<AchievementModel> achievements) : this(achievements.Select(Map)) { }
        public AchievementsViewModel(IEnumerable<AchievementViewModel> list) : base(list)
        { }

        public void UnlockAchievements(IEnumerable<string> list)
        {
            var toUnlock = Items.Where(x => list.Contains(x.Id));
            if (toUnlock.Any())
            {
                foreach(var item in toUnlock)
                    item.Unlocked = true;
            }
        }
        public void Reset()
        {
            foreach (var item in Items) 
                item.Unlocked = false;
        }

        public IEnumerable<AchievementModel> ToAchievementModels() => Items.Select(x => x.Achievement);
        private static AchievementViewModel Map(AchievementModel model) => new(model);
    }
}
