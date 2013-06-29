using AuxiliarKinect.FuncoesBasicas;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Acelerometro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor kinect;

        public MainWindow()
        {
            InitializeComponent();
            kinect = InicializadorKinect.InicializarPrimeiroSensor(27);
            InicializarTimer();
        }

        private void InicializarTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            AtualizarValoresAcelerometro();
        }

        private void InicializarSensor()
        {
            kinect = KinectSensor.KinectSensors.First(sensor => sensor.Status == KinectStatus.Connected);
            kinect.Start();
            kinect.ElevationAngle = 0;
        }

        private void AtualizarValoresAcelerometro()
        {
            Vector4 resultado = kinect.AccelerometerGetCurrentReading();
            labelX.Content = Math.Round(resultado.X, 3);
            labelY.Content = Math.Round(resultado.Y, 3);
            labelZ.Content = Math.Round(resultado.Z, 3);
        }

    }
}
