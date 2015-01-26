﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcutControllerTest ;
using EcutController;

namespace EcutControllerTest
{
    [TestClass]
    public class eCutControllerTest
    {
        public IntPtr eCutHandler;
        IEcutService ecutService;

        [TestInitialize]
        public void TestInit()
        {
            ecutService = EcutEntity.GetInstance();   
        }

        [TestMethod]
        public void TestIsOpen()
        {
            var result = ecutService.IsOpen();
            Assert.AreEqual(result , true);
        }

        public void Open(int num) //等官回来确认下。
        {
            var result = ecutService.Open(num);
            Assert.AreEqual(num , ecutService.GetSumNumberOfEcut());
        }

        [TestMethod]
        public void Close()
        {
            ecutService.Close();
        }

        [TestMethod]
        public void TestStopAll()
        {
            ecutService.StopAll(); 
        }

        [TestMethod]
        public int TestGetSumNumberOfEcut()
        {
            return ecutService.GetSumNumberOfEcut();
        }

        [TestMethod]
        public void TestStepsPerUnit()
        {
            var steps = new int[] { 100 ,0 , 0 , 0 , 0 , 0 , 0 , 0 , 0};
            ecutService.StepsPerUnit = steps;
            for (int i = 0; i < steps.Length - 1; i++)
            {
                Assert.AreEqual(steps[i] , ecutService.StepsPerUnit[i]);
            }
        }

        [TestMethod]
        public void TestDelayBetweenPulseAndDir()
        {
            ushort delay = 200;
            ecutService.DelayBetweenPulseAndDir = delay;
            Assert.AreEqual(ecutService.DelayBetweenPulseAndDir, delay);
        }

        [TestMethod]
        public void TestSmoothCoff()
        {
            uint smoothCoff = 400;
            ecutService.SmoothCoff = smoothCoff;
            Assert.AreEqual(ecutService.SmoothCoff , smoothCoff);
        }

        [TestMethod]
        public void TestMaxSpeed()
        {
            var maxSpeed = new double[] { 10 , 10 ,10 ,10 , 0 ,0 ,0 ,0 , 0};
            ecutService.MaxSpeed = maxSpeed;
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(ecutService.MaxSpeed[i] , maxSpeed[i]);
            }
        }

        [TestMethod]
        public void TestMachinePostion()
        {
            var machinePosition = new double[] { 10, 10, 10, 10, 0, 0, 0, 0, 0 };
            ecutService.MachinePostion = machinePosition;
            System.Threading.Thread.Sleep(200);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(ecutService.GetSteps()[i] /ecutService.SmoothCoff/ecutService.StepsPerUnit[i] , (int)machinePosition[i]);
            }
        }

        [TestMethod]
        public void TestAddLine()
        {
            double[] postion = new double[] { 10, 10, 10, 10, 0, 0, 0, 0, 0 };
            double acceleration = 20;
            double velocity = 10;
            ecutService.AddLine(postion ,velocity ,acceleration); 
            var step = new int[9] ;
            System.Threading.Thread.Sleep(20);
            while (ecutService.GetSteps()[0] != step[0] || ecutService.GetSteps()[1] != step[1] ||
                   ecutService.GetSteps()[2] != step[2] || ecutService.GetSteps()[3] != step[3])
            {
                step = ecutService.GetSteps();
                System.Threading.Thread.Sleep(20);
            }
            var dpos = ecutService.GetSteps()[0] / ecutService.SmoothCoff / ecutService.StepsPerUnit[0];
            for (int i = 0; i < postion.Length - 1; i++)
            {
                Assert.AreEqual(postion[i],dpos);
            }
        }

    }
}
