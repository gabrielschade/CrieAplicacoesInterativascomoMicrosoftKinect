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
    }
}
