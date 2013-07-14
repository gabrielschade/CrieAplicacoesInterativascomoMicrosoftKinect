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
        public EstadoRastreamento EstadoAtual { get; private set; }
        public event EventHandler MovimentoIdentificado;

        public Rastreador()
        {
            EstadoAtual = EstadoRastreamento.NaoIdentificado;
            movimento = Activator.CreateInstance<T>();
        }

        public void Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado = movimento.Rastrear(esqueletoUsuario);

            if (novoEstado == EstadoRastreamento.Identificado &&
                EstadoAtual != EstadoRastreamento.Identificado)
                ChamarEvento(MovimentoIdentificado);

            EstadoAtual = novoEstado;
        }

        private void ChamarEvento(EventHandler evento)
        {
            if (evento != null)
                evento(movimento, new EventArgs());
        }

    }
}
