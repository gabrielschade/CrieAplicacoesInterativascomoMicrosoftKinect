using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public class Rastreador<T> : IRastreador where T : Movimento, new()
    {
        private T movimento;
        private EstadoRastreamento estadoAnterior;
        public event EventHandler MovimentoIdentificado;

        public Rastreador()
        {
            estadoAnterior = EstadoRastreamento.NaoIdentificado;
            movimento = Activator.CreateInstance<T>();
        }

        public void Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento estadoAtual = movimento.Rastrear(esqueletoUsuario);

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
