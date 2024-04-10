using System;
using System.Drawing;
using System.Numerics;
using System.Xml;

namespace Attack.ViewModel
{
    public class AttackField : ViewModelBase
    {
        private String _pieceNumber="";
        private Int32 _color;
        private Boolean _isEnabled;

        public String PieceNumber
        {
            get { return _pieceNumber; }
            set
            {
                if (_pieceNumber != value)
                {
                    _pieceNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public Int32 Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
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
