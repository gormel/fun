using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fun1.Model;

namespace Fun1.View
{
    public class FieldView : INotifyPropertyChanged
    {
        private readonly Field mField;

        public Palette Palette { get; set; } = Palette.Default;

        public FieldView(Field field)
        {
            mField = field;
        }

        public int Score => mField.Score;

        public IEnumerable<FieldOptionView> FieldOptions => mField.Options.Select(o => new FieldOptionView(this, o)); 

        public IEnumerable<SquareView> FieldElements
        {
            get
            {
                for (int i = 0; i < mField.FieldData.GetLength(0); i++)
                {
                    for (int j = 0; j < mField.FieldData.GetLength(1); j++)
                    {
                        if (mField.FieldData[i, j] == null)
                            continue;
                        var view = new SquareView(this, mField.FieldData[i, j], i, j);
                        view.OnClick += OnSquareClick;
                        yield return view;
                    }
                }
            }
        }

        private void OnSquareClick(SquareView obj)
        {
            mField.ProcessClick(obj.FieldX, obj.FieldY);

            UpdateState();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateState()
        {
            OnPropertyChanged(nameof(FieldElements));
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(FieldOptions));
        }
    }
}
