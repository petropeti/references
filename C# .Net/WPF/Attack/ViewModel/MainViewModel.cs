using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Threading;
using System.Numerics;
using Attack.Model;

namespace Attack.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Private fields

        private AttackModel _model;

        //Public properties
        public ObservableCollection<AttackField> Fields { get; set; }
        public int CurrentTableSize { get { return _model.TableSize; } }

        //Constructors

        public MainViewModel(AttackModel model)
		{
            _model = model;

            //Model event handling
            _model.GameStarted += new EventHandler(Model_GameStarted);
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);
            _model.NextPieceChanged += new EventHandler<FieldChangedEventArgs>(Model_NextPieceChanged);
            _model.TableChanged += new EventHandler(Model_TableChanged);

            //Fields
            Fields = new ObservableCollection<AttackField>();
        }

        //Private methods

        private void TableInit()
        {
            Fields.Clear();

            for (Int32 x = 0; x < CurrentTableSize; x++)
            {
                for (Int32 y = 0; y < CurrentTableSize; y++)
                {
                    Fields.Add(new AttackField
                    {
                        X = x,
                        Y = y,
                        FieldChangeCommand = new DelegateCommand(param =>
                        {
                            if (param is AttackField field)
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
        private void Refresh()
        {
            for (Int32 x = 0; x < CurrentTableSize; x++)
            {
                for (Int32 y = 0; y < CurrentTableSize; y++)
                {
                    Fields.First(field => field.X == x && field.Y == y).PieceNumber = PieceToString(_model[x, y].Piece);
                    if (_model.IsAvailable(x, y))
                    {
                        Fields.First(field => field.X == x && field.Y == y).Color = 3;
                        Fields.First(field => field.X == x && field.Y == y).IsEnabled = true;
                    }
                    else
                    {
                        Fields.First(field => field.X == x && field.Y == y).Color = PlayerToColor(_model[x, y].Player);
                        Fields.First(field => field.X == x && field.Y == y).IsEnabled = true;
                    }
                }
            }
        }
        private static String PieceToString(int piece)
        {
            if (piece == 0)
                return "";
            return piece.ToString();
        }
        private static Int32 PlayerToColor(Player player)
        {
            switch (player)
            {
                case Player.Player1:
                    return 1;
                case Player.Player2:
                    return 2;
                default:
                    return 0;
            }
        }

        //Model event handlers

        private void Model_GameStarted(object? sender, EventArgs e)
        {
            TableInit();
            Refresh();
        }
        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            Fields.First(field => field.X == e.X && field.Y == e.Y).PieceNumber = PieceToString(_model[e.X, e.Y].Piece);
            Fields.First(field => field.X == e.X && field.Y == e.Y).Color = PlayerToColor(_model[e.X, e.Y].Player);
        }
        private void Model_NextPieceChanged(object? sender, FieldChangedEventArgs e)
        {
            switch (e.Player)
            {
                case Player.Player1:
                    Fields.First(field => field.X == e.X && field.Y == e.Y).Color = 11;
                    break;
                case Player.Player2:
                    Fields.First(field => field.X == e.X && field.Y == e.Y).Color = 21;
                    break;
            }
        }
        private void Model_TableChanged(object? sender, EventArgs e)
        {
            Refresh();
        }

        




    }
}
