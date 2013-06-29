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
using Microsoft.Kinect;
using AuxiliarKinect.FuncoesBasicas;

namespace PrimeiroContato
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool MaoDireitaAcimaCabeca
        { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InicializarSensor();
        }

        private void InicializarSensor()
        {
            KinectSensor kinect = InicializadorKinect.InicializarPrimeiroSensor(10);
            kinect.SkeletonStream.Enable();
            kinect.SkeletonFrameReady += kinect_SkeletonFrameReady;
        }

        private void ExecutarRegraMaoDireitaAcimaDaCabeca(SkeletonFrame quadroAtual)
        {
            Skeleton[] esqueletos = new Skeleton[6];
            quadroAtual.CopySkeletonDataTo(esqueletos);
            Skeleton usuario = esqueletos.FirstOrDefault(esqueleto => esqueleto.TrackingState == SkeletonTrackingState.Tracked);
            if (usuario != null)
            {
                Joint maoDireita = usuario.Joints[JointType.HandRight];
                Joint cabeca = usuario.Joints[JointType.Head];
                bool novoTesteMaoDireitaAcimaCabeca = maoDireita.Position.Y > cabeca.Position.Y;
                if (MaoDireitaAcimaCabeca != novoTesteMaoDireitaAcimaCabeca)
                {
                    MaoDireitaAcimaCabeca = novoTesteMaoDireitaAcimaCabeca;
                    if (MaoDireitaAcimaCabeca)
                    {
                        MessageBox.Show("A mão direita está acima da cabeça!");
                    }
                }
            }
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame quadroAtual = e.OpenSkeletonFrame())
            {
                if (quadroAtual != null)
                {
                    ExecutarRegraMaoDireitaAcimaDaCabeca(quadroAtual);
                }
            }
        }
    }
}
