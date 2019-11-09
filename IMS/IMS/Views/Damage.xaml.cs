using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Damage.xaml
    /// </summary>
    public partial class Damage : UserControl
    {
        public Damage(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new DamageViewModel(PageMode);
        }
    }
}
