using System.Collections;
using System.Collections.ObjectModel;

namespace KhAchievementTracker.Tools.MVVM
{
    public class GenericListModel<T> : IEnumerable, IEnumerable<T>
    {
        public ObservableCollection<T> Items { get; private set; }
        public GenericListModel(IEnumerable<T> list)
        {
            Items = new ObservableCollection<T>(list);
        }

        public IEnumerator GetEnumerator() => Items.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();
    }
}
