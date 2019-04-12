using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using laboratory_1.sources.mvvm;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using NSubstitute;
using Xceed.Wpf.Toolkit;

namespace laboratory_1.sources
{
    class MainViewModel :BindableBase
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

            MyEncryptionAction = new DelegateCommand(MyEncryption);
            MyDecryptionAction = new DelegateCommand(MyDecription);

            RC4StartAction = new DelegateCommand(RC4Start);
            VernamStartAction = new DelegateCommand(VernamStart);
            DESEncodeAction = new DelegateCommand(DESEncode);
            DESDecodeAction = new DelegateCommand(DESDecode);
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

        public string Number
        {
            get => _firstModel.Number;
            set
            {
                _firstModel.Number = value;
                OnPropertyChanged("ResultNum");
            }
            
        }

        public int Left
        {
            get => _firstModel.Left;
            set
            {
                _firstModel.Left = value;
                OnPropertyChanged("ResultNum");
            } 
        }

        public int Right
        {
            get => _firstModel.Right;
            set
            {
                _firstModel.Right = value;
                OnPropertyChanged("ResultNum");
            }
            
        }

        public string ResultNum => _firstModel.ResultNum;

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
                OnPropertyChanged("LeftOffsetResult");
                OnPropertyChanged("RightOffsetResult");
            }
        }

        public string NumP => _secondModel.NumP;

        public string XorResult => _secondModel.XorResult;

        #endregion


        #region Tab 2 part 7

        public int Offset
        {
            get => _secondModel.Offset;
            set
            {
                _secondModel.Offset = value;
                OnPropertyChanged("LeftOffsetResult");
                OnPropertyChanged("RightOffsetResult");
            }
        }

        public string LeftOffsetResult => _secondModel.LeftOffset;


        public string RightOffsetResult => _secondModel.RightOffset;

        #endregion


        #region Tab 2 part 8

        public string Input8
        {
            get => _secondModel.Input8;
            set
            {
                _secondModel.Input8 = value; 
                OnPropertyChanged("PermutResult");
                OnPropertyChanged("PermutInput");
            }
        }

        public string PermutInput
        {
            get => _secondModel.PermutInput;
            set
            {
                _secondModel.PermutInput = value; 
                OnPropertyChanged("PermutResult");
            }
        }

        public string PermutResult => _secondModel.PermutResult;

        #endregion


        public  bool IsProcess { get; set; }

        public bool IsButtonsEnabled => !IsProcess;

        #region Tab 3 part 9

        public DelegateCommand MyEncryptionAction { get; }

        public DelegateCommand MyDecryptionAction { get; }

        private void MyEncryption()
        {
            Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(() => IsProcess = true);
                var myDialog = new OpenFileDialog();
                myDialog.CheckFileExists = true;
                if (myDialog.ShowDialog() == true)
                {
                    _thirdModel.MyEncrypt(myDialog.FileName);
                }
                App.Current.Dispatcher.Invoke(() => IsProcess = false);
            });

        }
 
        private void MyDecription()
        {
            Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(() => IsProcess = true);

                var myDialog = new OpenFileDialog();
                myDialog.CheckFileExists = true;
                if (myDialog.ShowDialog() == true)
                {
                    _thirdModel.MyDecrypt(myDialog.FileName);
                }
                App.Current.Dispatcher.Invoke(() => IsProcess = false);

            });
        }

        #endregion


        #region Tab 3 part 10
        public DelegateCommand VernamStartAction { get; }

        public string VernamKey
        {
            get => _thirdModel.VernamKey;
            set => _thirdModel.VernamKey = value;
        }

        private void VernamStart()
        {
            Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(() => IsProcess = true);

                if (VernamKey.Length > 0)
                {
                    var myDialog = new OpenFileDialog();
                    myDialog.CheckFileExists = true;
                    if (myDialog.ShowDialog() == true)
                    {
                        _thirdModel.StartVernam(myDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid key");
                }
                App.Current.Dispatcher.Invoke(() => IsProcess = false);

            });
        }

        #endregion


        #region Tab 3 part 11
        public DelegateCommand DESEncodeAction { get; }
        public DelegateCommand DESDecodeAction { get; }

        public string DESKey
        {
            get => _thirdModel.DESKey;
            set => _thirdModel.DESKey = value;
        }

        public string DESIv
        {
            get => _thirdModel.DESIV;
            set => _thirdModel.DESIV = value;
        }

        public bool IvIsEnabled
        {
            get => !ECBMode;
        }

        public bool ECBMode
        {
            get => _thirdModel.ECBMode;
            set => _thirdModel.ECBMode = value;
        }
        public bool CBCMode
        {
            get => _thirdModel.CBCMode;
            set => _thirdModel.CBCMode = value;
        }
        public bool CFBMode
        {
            get => _thirdModel.CFBMode;
            set => _thirdModel.CFBMode = value;
        }
        public bool OFBMode
        {
            get => _thirdModel.OFBMode;
            set => _thirdModel.OFBMode = value;
        }

        private void DESEncode()
        {
            BindingTimeTest = "";
            if (DESKey.Length == 16)
            {
                var myDialog = new OpenFileDialog();
                myDialog.CheckFileExists = true;
                if (myDialog.ShowDialog() == true)
                {
                    Task.Factory.StartNew(() =>
                    {
                        App.Current.Dispatcher.Invoke(() => IsProcess = true);

                        try
                        {
                            var myStopwatch = new Stopwatch();
                            myStopwatch.Start();
                            _thirdModel.DESEncode(myDialog.FileName);
                            myStopwatch.Stop();


                            App.Current.Dispatcher.Invoke(() =>
                                BindingTimeTest += $" My : {myStopwatch.ElapsedMilliseconds};");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Invalid key");
                        }

                        App.Current.Dispatcher.Invoke(() => IsProcess = false);

                    });

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    DesEncrypt(myDialog.FileName, myDialog.FileName + "satndard", "12345678");
                    stopwatch.Stop();
                 BindingTimeTest += $" Original : { stopwatch.ElapsedMilliseconds}";
                }
            }
            else
            {
                MessageBox.Show("Invalid key");
            }

        }

        public void DesEncrypt(string pToEncrypt, string toFile, string sKey)
        {
            pToEncrypt = File.ReadAllText(pToEncrypt);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            using (var writer = new BinaryWriter(File.Open(toFile, FileMode.OpenOrCreate)))
            {
                foreach (byte b in ms.ToArray())
                {
                    writer.Write(b);
                }
            }
        }

        private void DESDecode()
        {
            if (DESKey.Length == 16)
            {
                Task.Factory.StartNew(() =>
                {
                    App.Current.Dispatcher.Invoke(() => IsProcess = true);

                    var myDialog = new OpenFileDialog();
                    myDialog.CheckFileExists = true;
                    if (myDialog.ShowDialog() == true)
                    {
                        try
                        {
                            _thirdModel.DESEncode(myDialog.FileName, true);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Invalid key");
                        }

                    }
                    App.Current.Dispatcher.Invoke(() => IsProcess = false);

                });
            }
            else
            {
                MessageBox.Show("Invalid key");
            }
        }


        public string BindingTimeTest { get; private set; }
        #endregion


        #region Tab 3 part 12

        public DelegateCommand RC4StartAction { get; }

        public string RC4Key
        {
            get => _thirdModel.RC4Key;
            set => _thirdModel.RC4Key = value;
        }

        

        private void RC4Start()
        {
            if (RC4Key.Length > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    var myDialog = new OpenFileDialog();
                    myDialog.CheckFileExists = true;
                    if (myDialog.ShowDialog() == true)
                    {
                        _thirdModel.RC4(myDialog.FileName);
                    }
                });
            }
            else
            {
                MessageBox.Show("Invalid key");
            }
        }

        #endregion
    }
}
