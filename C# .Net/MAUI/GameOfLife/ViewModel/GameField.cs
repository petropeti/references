using System;
using System.Drawing;
using System.Numerics;
using System.Xml;

namespace GameOfLife.ViewModel
{
    public class GameField : ViewModelBase
    {
        private Int32 _color = 0;
        private Boolean _isEnabled = true;

        public Int32 Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public DelegateCommand? FieldChangeCommand { get; set; }
    }
}
