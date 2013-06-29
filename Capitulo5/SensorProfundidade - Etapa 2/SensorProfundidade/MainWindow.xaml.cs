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

namespace SensorProfundidade
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
            slider.Value = kinect.ElevationAngle;

            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();
            kinect.ColorStream.Enable();
            kinect.AllFramesReady += kinect_AllFramesReady;
        }

        private void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            imagemCamera.Source = ReconhecerDistancia(e.OpenDepthImageFrame(), ObterImagemSensorRGB(e.OpenColorImageFrame()),2000);
        }

        private byte[] ObterImagemSensorRGB(ColorImageFrame quadroAtual)
        {
            if (quadroAtual == null) return null;

            using (ColorImageFrame quadro = quadroAtual)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);

                return bytesImagem;
            }
        }

        private BitmapSource ReconhecerDistancia(DepthImageFrame quadroAtual, byte[] bytesImagem, int distanciaMaxima)
        {
            if (quadroAtual == null || bytesImagem == null) return null;

            using (DepthImageFrame quadro = quadroAtual)
            {
                DepthImagePixel[] imagemProfundidade = new DepthImagePixel[quadro.PixelDataLength];
                quadro.CopyDepthImagePixelDataTo(imagemProfundidade);

                for (int indice = 0; indice < imagemProfundidade.Length; indice++)
                {
                    if (imagemProfundidade[indice].Depth < distanciaMaxima)
                    {
                        int indiceImageCores = indice * 4;
                        byte maiorValorCor = Math.Max(bytesImagem[indiceImageCores], Math.Max(bytesImagem[indiceImageCores + 1], bytesImagem[indiceImageCores + 2]));

                        bytesImagem[indiceImageCores] = maiorValorCor;
                        bytesImagem[indiceImageCores + 1] = maiorValorCor;
                        bytesImagem[indiceImageCores + 2] = maiorValorCor;
                    }
                }
                return BitmapSource.Create(quadro.Width, quadro.Height, 96, 96, PixelFormats.Bgr32, null, bytesImagem, quadro.Width * 4);
            }
        }



        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            kinect.ElevationAngle = Convert.ToInt32(slider.Value);
        }


    }
}
