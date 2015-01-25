using System;

namespace EcutController
{
    public class EcutEntity : IEcutService
    {
        private IntPtr eCutHandler;
        private const int NumOfAxis = 4;
        private UInt16 _delayBetweenPulseAndDir;
        private int[] _stepsPerUnit = new int[9];
        private UInt32 _smoothCoff;
        private UInt16 _axisMask;
        private double[] _acceleration = new double[9];
        private double[] _maxSpeed = new double[9];
        private UInt16 _ioOutPut;
        private ControllerConfigurationStruct _controllerConfiguration;
        private OutPutPinSettingStruct _outPutPinSetting;
        double[] _machinePostion;
        double[] _softPostion;

        private EcutEntity()
        {

        }

        public static IEcutService GetInstance()
        {
            return new EcutEntity();
        }

        public bool Open(int num)
        {
            if (eCutHandler.ToInt64() == 0)
            {
                eCutHandler = eCutDevice.eCutOpen(num);
                if (eCutHandler == null)
                    return false;
                var res = eCutDevice.eCutConfigDeviceDefault(eCutHandler);
                eCutDevice.eCutSetAxisOutputConfig(eCutHandler, this._outPutPinSetting.stepPin, this._outPutPinSetting.dirPin, new bool[9] { true, true, true, true, true, true, true, true, true }, 0, 0);

                if (this._controllerConfiguration != null)
                {
                    this.Acceleration = _controllerConfiguration.Acceleration;
                    this.MaxSpeed = _controllerConfiguration.Acceleration;
                    this.StepsPerUnit = _controllerConfiguration.StepsPerUnit;
                    this.SmoothCoff = _controllerConfiguration.SmoothCoff;
                    this.DelayBetweenPulseAndDir = _controllerConfiguration.DelayBetweenPulseAndDir;
                }
            }
            return true;
        }

        bool IEcutService.Open(ControllerConfigurationStruct controllerConfiguration, OutPutPinSettingStruct outPutPinSettingStruct, int num)
        {
            this._controllerConfiguration = controllerConfiguration;
            this._outPutPinSetting = outPutPinSettingStruct;
            return Open(num);
        }

        void IEcutService.Close()
        {
            if (eCutHandler.ToInt64() != 0)
            {
                eCutDevice.eCutEStop(eCutHandler);
                eCutDevice.eCutClose(eCutHandler);
                eCutHandler = (IntPtr)0;
            }
        }

        bool IEcutService.IsOpen()
        {
            return (eCutHandler.ToInt64() != 0);
        }

        int IEcutService.GetSumNumberOfEcut()
        {
            return eCutDevice.GetDeviceNum();
        }

        int[] IEcutService.GetSteps()
        {
            int[] steps = new int[6];
            if ((eCutHandler.ToInt64()) != 0 && (eCutError.eCut_True == eCutDevice.eCutGetSteps(eCutHandler, steps)))
                return steps;
            return null;
        }

        void IEcutService.AddLine(double[] postion, double velocity, double acceleration)
        {
            eCutPosition pos = new eCutPosition();
            eCutDevice.eCutSetCurrentPostion(eCutHandler, ref pos);

            if (postion != null)
            {
                pos.x = postion[0];
                pos.y = postion[1];
                pos.z = postion[2];
                pos.a = postion[3];
            }
            eCutDevice.eCutAddLine(eCutHandler, ref pos, velocity, velocity, acceleration);
        }

        void IEcutService.AddLineWithCertainPulse(int[] Pulse, double velocity, double acceleration)
        {
            throw new NotImplementedException();
        }

        void IEcutService.StopAll()
        {
            eCutDevice.eCutEStop(eCutHandler);
        }

        bool IEcutService.StopCertainAxis(int axisNum)
        {
            throw new NotImplementedException();
        }

        public ushort DelayBetweenPulseAndDir
        {
            get
            {
                return _delayBetweenPulseAndDir;
            }
            set
            {
                _delayBetweenPulseAndDir = value;
                MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public uint SmoothCoff
        {
            get
            {
                return (eCutHandler.ToInt64() != 0) ? CutConfiguration.GetSmoothCoff(eCutHandler) : 0;
            }
            set
            {
                if (eCutHandler.ToInt64() != 0)
                {
                    _smoothCoff = value;
                    CutConfiguration.SetStepsPerUnitWithSmoothCoffAndPulseDelay(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
                }
            }
        }

        public double[] Acceleration
        {
            set
            {
                _acceleration = value;
                CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public double[] MaxSpeed
        {
            get
            {
                return CutConfiguration.EcutGetMaxSpeed(eCutHandler);
            }
            set
            {
                _maxSpeed = value;
                CutConfiguration.EcutSetAccelerationMaxSpeed(eCutHandler, _acceleration, _maxSpeed);
            }
        }

        public int[] StepsPerUnit
        {
            get
            {
                return (eCutHandler.ToInt64() != 0) ? MoveManagerHardwareRelated.GetStepsPerUnit(eCutHandler) : null;
            }
            set
            {
                _stepsPerUnit = value;
                MoveManagerHardwareRelated.SetStepsPerUnit(eCutHandler, _stepsPerUnit, (int)_smoothCoff, _delayBetweenPulseAndDir);
            }
        }

        public double[] MachinePostion
        {
            get
            {
                return _machinePostion;
            }
            set
            {
                _machinePostion = value;
                if (_machinePostion == null)
                    _machinePostion = new double[9];
                eCutDevice.eCutSetCoordinate(eCutHandler, _machinePostion);
                    
            }
        }

        public double[] SoftPostion
        {
            get
            {
                return _softPostion;
            }
            set
            {
                _softPostion = value;
                if (_softPostion == null)
                    _softPostion = new double[9];
                var pos = new eCutPosition(){x = _softPostion[0], y = _softPostion[1], z = _softPostion[2], a = _softPostion[3]};
                eCutDevice.eCutSetCurrentPostion(eCutHandler, ref pos);
            }
        }

        public int? InputIO 
        { 
            get 
            { 
                return IOManager.GetIO(eCutHandler);
            }
        }

        public ushort OutputIO 
        {
            set
            {
                IOManager.SetIO(eCutHandler, value);
            } 
        }
    }
}
