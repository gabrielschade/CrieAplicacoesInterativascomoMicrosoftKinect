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
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit;

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
            InicializarSeletor();
        }

        private void InicializarSeletor()
        {
            InicializadorKinect inicializador = new InicializadorKinect();
            inicializador.MetodoInicializadorKinect = InicializarKinect;
            sensorChooserUi.KinectSensorChooser = inicializador.SeletorKinect;
        }

        private void InicializarKinect(KinectSensor kinectSensor)
        {
            kinect = kinectSensor;
            kinect.ColorStream.Enable();
            kinect.ColorFrameReady += kinect_ColorFrameReady;
        }

        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            imagemCamera.Source = ObterImagemSensorRGB(e.OpenColorImageFrame());
        }

        private BitmapSource ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            if (quadro == null) return null;

            using (quadro)
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
