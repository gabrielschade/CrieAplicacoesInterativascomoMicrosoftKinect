using Microsoft.Kinect.Toolkit.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComandosVoz.Auxiliar
{
    public class CanvasInteracao : Canvas, IInteractionClient
    {
        public InteractionInfo GetInteractionInfoAtLocation(int skeletonTrackingId, InteractionHandType handType, double x, double y)
        {
            InteractionInfo info = new InteractionInfo();
            info.IsGripTarget = true;

            return info;
        }
    }
}
