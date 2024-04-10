using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using GameOfLife.Model;
using System.Numerics;

namespace GameOfLife.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Private fields

        private GameModel _model;


        //Public properties

        public DelegateCommand SimulationCommand { get; private set; }
        public ObservableCollection<GameField> Fields { get; set; }
        public String SimulationText { get; set; }
        public RowDefinitionCollection RowDefinitions
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star),12).ToArray());
        }
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star),12).ToArray());
        }

        //Constructors

        public MainViewModel(GameModel model)
		{
            _model = model;
            SimulationText = "Start";

            //Model event handling
            _model.GameStarted += new EventHandler(Model_GameStarted);
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);

            //Command handling
            SimulationCommand = new DelegateCommand(param => OnSimulation());

            //Fields
            Fields = new ObservableCollection<GameField>();
            Refresh();
        }


        //Private methods

        private void Refresh()
        {
            Fields.Clear();
            for (Int32 x = 0; x < 12; x++)
            {
                for (Int32 y = 0; y < 12; y++)
                {
                    Fields.Add(new GameField
                    {
                        Color = BoolToColor(_model[x, y]),
                        X = x,
                        Y = y,
                        FieldChangeCommand = new DelegateCommand(param =>
                        {
                            if (param is GameField field)
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
        private static Int32 BoolToColor(bool isAlive)
        {
            switch (isAlive)
            {
                case true:
                    return 1;
                default:
                    return 0;
            }
        }


        //Public methods

        public void OnPauseButtonChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                SimulationText = "Pause";
                foreach (var field in Fields)
                    field.IsEnabled = false ;
            }
            else
            {
                SimulationText = "Start";
                foreach (var field in Fields)
                    field.IsEnabled = true ;
            }
            OnPropertyChanged(nameof(SimulationText));
        }


        //Model event handlers

        private void Model_GameStarted(object? sender, EventArgs e)
        {
            Refresh();
        }
        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            Fields.First(field => field.X == e.X && field.Y == e.Y).Color = BoolToColor(_model[e.X, e.Y]);
        }


        //Events

        public event EventHandler? Simulation;


        //Event triggers

        private void OnSimulation()
        {
            Simulation?.Invoke(this, EventArgs.Empty);
        }

    }
}
