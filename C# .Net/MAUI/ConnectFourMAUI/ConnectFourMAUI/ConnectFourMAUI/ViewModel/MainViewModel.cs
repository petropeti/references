using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using ConnectFourClassLib.Model;
using ConnectFourClassLib.Persistence;
using System.Numerics;

namespace ConnectFourMAUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private fields

        private ConnectFourGameModel _model;
        private int _size;

        #endregion

        #region Constants

        public const int _smallFont=6;
        public const int _mediumFont=10;
        public const int _bigFont=20;
        public const int _smallLength = 13;
        public const int _mediumLength = 20;
        public const int _bigLength = 40;

        #endregion

        #region Public properties

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        //public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand SizeChangeCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public ObservableCollection<ConnectFourField> Fields { get; set; }
        public String CurrentPlayer { get { return _model.CurrentPlayer.ToString() + " make a move!";  } }
        public int CurrentTableSize { get { return _size; } }
        public String TimePlayerX { get { return TimeSpan.FromSeconds(_model.GameTimeX).ToString("g"); } }
        public String TimePlayerO { get { return TimeSpan.FromSeconds(_model.GameTimeO).ToString("g"); } }
        public String PauseButtonText { get; set; }
        public RowDefinitionCollection RowDefinitions
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), CurrentTableSize).ToArray());
        }
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), CurrentTableSize).ToArray());
        }

        #endregion

        #region Constructors

        public MainViewModel(ConnectFourGameModel model, int size)
		{
            _model = model;
            _size = size;
            PauseButtonText = "Pause";

            //Model event handling
            _model.GameStarted += new EventHandler(Model_GameStarted);
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.GameAdvanced += new EventHandler(Model_GameAdvanced);
            _model.ChangeSize += new EventHandler<int>(Model_ChangeSize);
            _model.CurrentPlayerChanged += new EventHandler(Model_CurrentPlayerChanged);

            //Command handling
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            //ExitGameCommand = new DelegateCommand(param => OnGameExit());
            SizeChangeCommand = new DelegateCommand(param => OnSizeChange(param));
            PauseCommand = new DelegateCommand(param => OnPauseGame());

            //Fields
            Fields = new ObservableCollection<ConnectFourField>();
            Refresh();
        }

        #endregion

        #region Private methods

        private void Refresh()
        {
            Fields.Clear();

            Int32 length;
            Int32 font;
            switch (_size)
            {
                case 20:
                    length = _mediumLength;
                    font = _mediumFont;
                    break;
                case 30:
                    length = _smallLength;
                    font = _smallFont;
                    break;
                default:
                    length = _bigLength;
                    font = _bigFont;
                    break;
            }
            for (Int32 x = 0; x < _size; x++)
            {
                for (Int32 y = 0; y < _size; y++)
                {
                    Fields.Add(new ConnectFourField
                    {
                        Player = PlayerToField(_model[x, y]),
                        Color = PlayerToColor(_model[x, y]),
                        FontSize = font,
                        Length = length,
                        X = x,
                        Y = y,
                        FieldChangeCommand = new DelegateCommand(param =>
                        {
                            if (param is ConnectFourField field)
                            {
                                try
                                {
                                    _model.StepGame(field.X, field.Y);
                                }
                                catch { }
                            }
                        })
                    });
                }
            }
        }
        private static String PlayerToField(Player player)
        {
            switch (player)
            {
                case Player.PlayerX:
                    return "X";
                case Player.PlayerO:
                    return "O";
                default:
                    return String.Empty;
            }
        }
        private static Int32 PlayerToColor(Player player)
        {
            switch (player)
            {
                case Player.PlayerX:
                    return 1;
                case Player.PlayerO:
                    return 2;
                default:
                    return 0;
            }
        }

        #endregion

        #region Public methods

        public void OnPauseButtonChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                PauseButtonText = "Pause";
                foreach (var field in Fields)
                    field.IsEnabled = true;
            }
            else
            {
                PauseButtonText = "Start";
                foreach (var field in Fields)
                    field.IsEnabled = false;
            }
            OnPropertyChanged(nameof(PauseButtonText));
        }

        #endregion

        #region Model event handlers

        private void Model_GameStarted(object? sender, EventArgs e)
        {
            Refresh();
        }
        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            Fields.First(field => field.X == e.X && field.Y == e.Y).Player = PlayerToField(_model[e.X, e.Y]);
            Fields.First(field => field.X == e.X && field.Y == e.Y).Color = PlayerToColor(_model[e.X, e.Y]);
        }
        private void Model_GameAdvanced(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimePlayerX));
            OnPropertyChanged(nameof(TimePlayerO));
        }
        private void Model_ChangeSize(object? sender, int e)
        {
            _size = e;
            OnPropertyChanged(nameof(CurrentTableSize));
            OnPropertyChanged(nameof(RowDefinitions));
            OnPropertyChanged(nameof(ColumnDefinitions));
            Refresh();
        }
        private void Model_CurrentPlayerChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentPlayer));
        }

        #endregion

        #region Events

        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        //public event EventHandler? GameExit;
        public event EventHandler? PauseGame;

        #endregion

        #region Event triggers

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        /*
        private void OnGameExit()
        {
            GameExit?.Invoke(this, EventArgs.Empty);
        }
        */
        private void OnSizeChange(object? param)
        {
            if (param is String size)
            {
                _size = Convert.ToInt32(size);
                OnPropertyChanged(nameof(CurrentTableSize));
                OnPropertyChanged(nameof(RowDefinitions));
                OnPropertyChanged(nameof(ColumnDefinitions));
                _model.TableInit(_size);

                NewGame?.Invoke(this, EventArgs.Empty);
            }
        }
        private void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
