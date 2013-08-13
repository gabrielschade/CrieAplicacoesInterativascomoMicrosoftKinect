using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ComandosVoz.Auxiliar
{
    public class ConfiguracaoDesenho
    {
        public FormaDesenho Forma { get; set; }
        public SolidColorBrush Cor { get; set; }
        public int Tamanho { get; set; }
        public bool DesenhoAtivo { get; set; }
    }
}
