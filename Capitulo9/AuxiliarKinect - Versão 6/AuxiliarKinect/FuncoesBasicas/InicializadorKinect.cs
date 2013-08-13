using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.FuncoesBasicas
{
    public class InicializadorKinect
    {
        public SpeechRecognitionEngine EngenhoReconhecimentoVoz
        { get; private set; }

        public KinectSensorChooser SeletorKinect
        { get; private set; }

        public Action<KinectSensor> MetodoInicializadorKinect
        { get; set; }

        public Func<Grammar> MetodoGerarGramatica
        { get; set; }

        public InicializadorKinect()
        {
            SeletorKinect = new KinectSensorChooser();
            SeletorKinect.KinectChanged += SeletorKinect_KinectChanged;
            SeletorKinect.Start();
        }

        public void InicializarReconhecimentoVoz(Stream fluxoAudio)
        {
            Func<RecognizerInfo, bool> encontrarIdioma = reconhecedor =>
            {
                string value;
                reconhecedor.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase)
                && "en-US".Equals(reconhecedor.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };

            RecognizerInfo recognizerInfo = SpeechRecognitionEngine.InstalledRecognizers().Where(encontrarIdioma).FirstOrDefault();

            EngenhoReconhecimentoVoz = new SpeechRecognitionEngine(recognizerInfo.Id);
            EngenhoReconhecimentoVoz.LoadGrammar(MetodoGerarGramatica());
            EngenhoReconhecimentoVoz.SetInputToAudioStream(fluxoAudio, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            EngenhoReconhecimentoVoz.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SeletorKinect_KinectChanged(object sender, KinectChangedEventArgs kinectArgs)
        {
            if (kinectArgs.OldSensor != null)
            {
                try
                {
                    if (kinectArgs.OldSensor.DepthStream.IsEnabled)
                        kinectArgs.OldSensor.DepthStream.Disable();

                    if (kinectArgs.OldSensor.SkeletonStream.IsEnabled)
                        kinectArgs.OldSensor.SkeletonStream.Disable();

                    if (kinectArgs.OldSensor.ColorStream.IsEnabled)
                        kinectArgs.OldSensor.ColorStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // Captura exceção caso o KinectSensor entre em um estado inválido durante a desabilitação de um fluxo.
                }
            }

            if (kinectArgs.NewSensor != null)
            {
                if (MetodoInicializadorKinect != null)
                    MetodoInicializadorKinect(SeletorKinect.Kinect);
            }

        }
    }
}
