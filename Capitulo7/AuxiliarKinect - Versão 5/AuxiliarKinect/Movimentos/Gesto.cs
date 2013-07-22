using Microsoft.Kinect;
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
        protected LinkedListNode<GestoQuadroChave> QuadroChaveAtual { get; set; }
        private int contadorEtapas;
        private EstadoRastreamento novoEstado;

        public int QuantidadeEtapas { get { return QuadrosChave.Count; } }

        public int PercentualEtapasConcluidas
        {
            get
            {
                return contadorEtapas * 100 / QuantidadeEtapas;
            }
        }

        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            if (esqueletoUsuario != null)
            {
                if (PosicaoValida(esqueletoUsuario))
                {
                    novoEstado = EstadoRastreamento.EmExecucao;
                    if (QuadroChaveAtual.Value == QuadrosChave.Last.Value)
                        novoEstado = EstadoRastreamento.Identificado;
                    else
                    {
                        if (ContadorQuadros >= QuadroChaveAtual.Value.QuadroLimiteInferior &&
                           ContadorQuadros <= QuadroChaveAtual.Value.QuadroLimiteSuperior)
                            ProximoQuadroChave();
                        else
                            if (ContadorQuadros < QuadroChaveAtual.Value.QuadroLimiteInferior)
                                PermanecerRastreando();
                            else if (ContadorQuadros > QuadroChaveAtual.Value.QuadroLimiteSuperior)
                                ReiniciarRastreamento();
                    }
                }
                else
                    if (QuadroChaveAtual.Value.QuadroLimiteSuperior < ContadorQuadros)
                        ReiniciarRastreamento();
                    else
                        PermanecerRastreando();
            }
            else
                ReiniciarRastreamento();

            return novoEstado;
        }

        private void ProximoQuadroChave()
        {
            novoEstado = EstadoRastreamento.EmExecucao;
            ContadorQuadros = 0;
            QuadroChaveAtual = QuadroChaveAtual.Next;
            contadorEtapas++;
        }

        private void ReiniciarRastreamento()
        {
            ContadorQuadros = 0;
            QuadroChaveAtual = QuadrosChave.First;
            contadorEtapas = 0;
            novoEstado = EstadoRastreamento.NaoIdentificado;
        }

        private void PermanecerRastreando()
        {
            ContadorQuadros++;
            novoEstado = EstadoRastreamento.EmExecucao;
        }

    }
}
