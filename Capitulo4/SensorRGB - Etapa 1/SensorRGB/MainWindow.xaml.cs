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

namespace SensorRGB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;

        public MainWindow()
        {
            InitializeComponent();
            InicializarKinect();
        }

        private void InicializarKinect()
        {
            kinect = InicializadorKinect.InicializarPrimeiroSensor(10);
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
        }
            
        private void Button_BaterFoto(object sender, RoutedEventArgs e)
        {
            imagemCamera.Source = ObterImagemSensorRGB(kinect.ColorStream.OpenNextFrame(0));
        }

        private BitmapSource ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);

                return BitmapSource.Create(quadro.Width, quadro.Height,960 , 960, PixelFormats.Bgr32, null, bytesImagem, quadro.Width * quadro.BytesPerPixel);
            }
        }

    }
}
