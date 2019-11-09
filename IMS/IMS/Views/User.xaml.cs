using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        public User(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new UserViewModel(PageMode);
        }
    }
}
