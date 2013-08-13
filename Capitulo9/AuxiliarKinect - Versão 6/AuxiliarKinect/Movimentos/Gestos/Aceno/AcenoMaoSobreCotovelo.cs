using AuxiliarKinect.FuncoesBasicas;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos.Gestos.Aceno
{
    public class AcenoMaoSobreCotovelo : Pose
    {
        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            double margemErro = 0.30;
            Joint maoDireita = esqueletoUsuario.Joints[JointType.HandRight];
            Joint cotoveloDireito = esqueletoUsuario.Joints[JointType.ElbowRight];

            bool maoDireitaAntesCotovelo = maoDireita.Position.X > cotoveloDireito.Position.X;
            bool maoDireitaSobreCotovelo = Util.CompararComMargemErro(margemErro, maoDireita.Position.Y, cotoveloDireito.Position.Y);

            return maoDireitaSobreCotovelo && maoDireitaAntesCotovelo;
        }
    }
}
