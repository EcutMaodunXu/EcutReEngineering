using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public class MoveInfoStruct
    {
        public int Type { get; set; }
        public double[] Position { get; set; }
        public double Speed { get; set; }
        public string Gcode { get; set; }
        public MoveInfoStruct()
        {
            Position = new double[4];
        }
    }
}
