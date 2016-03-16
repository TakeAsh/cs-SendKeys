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
    }
}
