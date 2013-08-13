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
using ComandosVoz.Auxiliar;
using AuxiliarKinect.Movimentos.Poses;
using AuxiliarKinect.Movimentos;
using AuxiliarKinect.Movimentos.Gestos;
using AuxiliarKinect.Movimentos.Gestos.Aceno;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Speech.Recognition;
using System.IO;
using Microsoft.Speech.AudioFormat;

namespace ComandosVoz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        InicializadorKinect inicializador;
        List<IRastreador> rastreadores;
        InteractionStream fluxoInteracao;
        ConfiguracaoDesenho configuracaoMaoDireita;
        ConfiguracaoDesenho configuracaoMaoEsquerda;

        public MainWindow()
        {
            InitializeComponent();
            InicializarSeletor();
            InicializarRastreadores();
            InicializarConfiguracoesDesenho();
        }

        private void InicializarConfiguracoesDesenho()
        {
            configuracaoMaoDireita = new ConfiguracaoDesenho();
            configuracaoMaoDireita.Cor = Brushes.Red;
            configuracaoMaoDireita.Forma = FormaDesenho.Elipse;
            configuracaoMaoDireita.Tamanho = 20;

            configuracaoMaoEsquerda = new ConfiguracaoDesenho();
            configuracaoMaoEsquerda.Cor = Brushes.Blue;
            configuracaoMaoEsquerda.Forma = FormaDesenho.Retangulo;
            configuracaoMaoEsquerda.Tamanho = 20;
        }

        private void InicializarSeletor()
        {
            inicializador = new InicializadorKinect();
            inicializador.MetodoInicializadorKinect = InicializarKinect;
            inicializador.MetodoGerarGramatica = GerarGramatica;
            seletorSensorUI.KinectSensorChooser = inicializador.SeletorKinect;
        }

        private void InicializarKinect(KinectSensor kinectSensor)
        {
            kinect = kinectSensor;
            slider.Value = kinect.ElevationAngle;

            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();
            kinect.ColorStream.Enable();
            kinect.AllFramesReady += kinect_AllFramesReady;

            InicializarFluxoInteracao();
            InicializarFonteAudio();
        }

        private Grammar GerarGramatica()
        {
            Choices comandos = new Choices();
            List<string> listaFormas = GerarListaFormas();
            List<string> listaCores = GerarListaCores();
            comandos.Add("Kinect");
            comandos.Add("Cancel");

            for (int indice = 0; indice < 2; indice++)
            {
                StringBuilder sentencaBase = new StringBuilder();
                string direcaoMao = indice == 0 ? "right" : "left";

                sentencaBase.Append("Change ");
                sentencaBase.Append(direcaoMao);
                sentencaBase.Append(" hand ");

                string inicial = sentencaBase.ToString();

                for (int indiceLista = 0; indiceLista < listaCores.Count; indiceLista++)
                {
                    StringBuilder sentencaFinal = new StringBuilder(inicial);

                    sentencaFinal.Append("color to ");
                    sentencaFinal.Append(listaCores[indiceLista]);

                    comandos.Add(sentencaFinal.ToString());
                }

                for (int indiceLista = 0; indiceLista < listaFormas.Count; indiceLista++)
                {
                    StringBuilder sentencaFinal = new StringBuilder(inicial);

                    sentencaFinal.Append("shape to ");
                    sentencaFinal.Append(listaFormas[indiceLista]);

                    comandos.Add(sentencaFinal.ToString());
                }

            }


            GrammarBuilder construtor = new GrammarBuilder(comandos);
            construtor.Culture = inicializador.EngenhoReconhecimentoVoz.RecognizerInfo.Culture;

            Grammar gramatica = new Grammar(construtor);

            return gramatica;
        }

        private List<string> GerarListaCores()
        {
            List<string> cores = new List<string>();
            cores.Add("red");
            cores.Add("blue");
            cores.Add("yellow");
            cores.Add("green");
            cores.Add("pink");

            return cores;
        }

        private List<string> GerarListaFormas()
        {
            List<string> formas = new List<string>();
            formas.Add("rectangle");
            formas.Add("ellipse");

            return formas;
        }

        private void KinectSpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            RecognitionResult resultado = e.Result;
            if (resultado.Confidence > 0.5)
            {
                if (resultado.Text == "Kinect")
                    seletorSensorUI.IsListening = true;
                else if (resultado.Text == "Cancel")
                    seletorSensorUI.IsListening = false;
                else if (seletorSensorUI.IsListening)
                    InterpretarComandoConfiguracaoCor(resultado);
            }
        }

        private void InterpretarComandoConfiguracaoCor(RecognitionResult resultado)
        {
            ConfiguracaoDesenho configuracao;

            if (resultado.Text.Contains("left hand"))
                configuracao = configuracaoMaoEsquerda;
            else
                configuracao = configuracaoMaoDireita;

            if (resultado.Text.Contains("color"))
            {
                string cor = resultado.Text.Substring(resultado.Text.LastIndexOf(" ") + 1);
                SolidColorBrush novaCor = Brushes.Red;
                switch (cor)
                {
                    case "red":
                        novaCor = Brushes.Red;
                        break;
                    case "blue":
                        novaCor = Brushes.Blue;
                        break;
                    case "yellow":
                        novaCor = Brushes.Yellow;
                        break;
                    case "green":
                        novaCor = Brushes.Green;
                        break;
                    case "pink":
                        novaCor = Brushes.Pink;
                        break;
                }

                configuracao.Cor = novaCor;
            }
            else if (resultado.Text.Contains("shape"))
            {
                string forma = resultado.Text.Substring(resultado.Text.LastIndexOf(" ") + 1);
                FormaDesenho novaForma = FormaDesenho.Retangulo;
                switch (forma)
                {
                    case "rectangle":
                        novaForma = FormaDesenho.Retangulo;
                        break;
                    case "ellipse":
                        novaForma = FormaDesenho.Elipse;
                        break;
                }

                configuracao.Forma = novaForma;
            }
        }

        private void InicializarFonteAudio()
        {
            kinect.AudioSource.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;
            kinect.AudioSource.BeamAngleChanged += AudioSource_BeamAngleChanged;
            kinect.AudioSource.SoundSourceAngleChanged += AudioSource_SoundSourceAngleChanged;
            Stream fluxoAudio = kinect.AudioSource.Start();

            inicializador.InicializarReconhecimentoVoz(fluxoAudio);
            inicializador.EngenhoReconhecimentoVoz.SpeechRecognized += KinectSpeechRecognitionEngine_SpeechRecognized;
        }

        private void AudioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            AjustarPonteiroPorAngulo(e.Angle, ponteiroSoundSourceAngle);
        }

        private void AudioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            AjustarPonteiroPorAngulo(e.Angle, ponteiroBeamAngle);
        }

        private void AjustarPonteiroPorAngulo(double angulo, Rectangle ponteiro)
        {
            double calculoPosicao = (barraDirecaoAudio.ActualWidth * angulo / 100) + (barraDirecaoAudio.ActualWidth / 2) - ponteiro.ActualWidth / 2;
            double deslocamento = Math.Min(calculoPosicao, barraDirecaoAudio.ActualWidth - ponteiro.ActualWidth);
            deslocamento = Math.Max(calculoPosicao, 0);
            Canvas.SetLeft(ponteiro, deslocamento);
        }

        private void InicializarFluxoInteracao()
        {
            fluxoInteracao = new InteractionStream(kinect, canvasDesenho);
            fluxoInteracao.InteractionFrameReady += fluxoInteracao_InteractionFrameReady;
        }

        private void fluxoInteracao_InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            using (InteractionFrame quadro = e.OpenInteractionFrame())
            {
                if (quadro == null) return;

                UserInfo[] informacoesUsuarios = new UserInfo[6];
                quadro.CopyInteractionDataTo(informacoesUsuarios);
                IEnumerable<UserInfo> usuariosRastreados = informacoesUsuarios.Where(info => info.SkeletonTrackingId != 0);
                if (usuariosRastreados.Count() > 0)
                {
                    UserInfo usuarioPrincipal = usuariosRastreados.First();

                    if (usuarioPrincipal.HandPointers[0].HandEventType == InteractionHandEventType.Grip)
                        configuracaoMaoEsquerda.DesenhoAtivo = true;
                    else if (usuarioPrincipal.HandPointers[0].HandEventType == InteractionHandEventType.GripRelease)
                        configuracaoMaoEsquerda.DesenhoAtivo = false;

                    if (usuarioPrincipal.HandPointers[1].HandEventType == InteractionHandEventType.Grip)
                        configuracaoMaoDireita.DesenhoAtivo = true;
                    else if (usuarioPrincipal.HandPointers[1].HandEventType == InteractionHandEventType.GripRelease)
                        configuracaoMaoDireita.DesenhoAtivo = false;

                }
            }
        }

        private void InicializarRastreadores()
        {
            rastreadores = new List<IRastreador>();

            Rastreador<PoseT> rastreadorPoseT = new Rastreador<PoseT>();
            rastreadorPoseT.MovimentoIdentificado += PoseTIdentificada;

            Rastreador<PosePause> rastreadorPosePause = new Rastreador<PosePause>();
            rastreadorPosePause.MovimentoIdentificado += PosePauseIdentificada;
            rastreadorPosePause.MovimentoEmProgresso += PosePauseEmProgresso;

            Rastreador<Aceno> rastreadorAceno = new Rastreador<Aceno>();
            rastreadorAceno.MovimentoIdentificado += AcenoIndentificado;

            rastreadores.Add(rastreadorPoseT);
            rastreadores.Add(rastreadorPosePause);
            rastreadores.Add(rastreadorAceno);
        }


        private void AcenoIndentificado(object sender, EventArgs e)
        {
            if (kinectRegion.KinectSensor == null)
                kinectRegion.KinectSensor = kinect;
        }

        private void PosePauseEmProgresso(object sender, EventArgs e)
        {
            PosePause pose = sender as PosePause;

            Rectangle retangulo = new Rectangle();
            retangulo.Width = canvasKinect.ActualWidth;
            retangulo.Height = 20;
            retangulo.Fill = Brushes.Black;

            Rectangle poseRetangulo = new Rectangle();
            poseRetangulo.Width = canvasKinect.ActualWidth * pose.PercentualProgresso / 100;
            poseRetangulo.Height = 20;
            poseRetangulo.Fill = Brushes.BlueViolet;

            canvasKinect.Children.Add(retangulo);
            canvasKinect.Children.Add(poseRetangulo);
        }

        private void PosePauseIdentificada(object sender, EventArgs e)
        {
            btnEscalaCinza.IsChecked = !btnEscalaCinza.IsChecked;
        }

        private void PoseTIdentificada(object sender, EventArgs e)
        {
            btnEsqueletoUsuario.IsChecked = !btnEsqueletoUsuario.IsChecked;
        }

        private void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            byte[] imagem = ObterImagemSensorRGB(e.OpenColorImageFrame());

            FuncoesProfundidade(e.OpenDepthImageFrame(), imagem, 2000);

            if (imagem != null)
                canvasKinect.Background = new ImageBrush(BitmapSource.Create(kinect.ColorStream.FrameWidth, kinect.ColorStream.FrameHeight,
                                    96, 96, PixelFormats.Bgr32, null, imagem,
                                    kinect.ColorStream.FrameWidth * kinect.ColorStream.FrameBytesPerPixel));

            canvasKinect.Children.Clear();
            FuncoesEsqueletoUsuario(e.OpenSkeletonFrame());

        }

        private void FuncoesEsqueletoUsuario(SkeletonFrame quadro)
        {
            if (quadro == null) return;

            using (quadro)
            {
                Skeleton esqueletoUsuario = quadro.ObterEsqueletoUsuario();

                if (btnDesenhar.IsChecked)
                {
                    Skeleton[] esqueletos = new Skeleton[6];
                    quadro.CopySkeletonDataTo(esqueletos);
                    fluxoInteracao.ProcessSkeleton(esqueletos, kinect.AccelerometerGetCurrentReading(), quadro.Timestamp);
                    EsqueletoUsuarioAuxiliar esqueletoAuxiliar = new EsqueletoUsuarioAuxiliar(kinect);

                    if (configuracaoMaoDireita.DesenhoAtivo)
                        esqueletoAuxiliar.InteracaoDesenhar(esqueletoUsuario.Joints[JointType.HandRight], canvasDesenho, configuracaoMaoDireita);

                    if (configuracaoMaoEsquerda.DesenhoAtivo)
                        esqueletoAuxiliar.InteracaoDesenhar(esqueletoUsuario.Joints[JointType.HandLeft], canvasDesenho, configuracaoMaoEsquerda);
                }
                else
                {
                    foreach (IRastreador rastreador in rastreadores)
                        rastreador.Rastrear(esqueletoUsuario);

                    if (btnEsqueletoUsuario.IsChecked)
                        quadro.DesenharEsqueletoUsuario(kinect, canvasKinect);
                }
            }
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

        private void FuncoesProfundidade(DepthImageFrame quadro, byte[] bytesImagem, int distanciaMaxima)
        {
            if (quadro == null || bytesImagem == null) return;

            using (quadro)
            {
                DepthImagePixel[] imagemProfundidade = new DepthImagePixel[quadro.PixelDataLength];
                quadro.CopyDepthImagePixelDataTo(imagemProfundidade);

                if (btnDesenhar.IsChecked)
                    fluxoInteracao.ProcessDepth(imagemProfundidade, quadro.Timestamp);
                else if (btnEscalaCinza.IsChecked)
                    ReconhecerProfundidade(bytesImagem, distanciaMaxima, imagemProfundidade);
            }
        }

        private void ReconhecerProfundidade(byte[] bytesImagem, int distanciaMaxima, DepthImagePixel[] imagemProfundidade)
        {
            DepthImagePoint[] pontosImagemProfundidade = new DepthImagePoint[640 * 480];
            kinect.CoordinateMapper.MapColorFrameToDepthFrame(kinect.ColorStream.Format, kinect.DepthStream.Format, imagemProfundidade, pontosImagemProfundidade);

            for (int i = 0; i < pontosImagemProfundidade.Length; i++)
            {
                var point = pontosImagemProfundidade[i];
                if (point.Depth < distanciaMaxima && KinectSensor.IsKnownPoint(point))
                {
                    var pixelDataIndex = i * 4;

                    byte maiorValorCor = Math.Max(bytesImagem[pixelDataIndex], Math.Max(bytesImagem[pixelDataIndex + 1], bytesImagem[pixelDataIndex + 2]));

                    bytesImagem[pixelDataIndex] = maiorValorCor;
                    bytesImagem[pixelDataIndex + 1] = maiorValorCor;
                    bytesImagem[pixelDataIndex + 2] = maiorValorCor;
                }
            }
        }



        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            kinect.ElevationAngle = Convert.ToInt32(slider.Value);
        }

        private void btnFecharClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnDesenharClick(object sender, RoutedEventArgs e)
        {
            if (!btnDesenhar.IsChecked)
                canvasDesenho.Children.Clear();
        }

        private void btnVoltarClick(object sender, RoutedEventArgs e)
        {
            if (kinectRegion.KinectSensor != null)
                kinectRegion.KinectSensor = null;
        }
    }
}
