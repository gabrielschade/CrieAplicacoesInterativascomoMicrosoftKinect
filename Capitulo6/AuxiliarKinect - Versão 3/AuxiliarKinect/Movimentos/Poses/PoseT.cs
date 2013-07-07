using AuxiliarKinect.FuncoesBasicas;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos.Poses
{
    public class PoseT : Pose
    {
        public PoseT()
        {
            this.Nome = "PoseT";
            this.QuadroIdentificacao = 10;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            Joint centroOmbros = esqueletoUsuario.Joints[JointType.ShoulderCenter];
            Joint maoDireita = esqueletoUsuario.Joints[JointType.HandRight];
            Joint cotoveloDireito = esqueletoUsuario.Joints[JointType.ElbowRight];
            Joint maoEsquerda = esqueletoUsuario.Joints[JointType.HandLeft];
            Joint cotoveloEsquerdo = esqueletoUsuario.Joints[JointType.ElbowLeft];
            double margemErro = 0.30;

            bool maoDireitaAlturaCorreta = Util.CompararComMargemErro(margemErro, maoDireita.Position.Y, centroOmbros.Position.Y);
            bool maoDireitaDistanciaCorreta = Util.CompararComMargemErro(margemErro, maoDireita.Position.Z, centroOmbros.Position.Z);
            bool maoDireitaAposCotovelo = maoDireita.Position.X > cotoveloDireito.Position.X;

            bool cotoveloDireitoAlturaCorreta = Util.CompararComMargemErro(margemErro, cotoveloDireito.Position.Y, centroOmbros.Position.Y);
            bool cotoveloEsquerdoAlturaCorreta = Util.CompararComMargemErro(margemErro, cotoveloEsquerdo.Position.Y, centroOmbros.Position.Y);

            bool maoEsquerdaAlturaCorreta = Util.CompararComMargemErro(margemErro, maoEsquerda.Position.Y, centroOmbros.Position.Y);
            bool maoEsquerdaDistanciaCorreta = Util.CompararComMargemErro(margemErro, maoEsquerda.Position.Z, centroOmbros.Position.Z);
            bool maoEsquerdaAposCotovelo = maoEsquerda.Position.X < cotoveloEsquerdo.Position.X;
            

            return maoDireitaAlturaCorreta && 
                   maoDireitaDistanciaCorreta && 
                   maoDireitaAposCotovelo &&
                   cotoveloDireitoAlturaCorreta &&
                   maoEsquerdaAlturaCorreta && 
                   maoEsquerdaDistanciaCorreta &&
                   maoEsquerdaAposCotovelo &&
                   cotoveloEsquerdoAlturaCorreta;
        }



    }
}
