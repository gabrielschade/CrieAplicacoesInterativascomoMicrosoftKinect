using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EsqueletoUsuario.Auxiliar
{
    public class EsqueletoUsuarioAuxiliar
    {
        private KinectSensor kinect;

        public EsqueletoUsuarioAuxiliar(KinectSensor kinect)
        {
            this.kinect = kinect;
        }

        public void DesenharArticulacao(Joint articulacao, Canvas canvasParaDesenhar)
        {
            int diametroArticulacao = articulacao.JointType == JointType.Head ? 50 : 10;
            int larguraDesenho = 4;
            Brush corDesenho = Brushes.Red;
            Ellipse objetoArticulacao = CriarComponenteVisualArticulacao(diametroArticulacao, larguraDesenho, corDesenho);

            ColorImagePoint posicaoArticulacao = ConverterCoordenadasArticulacao(articulacao, canvasParaDesenhar.ActualWidth, canvasParaDesenhar.ActualHeight);

            double deslocamentoHorizontal = posicaoArticulacao.X - objetoArticulacao.Width / 2;
            double deslocamentoVertical = (posicaoArticulacao.Y - objetoArticulacao.Height / 2);

            if (deslocamentoVertical >= 0 && deslocamentoVertical < canvasParaDesenhar.ActualHeight
                && deslocamentoHorizontal >= 0 && deslocamentoHorizontal < canvasParaDesenhar.ActualWidth)
            {
                Canvas.SetLeft(objetoArticulacao, deslocamentoHorizontal);
                Canvas.SetTop(objetoArticulacao, deslocamentoVertical);
                Canvas.SetZIndex(objetoArticulacao, 100);

                canvasParaDesenhar.Children.Add(objetoArticulacao);
            }
            
        }

        public void DesenharOsso(Joint articulacaoOrigem, Joint articulacaoDestino, Canvas canvasParaDesenhar)
        {
            int larguraDesenho = 4;
            Brush corDesenho = Brushes.Green;

            ColorImagePoint posicaoArticulacaoOrigem = 
                ConverterCoordenadasArticulacao(articulacaoOrigem, canvasParaDesenhar.ActualWidth, canvasParaDesenhar.ActualHeight);

            ColorImagePoint posicaoArticulacaoDestino =
                ConverterCoordenadasArticulacao(articulacaoDestino, canvasParaDesenhar.ActualWidth, canvasParaDesenhar.ActualHeight);

            Line objetoOsso = 
                CriarComponenteVisualOsso(larguraDesenho, corDesenho, 
                posicaoArticulacaoOrigem.X, posicaoArticulacaoOrigem.Y, 
                posicaoArticulacaoDestino.X, posicaoArticulacaoDestino.Y);


            if (Math.Max(objetoOsso.X1, objetoOsso.X2) < canvasParaDesenhar.ActualWidth && 
                Math.Min(objetoOsso.X1, objetoOsso.X2) > 0 &&
                Math.Max(objetoOsso.Y1, objetoOsso.Y2) < canvasParaDesenhar.ActualHeight && 
                Math.Min(objetoOsso.Y1, objetoOsso.Y2) > 0)
                canvasParaDesenhar.Children.Add(objetoOsso);
        }

        private Ellipse CriarComponenteVisualArticulacao(int diametroArticulacao, int larguraDesenho, Brush corDesenho)
        {
            Ellipse objetoArticulacao = new Ellipse();

            objetoArticulacao.Height = diametroArticulacao;
            objetoArticulacao.Width = diametroArticulacao;
            objetoArticulacao.StrokeThickness = larguraDesenho;
            objetoArticulacao.Stroke = corDesenho;
            return objetoArticulacao;
        }

        private Line CriarComponenteVisualOsso(int larguraDesenho, Brush corDesenho, double origemX, double origemY, double destinoX, double destinoY)
        {
            Line objetoOsso = new Line();

            objetoOsso.StrokeThickness = larguraDesenho;
            objetoOsso.Stroke = corDesenho;
            objetoOsso.X1 = origemX;
            objetoOsso.X2 = destinoX;

            objetoOsso.Y1 = origemY;
            objetoOsso.Y2 = destinoY;

            return objetoOsso;
        }

        private ColorImagePoint ConverterCoordenadasArticulacao(Joint articulacao, double larguraCanvas, double alturaCanvas)
        {
            ColorImagePoint posicaoArticulacao = kinect.CoordinateMapper.MapSkeletonPointToColorPoint(articulacao.Position, kinect.ColorStream.Format);
            posicaoArticulacao.X = (int)(posicaoArticulacao.X * larguraCanvas) / kinect.ColorStream.FrameWidth;
            posicaoArticulacao.Y = (int)(posicaoArticulacao.Y * alturaCanvas) / kinect.ColorStream.FrameHeight;
            return posicaoArticulacao;
        }


    }
}
