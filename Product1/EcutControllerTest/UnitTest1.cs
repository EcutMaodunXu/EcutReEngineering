using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcutControllerTest ;
using EcutController;

namespace EcutControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        public IntPtr eCutHandler;
        IEcutService ecutService;

        [TestInitialize]
        public void TestInit()
        {
            ecutService = EcutEntity.GetInstance();   
            //hahah
        }

        
    }
}
