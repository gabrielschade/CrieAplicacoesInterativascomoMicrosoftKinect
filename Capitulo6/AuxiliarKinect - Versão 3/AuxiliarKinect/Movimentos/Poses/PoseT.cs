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

        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado;
            if ( esqueletoUsuario != null && PosicaoValida(esqueletoUsuario))
            {
                if (QuadroIdentificacao == ContadorQuadros)
                    novoEstado = EstadoRastreamento.Identificado;
                else
                {
                    novoEstado = EstadoRastreamento.EmExecucao;
                    ContadorQuadros += 1;
                }
            }
            else
            {
                novoEstado = EstadoRastreamento.NaoIdentificado;
                ContadorQuadros = 0;
            }

            return novoEstado;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            Joint centroOmbros = esqueletoUsuario.Joints[JointType.ShoulderCenter];
            Joint maoDireita = esqueletoUsuario.Joints[JointType.HandRight];
            Joint maoEsquerda = esqueletoUsuario.Joints[JointType.HandLeft];
            double margemErro = 0.10;

            bool maoDireitaCorreta = Util.CompararComMargemErro(margemErro, maoDireita.Position.Y, centroOmbros.Position.Y);
            bool maoEsquerdaCorreta = Util.CompararComMargemErro(margemErro, maoEsquerda.Position.Y, centroOmbros.Position.Y);

            return maoDireitaCorreta && maoEsquerdaCorreta;
        }



    }
}
