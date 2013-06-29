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
            kinect.ColorFrameReady += kinect_ColorFrameReady;
        }

        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            imagemCamera.Source = ObterImagemSensorRGB(e.OpenColorImageFrame());
        }

        private BitmapSource ObterImagemSensorRGB(ColorImageFrame quadroAtual)
        {
            if (quadroAtual == null) return null;

            using (ColorImageFrame quadro = quadroAtual)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);

                if (chkEscalaCinza.IsChecked.HasValue && chkEscalaCinza.IsChecked.Value)
                    for (int indice = 0; indice < bytesImagem.Length; indice += quadro.BytesPerPixel)
                    {
                        byte maiorValorCor = Math.Max(bytesImagem[indice], Math.Max(bytesImagem[indice + 1], bytesImagem[indice + 2]));

                        bytesImagem[indice] = maiorValorCor;
                        bytesImagem[indice + 1] = maiorValorCor;
                        bytesImagem[indice + 2] = maiorValorCor;
                    }

                return BitmapSource.Create(quadro.Width, quadro.Height, 960, 960, PixelFormats.Bgr32, null, bytesImagem, quadro.Width * quadro.BytesPerPixel);
            }
        }

    }
}
