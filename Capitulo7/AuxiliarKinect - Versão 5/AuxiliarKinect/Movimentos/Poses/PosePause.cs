using AuxiliarKinect.FuncoesBasicas;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos.Poses
{
    public class PosePause : Pose
    {
        public PosePause()
        {
            this.Nome = "PosePause";
            this.QuadroIdentificacao = 30;
        }

        protected override bool PosicaoValida(Microsoft.Kinect.Skeleton esqueletoUsuario)
        {
            const double ANGULO_ESPERADO = 25;
            double margemErroPosicao = 0.30;
            double margemErroAngulo = 10;
            

            Joint quadrilEsquerdo = esqueletoUsuario.Joints[JointType.HipLeft];
            Joint ombroEsquerdo = esqueletoUsuario.Joints[JointType.ShoulderLeft];
            Joint maoEsquerda = esqueletoUsuario.Joints[JointType.HandLeft];
            Joint cotoveloEsquerdo = esqueletoUsuario.Joints[JointType.ElbowLeft];

            double resultadoAngulo = Util.CalcularProdutoEscalar(quadrilEsquerdo, ombroEsquerdo, maoEsquerda);

            bool anguloCorreto = Util.CompararComMargemErro(margemErroAngulo, resultadoAngulo, ANGULO_ESPERADO);

            bool maoEsquerdaDistanciaCorreta = Util.CompararComMargemErro(margemErroPosicao, maoEsquerda.Position.Z, quadrilEsquerdo.Position.Z);
            bool maoEsquerdaAposCotovelo = maoEsquerda.Position.X < cotoveloEsquerdo.Position.X;
            bool maoEsquerdaAbaixoCotovelo = maoEsquerda.Position.Y < cotoveloEsquerdo.Position.Y;

            return anguloCorreto &&
                   maoEsquerdaDistanciaCorreta &&
                   maoEsquerdaAposCotovelo &&
                   maoEsquerdaAbaixoCotovelo;
        }
    }
}
