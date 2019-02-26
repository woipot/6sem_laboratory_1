using System.ComponentModel;
using System.Runtime.CompilerServices;
using laboratory_1.sources.mvvm;
using Microsoft.Practices.Prism.Mvvm;

namespace laboratory_1.sources
{
    class MainViewModel : BindableBase
    {
        private readonly FirstTabModel _firstModel;
        private readonly TabSecondModel _secondModel;
        private readonly TabThirdModel _thirdModel;

        public MainViewModel()
        {
            _firstModel = new FirstTabModel();
            _secondModel = new TabSecondModel();
            _thirdModel = new TabThirdModel();
        }

        public string Input32
        {
            get => _firstModel.Input32;
            set => _firstModel.Input32 = value;
        }

        public int BitNum
        {
            get => _firstModel.BitNum;
            set
            {
                _firstModel.BitNum = value;
                OnPropertyChanged("SelectedBit");
                OnPropertyChanged("Checked");
            }
        }

        public string SelectedBit
        {
            get => _firstModel.SelectedBit;
        }

        public bool Checked
        {
            get => _firstModel.Checked;
            set
            {
                _firstModel.Checked = value;
                OnPropertyChanged("Input32");  
                OnPropertyChanged("SelectedBit");
            }
        }
    }
}
