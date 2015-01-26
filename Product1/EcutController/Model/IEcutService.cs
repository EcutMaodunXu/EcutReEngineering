using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public interface    IEcutService
    {
        bool Open(int num);
        bool Open(ControllerConfigurationStruct controllerConfiguration, OutPutPinSettingStruct outPutPinSettingStruct, int num);
        void Close();
        bool IsOpen();
        int GetSumNumberOfEcut();
        int[] GetSteps();
        void AddLine(double[] postion, double velocity, double acceleration);
        void AddLineWithCertainPulse(int[] Pulse, double velocity, double acceleration);
        void StopAll();
        bool StopCertainAxis(int axisNum);
        int? InputIO { get; }
        ushort OutputIO { set; }
        ushort DelayBetweenPulseAndDir { get; set; }
        uint SmoothCoff { get; set; }
        double[] Acceleration { set; }
        double[] MaxSpeed { get; set; }
        int[] StepsPerUnit { get; set; }
        double[] MachinePostion { get; set; }
        double[] SoftPostion { get; set; }
    }
}
