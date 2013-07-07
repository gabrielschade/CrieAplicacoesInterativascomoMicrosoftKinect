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

    }
}
