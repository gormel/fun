using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fun1.Model;

namespace Fun1.View
{
    public class FieldOptionView : INotifyPropertyChanged
    {
        private readonly FieldView mFieldView;
        private readonly FieldOption mOption;

        public FieldOptionView(FieldView fieldView, FieldOption option)
        {
            mFieldView = fieldView;
            mOption = option;
        }

        public int Cost => mOption.Cost;

        public bool Enabled => mOption.Enabled;

        public string Name => mOption.Name;

        public void Fire()
        {
            mOption.Fire();
            mFieldView.UpdateState();
            OnPropertyChanged(nameof(Enabled));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
