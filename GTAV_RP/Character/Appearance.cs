using System;
using System.Collections.Generic;
using System.Text;

namespace GTAV_RP.Character
{
    public class Appearance
    {
        public IEnumerable<FaceFeature> facefeature { get; set; }
        public IEnumerable<ComponentVariation> components { get; set; }
        public IEnumerable<HeadOverlay> headoverlay { get; set; }
        public int genID { get; set; }
        
    }

    public class FaceFeature
    {
        public int faceIndex { get; set; }
        public float faceScale { get; set; }
    }

    public class ComponentVariation
    {
        public int componentId { get; set; }
        public int drawableId { get; set; }
        public int textureId { get; set; }
    }

    public class HeadOverlay
    {
        public int overlayId { get; set; }
        public int index { get; set; }
        public int firstColor { get; set; }
        public float opacity { get; set; }
    }
}
