using System.ComponentModel;
using System.Runtime.CompilerServices;
using laboratory_1.sources.mvvm;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Xceed.Wpf.Toolkit;

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

            SwapCommand = new DelegateCommand(Swap);
            ZeroCommand = new DelegateCommand(Zero);
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

        public int SwapLeft
        {
            get => _firstModel.SwapLeft;
            set => _firstModel.SwapLeft = value;
        }

        public int SwapRight
        {
            get => _firstModel.SwapRight;
            set => _firstModel.SwapRight = value;
        }

        public int ToZeroNum
        {
            get => _firstModel.ToZeroNum;
            set => _firstModel.ToZeroNum = value;
        }

        public DelegateCommand SwapCommand { get; }
        public DelegateCommand ZeroCommand { get; }


        private void Swap()
        {
            var res = _firstModel.Swap();

            if (res)
            {
                OnPropertyChanged("Input32");
                OnPropertyChanged("SelectedBit");
                OnPropertyChanged("Checked");
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "Bad swap params");
            }
        }

        private void Zero()
        {
            _firstModel.ToZero();
            OnPropertyChanged("Input32");
            OnPropertyChanged("SelectedBit");
            OnPropertyChanged("Checked");
        }
    }
}
