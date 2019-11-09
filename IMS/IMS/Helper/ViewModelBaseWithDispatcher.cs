using System.ComponentModel;
using System.Windows.Threading;

namespace IMS.Helper
{
    public class ViewModelBaseWithDispatcher : DispatcherObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(object sender, string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}