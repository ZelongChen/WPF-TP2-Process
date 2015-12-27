using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WPF_TP2
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<int> ballons { get; private set; }
        public ObservableCollection<int> premiers { get; private set; }
        private List<int> allProcess = new List<int>();
        private string ballonFilePath;
        private string premierFilePath;

        public MainWindow()
        {
            InitializeComponent();
            ballons = new ObservableCollection<int>();
            premiers = new ObservableCollection<int>();
            string currentProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ballonFilePath = currentProjectPath + @"\Ballon.exe";
            premierFilePath = currentProjectPath + @"\premier.exe";
            DataContext = this;
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        private void StartBallon_Click(object sender, RoutedEventArgs e)
        {
            if (ballons.Count < 5)
            {
                Process process = new Process();
                process.Exited += new EventHandler(Process_Exited);
                process.EnableRaisingEvents = true;
                process.StartInfo.FileName = ballonFilePath;
                process.StartInfo.Verb = "OPEN";
                process.Start();

                int id = process.Id;
                ballons.Add(id);
                allProcess.Add(id);
            }
            else
            {
                MessageBox.Show("You have already created 5 ballon process. You cannot creat more.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void StartPremier_Click(object sender, RoutedEventArgs e)
        {
            if (premiers.Count < 5)
            {

                Process process = new Process();
                process.Exited += new EventHandler(Process_Exited);
                process.EnableRaisingEvents = true;
                process.StartInfo.FileName =premierFilePath;
                process.StartInfo.Verb = "OPEN";
                process.Start();

                int id = process.Id;
                premiers.Add(id);
                allProcess.Add(id);
            }
            else
            {
                MessageBox.Show("You have already created 5 premier process. You cannot creat more.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ShowHideBallonPID_Click(object sender, RoutedEventArgs e)
        {
            if (this.ShowHideBallonPID.IsChecked)
            {
                this.BallonPIDListView.Visibility = Visibility.Visible;
            }
            else
            {
                this.BallonPIDListView.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowHidePremierPID_Click(object sender, RoutedEventArgs e)
        {
            if (this.ShowHidePremierPID.IsChecked)
            {
                this.PremierPIDListView.Visibility = Visibility.Visible;
            }
            else
            {
                this.PremierPIDListView.Visibility = Visibility.Collapsed;
            }
        }

        private void LastOne_Click(object sender, RoutedEventArgs e)
        {
            if (allProcess.Count > 0)
            {
                int id = allProcess[allProcess.Count - 1];
                if (ballons.Contains(id))
                {
                    removeLastBallon();
                }
                else
                {
                    removeLastPremier();
                }
            }
            else
            {
                MessageBox.Show("There is no ballon or premier process running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LastBallon_Click(object sender, RoutedEventArgs e)
        {
            if (ballons.Count > 0)
            {
                removeLastBallon();
            }
            else
            {
                MessageBox.Show("There is no ballon process running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LastPremier_Click(object sender, RoutedEventArgs e)
        {
            if (premiers.Count > 0)
            {
                removeLastPremier();
            }
            else
            {
                MessageBox.Show("There is no premier process running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            if (premiers.Count > 0 || ballons.Count > 0)
            {
                removeAllProcess();
            }
            else
            {
                MessageBox.Show("There is no ballon or premier process running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void removeAllProcess()
        {
            while (ballons.Count > 0)
            {
                removeLastBallon();
            }
            while (premiers.Count > 0)
            {
                removeLastPremier();
            }
            allProcess.Clear();

        }

        private void removeLastBallon()
        {
            int id = ballons[ballons.Count - 1];
            Process.GetProcessById(id).Kill();
            ballons.RemoveAt(ballons.Count - 1);
            allProcess.Remove(id);
        }

        private void removeLastPremier()
        {
            int id = premiers[premiers.Count - 1];
            Process.GetProcessById(id).Kill();
            premiers.RemoveAt(premiers.Count - 1);
            allProcess.Remove(id);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            removeAllProcess();
            Application.Current.Shutdown();
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            removeAllProcess();
            Application.Current.Shutdown();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            int id = ((Process)sender).Id;
            ((Process)sender).Close();

            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate ()
                {
                    ballons.Remove(id);
                    premiers.Remove(id);
                    allProcess.Remove(id);
                });
        }

    }
}
