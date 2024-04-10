using GameOfLife.Model;
using GameOfLife.ViewModel;
using GameOfLife.View;
using Microsoft.Maui.Dispatching;
using System.Threading;

namespace GameOfLife
{
    public partial class App : Application
    {
        //Private fields

        private GameModel _model = null!;
        private MainViewModel _viewModel = null!;
        private IDispatcherTimer _timer = null!;


        //Constructor

        public App()
        {
            InitializeComponent();

            _model = new GameModel();
            _model.NewGame();

            _viewModel = new MainViewModel(_model);
            _viewModel.Simulation += new EventHandler(ViewModel_Simulation);

            BindingContext = _viewModel;
            MainPage = new AppShell();

            _timer = Current!.Dispatcher.CreateTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(Timer_Tick);
        }


        //App event handlers

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        //ViewModel event handlers
        
        private void ViewModel_Simulation(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
                _timer.Stop();
            else
                _timer.Start();
            _viewModel.OnPauseButtonChanged(_timer.IsRunning);
        }

    }
}