using System.Windows.Controls;
using AppVeyor.UI.ViewModel;

namespace AppVeyor.UI.ToolWindows
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class AppVeyorWindowContent : UserControl
    {
        public AppVeyorWindowContent()
        {
            InitializeComponent();
            DataContext = AppVeyorWindowViewModel.Instance;
            AppVeyorWindowViewModel.Instance.Initialize();
        }

        public void Connect(int connectionId, object target)
        {
            throw new System.NotImplementedException();
        }
    }
}