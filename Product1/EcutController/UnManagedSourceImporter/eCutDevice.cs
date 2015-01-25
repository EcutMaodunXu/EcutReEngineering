using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EcutController
{
    public enum eCutStopType
    {
        eCut_Stop_Type_Stop = 0,
        eCut_Stop_Type_Exact = 1,
        eCut_Stop_Type_Parabolic = 2,
        eCut_Stop_Type_Tangent = 3
    }

    public enum eCutError
    {
        eCut_Error_Ok = 0,
        eCut_Error_NullPointer = 1,
        eCut_Error = 2,
        eCut_True = 3,
        eCut_False = 4,
    }

    public struct eCutPosition
    {
        public double x, y, z;
        public double a, b, c;
        public double u, v, w;
    }

    public struct eCutCartesian
    {
        public double x, y, z;
    }

    public class eCutDevice
    {
        /// <summary>   
        /// 开启与e-Cut的连接
        /// </summary>
        /// <param name="Num">第几个e-Cut</param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern IntPtr eCutOpen(int Num);

        /// <summary>
        /// 获取当前检测到e-Cut的数量
        /// </summary>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int GetDeviceNum();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="SerialString"></param>
        /// <returns></returns>
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError GetDeviceInfo(int Num, char[] SerialString);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutClose(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutResume(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutPause(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutAbort(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutIsDone(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutConfigDeviceDefault(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutMoveAbsolute(IntPtr eCut, UInt16 AxisMask, Double[] PositionGiven);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetInputIO(IntPtr eCut, ref int Input);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetSteps(IntPtr eCut, Int32[] Steps);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetIOOutput(IntPtr eCut, UInt16 Out);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetSpindle(IntPtr eCut, UInt16 Out);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutStopAll(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutEStop(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutStop(IntPtr eCut, UInt16 Axis);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetCoordinate(IntPtr eCut, double[] Posi);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetSmoothCoff(IntPtr eCut, ref UInt32 pSmoothCoff);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetStepsPerUnit(IntPtr eCut, int[] StepsPerUnit);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetMaxSpeed(IntPtr eCut, Double[] MaxSpeed);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetStepNegAndDirNeg(IntPtr eCut, Byte StepNeg, Byte DirNeg);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetStepsPerUnitSmoothCoff(IntPtr eCut, UInt16 DelayBetweenPulseAndDir,
        int[] StepsPerAxis, int[] WorkOffset, int SmoothCoff);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutJogOn(IntPtr eCut, UInt16 Axis, ref double PositionGiven);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int eCutActiveDepth(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int eCutQueueDepth(IntPtr eCut);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetStopType(IntPtr eCut, eCutStopType type, double tolerance);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetAccelarationMaxSpeed(IntPtr eCut, double[] Acceleration, double[] MaxSpeed);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutMoveAtSpeed(IntPtr eCut, UInt16 AxisMask, double[] Acceleration, double[] MaxSpeed);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetInputIOEngineDir(IntPtr eCut,
                UInt64 InputIOEnable,
                UInt64 InputIONeg,
                Byte[] InputIOPin,
                SByte[] EngineDirections
            );
        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetG92StepDirEncPin(IntPtr eCut,
                UInt32[] G92Offset,/*In Pulse Number*/
                UInt16 StepNegAndDirNeg,
                Byte[] EncoderAPin,
                Byte[] EncoderBPin,
                Byte MPGIndex
                );

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetSpindlePostion(IntPtr eCut, ref UInt16 SpindlePostion);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGetEncoderPostion(IntPtr eCut, UInt16[] EncoderPostion);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetAxisOutputConfig(IntPtr eCut, Byte[] StepSel,
                Byte[] DirSel,
                bool[] Enable,
                UInt16 StepNeg,
                UInt16 DirNeg
                );

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutSetCurrentPostion(IntPtr eCut, ref eCutPosition Pos);//DONE

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutAddLine(IntPtr eCut, ref eCutPosition end, double vel, double ini_maxvel, double acc);//TO DO DIFF BETWEEN vel ini_maxvel

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutGcodeLineInterpret(IntPtr eCut, char[] Code);

        [DllImport("eCutDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern eCutError eCutAddCircle(IntPtr eCut,

                ref eCutPosition end,
                ref eCutCartesian center,
                ref eCutCartesian normal,
                int turn,
                double vel,
                double ini_maxvel,
                double acc
                );
    }
}
