using System.Configuration;
using System.Data;
using System.Windows;
using Attack.View;
using Attack.Model;
using Attack.ViewModel;
using Microsoft.Win32;
using System.Windows.Threading;

namespace Attack
{
    public partial class App : Application
    {
        //Private fields

        private AttackModel _model = null!;
        private MainViewModel _viewModel = null!;
        private MainWindow _window = null!;

        //Constructor

        public App()
        {
            Startup += new StartupEventHandler(AppStartUp);
        }

        //App event handlers

        private void AppStartUp(object sender, StartupEventArgs e)
        {
            _model = new AttackModel();
            _model.GameWon += new EventHandler<Player>(Model_GameWon);
            _viewModel = new MainViewModel(_model);
            _model.NewGame();

            _window = new MainWindow();
            _window.DataContext = _viewModel;
            _window.Show();

        }
        //Model event handlers

        private void Model_GameWon(object? sender, Player p)
        {
            MessageBox.Show("Congratulations!\n" + p.ToString() + " won the game!", "Game Over!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            _model.NewGame();
        }
    }

}
