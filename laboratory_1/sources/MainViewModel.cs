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

        #region Tab 1 part 1

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

        #endregion


        #region Tab 1 part 2
        public string InputN
        {
            get => _firstModel.InputN;
            set
            {
                _firstModel.InputN = value;
                OnPropertyChanged("EndsTrim");
                OnPropertyChanged("MidleTrim");
            }
        }

        public int LeftTrim
        {
            get => _firstModel.LeftTrim;
            set
            {
                _firstModel.LeftTrim = value;
                OnPropertyChanged("EndsTrim");
                OnPropertyChanged("MidleTrim");
            }

        }

        public int RightTrim
        {
            get => _firstModel.RightTrim;
            set
            {
                _firstModel.RightTrim = value;
                OnPropertyChanged("EndsTrim");
                OnPropertyChanged("MidleTrim");
            }
        }

        public string EndsTrim => _firstModel.EndsTrim;

        public string MidleTrim => _firstModel.MidleTrim;


        #endregion


        #region Tab 1 part 3



        #endregion


        #region Tab 2 part 4 
        public string InputToMaxDivider
        {
            get => _secondModel.InputToMaxDivider;
            set
            {
                _secondModel.InputToMaxDivider = value;
                OnPropertyChanged("MaxDivider");
            } 
        }

        public string MaxDivider => _secondModel.MaxDivider;
        #endregion


        #region Tab 2 part 5

        public string InputToFindLimits
        {
            get => _secondModel.InputToFindLimits;
            set
            {
                _secondModel.InputToFindLimits = value;
                OnPropertyChanged("Limits");
            }
        }

        public string Limits => _secondModel.Limits;

        #endregion


        #region Tab 2 part 6

        public string InputP
        {
            get => _secondModel.InputP;

            set
            {
                _secondModel.InputP = value;
                OnPropertyChanged("NumP");
                OnPropertyChanged("XorResult");
            }
        }

        public string NumP => _secondModel.NumP;

        public string XorResult => _secondModel.XorResult;

        #endregion
    }
}
