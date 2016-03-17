using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SendKeys {

    using _resources = Properties.Resources;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow :
        Window {

        const int WaitSeconds = 10;

        private static Properties.Settings _settings = Properties.Settings.Default;

        private BackgroundWorker _worker;

        public MainWindow() {
            InitializeComponent();
            progressBar.Maximum = WaitSeconds;
            foreach (var key in _resources._Keys.Split()) {
                var button = new Button() {
                    Content = key,
                };
                button.Click += button_FunctionKey_Click;
                panel_FunctionKeys.Children.Add(button);
            }
            _worker = new BackgroundWorker() {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            _worker.DoWork += (sender, e) => {
                var worker = sender as BackgroundWorker;
                var wait = WaitSeconds;
                while (wait > 0) {
                    if (worker.CancellationPending) {
                        return;
                    }
                    worker.ReportProgress(wait--);
                    Thread.Sleep(1000);
                }
            };
            _worker.ProgressChanged += (sender, e) => {
                progressBar.Value = e.ProgressPercentage;
            };
            _worker.RunWorkerCompleted += (sender, e) => {
                if (!e.Cancelled) {
                    System.Windows.Forms.SendKeys.SendWait(textBox_Text.Text);
                }
                button_Send.IsEnabled = true;
                button_Cancel.IsEnabled = false;
                textBox_Text.IsEnabled = true;
                progressBar.Value = 0;
            };
            button_LoadMacro0.ToolTip = _settings.Macro0;
            button_LoadMacro1.ToolTip = _settings.Macro1;
            button_LoadMacro2.ToolTip = _settings.Macro2;
            button_LoadMacro3.ToolTip = _settings.Macro3;
            button_LoadMacro4.ToolTip = _settings.Macro4;
            button_LoadMacro5.ToolTip = _settings.Macro5;
            button_LoadMacro6.ToolTip = _settings.Macro6;
            button_LoadMacro7.ToolTip = _settings.Macro7;
            button_LoadMacro8.ToolTip = _settings.Macro8;
            button_LoadMacro9.ToolTip = _settings.Macro9;
            textBox_Text.Text = _settings.Macro0;
        }

        private void button_Send_Click(object sender, RoutedEventArgs e) {
            button_Send.IsEnabled = false;
            button_Cancel.IsEnabled = true;
            textBox_Text.IsEnabled = false;
            _worker.RunWorkerAsync();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e) {
            if (_worker == null ||
                !_worker.IsBusy) {
                return;
            }
            _worker.CancelAsync();
        }

        private void button_FunctionKey_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var value = button.Content as string;
            switch (value) {
                case "SHIFT":
                    value = "+";
                    break;
                case "CTRL":
                    value = "^";
                    break;
                case "ALT":
                    value = "%";
                    break;
                default:
                    value = "{" + value + "}";
                    break;
            }
            textBox_Text.Text += value;
            textBox_Text.Focus();
            textBox_Text.CaretIndex = int.MaxValue;
        }

        private void button_LoadMacro_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            textBox_Text.Text = button.ToolTip as string;
        }

        private void button_SaveMacro_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            switch (button.Name) {
                case "button_SaveMacro0":
                    button_LoadMacro0.ToolTip = _settings.Macro0 = textBox_Text.Text;
                    break;
                case "button_SaveMacro1":
                    button_LoadMacro1.ToolTip = _settings.Macro1 = textBox_Text.Text;
                    break;
                case "button_SaveMacro2":
                    button_LoadMacro2.ToolTip = _settings.Macro2 = textBox_Text.Text;
                    break;
                case "button_SaveMacro3":
                    button_LoadMacro3.ToolTip = _settings.Macro3 = textBox_Text.Text;
                    break;
                case "button_SaveMacro4":
                    button_LoadMacro4.ToolTip = _settings.Macro4 = textBox_Text.Text;
                    break;
                case "button_SaveMacro5":
                    button_LoadMacro5.ToolTip = _settings.Macro5 = textBox_Text.Text;
                    break;
                case "button_SaveMacro6":
                    button_LoadMacro6.ToolTip = _settings.Macro6 = textBox_Text.Text;
                    break;
                case "button_SaveMacro7":
                    button_LoadMacro7.ToolTip = _settings.Macro7 = textBox_Text.Text;
                    break;
                case "button_SaveMacro8":
                    button_LoadMacro8.ToolTip = _settings.Macro8 = textBox_Text.Text;
                    break;
                case "button_SaveMacro9":
                    button_LoadMacro9.ToolTip = _settings.Macro9 = textBox_Text.Text;
                    break;
            }
            _settings.Save();
        }
    }
}
