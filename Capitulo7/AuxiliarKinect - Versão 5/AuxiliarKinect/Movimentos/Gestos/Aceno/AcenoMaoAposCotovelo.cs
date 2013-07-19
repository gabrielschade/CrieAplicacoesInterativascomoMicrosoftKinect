using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos.Gestos.Aceno
{
    public class AcenoMaoAposCotovelo : Pose
    {
        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            Joint maoDireita = esqueletoUsuario.Joints[JointType.HandRight];
            Joint cotoveloDireito = esqueletoUsuario.Joints[JointType.ElbowRight];

            bool maoDireitaAposCotovelo = maoDireita.Position.X > cotoveloDireito.Position.X;
            bool maoDireitaAcimaCotovelo = maoDireita.Position.Y > cotoveloDireito.Position.Y;

            return maoDireitaAcimaCotovelo && maoDireitaAposCotovelo;
        }
    }
}
