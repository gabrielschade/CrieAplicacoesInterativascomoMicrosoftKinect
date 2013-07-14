using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.FuncoesBasicas
{
    public static class Util
    {
        public static bool CompararComMargemErro(double margemErro, double valor1, double valor2)
        {
            return valor1 >= valor2 - margemErro && valor1 <= valor2 + margemErro;
        }

        public static double CalcularProdutoEscalar(Joint articulacao1, Joint articulacao2, Joint articulacao3)
        {
            Vector4 vetorV = CriarVetorEntreDoisPontos(articulacao1, articulacao2);
            Vector4 vetorW = CriarVetorEntreDoisPontos(articulacao2, articulacao3);


            double resultadoRadianos = Math.Acos(ProdutoVetores(vetorV, vetorW) / ProdutoModuloVetores(vetorV, vetorW));
            double resultadoGraus = resultadoRadianos * 180 / Math.PI;

            return resultadoGraus;
        }

        private static double ProdutoVetores(Vector4 vetorV, Vector4 vetorW)
        {
            return vetorV.X * vetorW.X +
                   vetorV.Y * vetorW.Y +
                   vetorV.Z * vetorW.Z;
        }

        private static double ProdutoModuloVetores(Vector4 vetorV, Vector4 vetorW)
        {
            return Math.Sqrt(Math.Pow(vetorV.X, 2) +
                           Math.Pow(vetorV.Y, 2) +
                           Math.Pow(vetorV.Z, 2))
                           *
                   Math.Sqrt(Math.Pow(vetorW.X, 2) +
                           Math.Pow(vetorW.Y, 2) +
                           Math.Pow(vetorW.Z, 2));
        }

        private static Vector4 CriarVetorEntreDoisPontos(Joint articulacao1, Joint articulacao2)
        {
            Vector4 vetorResultante = new Vector4();

            vetorResultante.X = Convert.ToSingle(Math.Sqrt(Math.Pow(articulacao1.Position.X - articulacao2.Position.X, 2)));
            vetorResultante.Y = Convert.ToSingle(Math.Sqrt(Math.Pow(articulacao1.Position.Y - articulacao2.Position.Y, 2)));
            vetorResultante.Z = Convert.ToSingle(Math.Sqrt(Math.Pow(articulacao1.Position.Z - articulacao2.Position.Z, 2)));

            return vetorResultante;
        }

    }
}
