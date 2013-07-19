using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public abstract class Gesto : Movimento
    {
        protected LinkedList<GestoQuadroChave> QuadrosChave { get; set; }
        protected LinkedListNode<GestoQuadroChave> QuadroChaveAtual {get;set;}
        private int contadorEtapas;

        public int QuantidadeEtapas { get { return QuadrosChave.Count; } }

        public int PercentualEtapasConcluidas
        {
            get
            {
                return contadorEtapas * 100 / QuantidadeEtapas;
            }
        }

        public override EstadoRastreamento Rastrear(Microsoft.Kinect.Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado;
            if (esqueletoUsuario != null)
            {
                if (PosicaoValida(esqueletoUsuario))
                {
                    novoEstado = EstadoRastreamento.EmExecucao;
                    if (QuadroChaveAtual.Value == QuadrosChave.Last.Value)
                        novoEstado = EstadoRastreamento.Identificado;
                    else if (QuadroChaveAtual.Value == QuadrosChave.First.Value)
                    {
                        novoEstado = EstadoRastreamento.EmExecucao;
                        ContadorQuadros += 1;
                        QuadroChaveAtual = QuadroChaveAtual.Next;
                        contadorEtapas++;
                    }
                    else
                    {
                        if (ContadorQuadros > QuadroChaveAtual.Value.QuadroLimiteInferior &&
                            ContadorQuadros < QuadroChaveAtual.Value.QuadroLimiteSuperior)
                        {
                            ContadorQuadros = 0;
                            QuadroChaveAtual = QuadroChaveAtual.Next;
                            contadorEtapas++;
                        }
                    }
                }
                else
                {
                    ContadorQuadros += 1;
                    novoEstado = EstadoRastreamento.EmExecucao;

                    if (QuadroChaveAtual.Value.QuadroLimiteSuperior < ContadorQuadros)
                    {
                        ResetarRastreamento();
                        novoEstado = EstadoRastreamento.NaoIdentificado;
                    }
                }
            }
            else
            {
                ResetarRastreamento();
                novoEstado = EstadoRastreamento.NaoIdentificado;
            }

            return novoEstado;
        }

        private void ResetarRastreamento()
        {
            ContadorQuadros = 0;
            QuadroChaveAtual = QuadrosChave.First;
            contadorEtapas = 0;
        }


    }
}
