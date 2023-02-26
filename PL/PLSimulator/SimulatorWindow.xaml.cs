using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Simulator;

namespace PL.PLSimulator
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        private bool ableToClose =  false;
        bool t=false;   
        BlApi.IBl bl;
        string timerText { get; set; }
        public string NextStatus { get; set; } = "";
        public string PreviousStatus { get; set; } = "";
        BackgroundWorker backgroundWorker;
        Tuple<BO.Order, int, string, string,string> orderAndTime;
        private Stopwatch stopWatch;
        private bool IfTimerRunning;
        DispatcherTimer _timer;
        TimeSpan _time;
        public SimulatorWindow(BlApi.IBl Bl)
        {
            InitializeComponent();
            bl = Bl;
            TimerStart();
        }

        void countDownTimer(int sec)
        {
            _time = TimeSpan.FromSeconds(sec);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (t is true)
                    _timer.Stop();
                t = true;
                tbTime.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
       
        void TimerStart()
        {
            stopWatch = new Stopwatch();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += TimerDoWork;
            backgroundWorker.ProgressChanged += TimerProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            stopWatch.Restart();
            IfTimerRunning = true;
            backgroundWorker.RunWorkerAsync();
        }
        void TimerDoWork(object sender, DoWorkEventArgs e)
        {
            Simulator.Simulator.ProgressChange += ChooseOrder;
            Simulator.Simulator.StopSimulator += Stop;
            Simulator.Simulator.run();
            while (IfTimerRunning)
            {
                backgroundWorker.ReportProgress(1);
                Thread.Sleep(1000);
            }
        }
        private void ChooseOrder(object sender, EventArgs e)
        {
            if (!(e is Details))
                return;

            Details details = e as Details;
            PreviousStatus = (details.order.ShipDate == null) ? BO.Enums.EStatus.Done.ToString() : BO.Enums.EStatus.Sent.ToString();
            NextStatus = (details.order.ShipDate == null) ? BO.Enums.EStatus.Sent.ToString() : BO.Enums.EStatus.Provided.ToString();

            orderAndTime = new Tuple<BO.Order, int, string, string,string>(details.order, details.seconds / 1000, PreviousStatus, NextStatus, timerText);
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(ChooseOrder, sender, e);
            }
            else
            {
                DataContext = orderAndTime;
                countDownTimer(details.seconds / 1000);

            }
        }
        void TimerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            timerText = stopWatch.Elapsed.ToString();
            timerText = timerText.Substring(0, 8);
            SimulatorTXTB.Text = timerText;
        }

        private void StopSimulatorBTN_Click(object sender, RoutedEventArgs e)
        {
            if (IfTimerRunning)
            {
                stopWatch.Stop();
                IfTimerRunning = false;
            }
            Simulator.Simulator.stoping();
            this.Close();
        }
        public void Stop(object sender, EventArgs e)
        {
            Simulator.Simulator.ProgressChange -= ChooseOrder;
            Simulator.Simulator.StopSimulator -= Stop;
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(Stop, sender, e);
            }
            else
            {
                ableToClose = true;
                MessageBox.Show("complete updating");
                this.Close();
                ableToClose = false;

            }
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!ableToClose)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}



