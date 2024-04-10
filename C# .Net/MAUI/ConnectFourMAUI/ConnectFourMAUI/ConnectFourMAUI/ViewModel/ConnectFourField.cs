using ConnectFourClassLib.Persistence;
using System;
using System.Drawing;
using System.Numerics;
using System.Xml;

namespace ConnectFourMAUI.ViewModel
{
    public class ConnectFourField : ViewModelBase
    {
        private String _player = String.Empty;
        private Int32 _color = 0;
        private Boolean _isEnabled = true;
        private Int32 _fontSize = MainViewModel._bigFont;
        private Int32 _length = MainViewModel._bigLength;

        public String Player
        {
            get { return _player; }
            set
            {
                if (_player != value)
                {
                    _player = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public Int32 FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged();
                }
            }
        }
        public Int32 Length
        {
            get { return _length; }
            set
            {
                if (_length != value)
                {
                    _length = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public DelegateCommand? FieldChangeCommand { get; set; }
    }
}
