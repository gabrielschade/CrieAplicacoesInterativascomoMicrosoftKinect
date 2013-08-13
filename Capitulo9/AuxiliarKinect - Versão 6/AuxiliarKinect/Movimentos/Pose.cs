using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public abstract class Pose : Movimento
    {
        protected int QuadroIdentificacao { get; set; }

        public int PercentualProgresso
        {
            get 
            {
                return ContadorQuadros * 100 / QuadroIdentificacao;
            }
        }

        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado;
            if (esqueletoUsuario != null && PosicaoValida(esqueletoUsuario))
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
    }
}
