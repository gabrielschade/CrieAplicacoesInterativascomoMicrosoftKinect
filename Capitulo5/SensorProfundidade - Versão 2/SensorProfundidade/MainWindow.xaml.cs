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
            byte[] imagem = ObterImagemSensorRGB(e.OpenColorImageFrame());
            if (chkEscalaCinza.IsChecked.HasValue && chkEscalaCinza.IsChecked.Value)
                ReconhecerDistancia(e.OpenDepthImageFrame(), imagem, 2000);
            if (imagem != null)
                imagemCamera.Source = BitmapSource.Create(kinect.ColorStream.FrameWidth, kinect.ColorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null, imagem, kinect.ColorStream.FrameBytesPerPixel * kinect.ColorStream.FrameWidth);
        }

        private byte[] ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            if (quadro == null) return null;

            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);

                return bytesImagem;
            }
        }

        private void ReconhecerDistancia(DepthImageFrame quadro, byte[] bytesImagem, int distanciaMaxima)
        {
            if (quadro == null || bytesImagem == null) return;

            using (quadro)
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
            }
        }



        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            kinect.ElevationAngle = Convert.ToInt32(slider.Value);
        }


    }
}
