using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public class Rastreador<T> where T : Movimento
    {
        private T movimento;
        private EstadoRastreamento estadoAnterior;
        public event EventHandler MovimentoIdentificado;
        public event EventHandler MovimentoPerdido;

        public Rastreador()
        {
            movimento = Activator.CreateInstance<T>();
            estadoAnterior = EstadoRastreamento.NaoIdentificado;
        }

        public void Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento estadoAtual = movimento.Rastrear(esqueletoUsuario);

            if (estadoAtual == EstadoRastreamento.NaoIdentificado &&
                estadoAnterior == EstadoRastreamento.Identificado)
                ChamarEvento(MovimentoPerdido);

            if (estadoAtual == EstadoRastreamento.Identificado &&
                estadoAnterior != EstadoRastreamento.Identificado)
                ChamarEvento(MovimentoIdentificado);

            estadoAnterior = estadoAtual;
        }

        private void ChamarEvento(EventHandler evento)
        {
            if (evento != null)
                evento(movimento, new EventArgs());
        }

    }
}
