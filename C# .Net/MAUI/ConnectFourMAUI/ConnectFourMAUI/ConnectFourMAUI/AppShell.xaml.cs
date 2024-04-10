using ConnectFourMAUI.ViewModel;

namespace ConnectFourMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}
