using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos.Gestos.Aceno
{
    public class Aceno:Gesto
    {
        public Aceno()
        {
            GestoQuadroChave etapa1 = new GestoQuadroChave(new AcenoMaoAposCotovelo(), 0, 0);
            GestoQuadroChave etapa2 = new GestoQuadroChave(new AcenoMaoSobreCotovelo(), 1, 35);
            GestoQuadroChave etapa3 = new GestoQuadroChave(new AcenoMaoAntesCotovelo(), 1, 35);
            GestoQuadroChave etapa4 = new GestoQuadroChave(new AcenoMaoSobreCotovelo(), 1, 35);
            GestoQuadroChave etapa5 = new GestoQuadroChave(new AcenoMaoAposCotovelo(), 1, 35);

            QuadrosChave = new LinkedList<GestoQuadroChave>();
            QuadrosChave.AddFirst(etapa1);
            QuadrosChave.AddLast(etapa2);
            QuadrosChave.AddLast(etapa3);
            QuadrosChave.AddLast(etapa4);
            QuadrosChave.AddLast(etapa5);

            Nome = "Aceno";
            ContadorQuadros = 0;
            QuadroChaveAtual = QuadrosChave.First;
        }

        protected override bool PosicaoValida(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento estado = QuadroChaveAtual.Value.PoseChave.Rastrear(esqueletoUsuario);
            return estado == EstadoRastreamento.Identificado;
        }
    }
}
