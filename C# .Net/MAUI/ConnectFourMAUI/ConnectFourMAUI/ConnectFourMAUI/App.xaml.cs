using ConnectFourClassLib.Model;
using ConnectFourMAUI.ViewModel;
using ConnectFourClassLib.Persistence;
using Microsoft.Maui.Dispatching;
using System.Threading;

namespace ConnectFourMAUI
{
    public partial class App : Application
    {
        #region Private fields

        private IPersistence _persistance = null!;
        private ConnectFourGameModel _model = null!;
        private MainViewModel _viewModel = null!;
        private int _size = 10;
        private IDispatcherTimer _timer = null!;

        #endregion

        #region Constructor

        public App()
        {
            InitializeComponent();

            _persistance = new TextFilePersistence();

            _model = new ConnectFourGameModel(_persistance, _size);
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);
            _model.GameOver += new EventHandler(Model_GameOver);
            _model.NewGame();

            _viewModel = new MainViewModel(_model, _size);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            //_viewModel.GameExit += new EventHandler(ViewModel_GameExit);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);

            MainPage = new AppShell(_viewModel);

            _timer = Current!.Dispatcher.CreateTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        #endregion

        #region App event handlers

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion

        #region Application life-cycle methods

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            window.Created += async (s, e) =>
            {
                _model.NewGame();
                try
                {
                    await _model.LoadGameAsync(Path.Combine(FileSystem.AppDataDirectory, "SuspendedGame"));
                }
                catch { }
            };

            window.Resumed += async (s, e) =>
            {
                try
                {
                    await _model.LoadGameAsync(Path.Combine(FileSystem.AppDataDirectory, "SuspendedGame"));
                }
                catch { }
            };

            window.Stopped += async (s, e) =>
            {
                try
                {
                    await _model.SaveGameAsync(Path.Combine(FileSystem.AppDataDirectory, "SuspendedGame"));
                }
                catch { }
            };
            window.Destroying += async (s, e) =>
            {
                try
                {
                    await _model.SaveGameAsync(Path.Combine(FileSystem.AppDataDirectory, "SuspendedGame"));
                }
                catch { }
            };

            return window;
        }

        #endregion

        #region Model event handlers

        private async void Model_GameWon(object? sender, GameWonEventArgs e)
        {
            _timer.Stop();

            //A nyertes 4 mező kiszinezése
            switch (e.Direction)
            {
                case 0: //vizszintes
                    for (int i = 0; i < 4; i++)
                    {
                        _viewModel.Fields.First(field => field.X == e.StartX && field.Y == (e.StartY + i)).Color = -1;
                    }
                    break;
                case 1: //jobble
                    for (int i = 0; i < 4; i++)
                    {
                        _viewModel.Fields.First(field => field.X == (e.StartX + i) && field.Y == (e.StartY + i)).Color = -1;
                    }
                    break;
                case -1: //balle
                    for (int i = 0; i < 4; i++)
                    {
                        _viewModel.Fields.First(field => field.X == (e.StartX - i) && field.Y == (e.StartY + i)).Color = -1;
                    }
                    break;
            }
            switch (e.Player)
            {
                case Player.PlayerO:
                    await MainPage!.DisplayAlert("Játék vége!", "A kör játékos győzött!", "OK");
                    break;
                case Player.PlayerX:
                    await MainPage!.DisplayAlert("Játék vége!", "A kereszt játékos győzött!", "OK");
                    break;
            }
            _model.NewGame();
            _timer.Start();
        }
        private async void Model_GameOver(object? sender, EventArgs e)
        {
            _timer.Stop();

            await MainPage!.DisplayAlert("Döntetlen játék!", "Játék vége!", "OK");
            _model.NewGame();
            _timer.Start();
        }

        #endregion

        #region ViewModel event handlers

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
            _timer.Start();
            _viewModel.OnPauseButtonChanged(_timer.IsRunning);
        }
        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            try
            {
                await _model.LoadGameAsync(Path.Combine(FileSystem.AppDataDirectory, "LastGame"));
                _timer.Start();
                _viewModel.OnPauseButtonChanged(_timer.IsRunning);
            }
            catch (Exception ex)
            {
                await MainPage!.DisplayAlert("Error", ex.Message, "OK");
                _model.NewGame();
                _timer.Start();
                _viewModel.OnPauseButtonChanged(_timer.IsRunning);
            }
        }
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            try
            {
                await _model.SaveGameAsync(Path.Combine(FileSystem.AppDataDirectory, "LastGame"));
            }
            catch (Exception ex)
            {
                await MainPage!.DisplayAlert("Error", ex.Message, "OK");
                _model.NewGame();
                _timer.Start();
                _viewModel.OnPauseButtonChanged(_timer.IsRunning);
            }
        }
        /*
        private void ViewModel_GameExit(object? sender, EventArgs e)
        {
            Current!.Quit();
        }
        */
        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
                _timer.Stop();
            else
                _timer.Start();
            _viewModel.OnPauseButtonChanged(_timer.IsRunning);
        }

        #endregion
    }
}
