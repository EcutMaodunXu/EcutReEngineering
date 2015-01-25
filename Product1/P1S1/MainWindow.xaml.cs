using EcutController;
using Microsoft.Win32;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Utility;
using System.Linq;

//Product : Ecut Software Demo Sprint 1
namespace P1S1
{
    public partial class MainWindow : Window
    {
        InfoBorad infoBorad;
        InfoBorad gCodeBorad;
        IEcutService eCutController;

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            MainWindow win = new MainWindow();
            app.Run(win);
        }

        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            InitControls();
            eCutController = EcutEntity.GetInstance();
            InitTimer();
        }

        private void InitTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Start();
        }


        private void InitControls()
        {
            infoBorad = new InfoBorad(MessageBox);
            gCodeBorad = new InfoBorad(GcodeBox);
            initIOLed();
            bindingManualControlModeRate();
            initAxisPos();

        }

        private int zeroModeCounter;
        private void AxistTick(object sender, EventArgs e)
        {
            var stepNumer = eCutController.GetSteps();
            var stepsPerUnit = eCutController.StepsPerUnit;
            var smooth = (double)eCutController.SmoothCoff;

            for (int i = 0; i < 4; i++)
            {
                AxisNumbers[i].Value = (double)((double)(stepNumer[i] / smooth / stepsPerUnit[i]));
            }
            //TODO:此处有很多魔数
            if (ManualGrid.IsEnabled == false)
            {
                if ((Math.Round(AxisNumbers[0].Value, 3) == 0) && (Math.Round(AxisNumbers[1].Value, 3) == 0)
                    && (Math.Round(AxisNumbers[2].Value, 3) == 0) && (Math.Round(AxisNumbers[3].Value, 3) == 0))
                {
                    ManualGrid.IsEnabled = true;
                }
                if ((Math.Abs(Math.Round(AxisNumbers[0].Value, 3)) <= 0.008) && (Math.Abs(Math.Round(AxisNumbers[1].Value, 3)) <= 0.008) &&
                    (Math.Abs(Math.Round(AxisNumbers[2].Value, 3)) <= 0.008) && (Math.Abs(Math.Round(AxisNumbers[3].Value, 3)) <= 0.008))
                {
                    zeroModeCounter++;
                    if (zeroModeCounter > 5)
                    {

                        ALLToZero();
                        zeroModeCounter = 0;
                    }
                }
                else
                {
                    zeroModeCounter = 0;
                }
            }
        }

        bindingNumber[] AxisNumbers = new bindingNumber[4];
        private void initAxisPos()
        {
            var textBlockArray = new TextBlock[4];
            textBlockArray[0] = XAxisPosArea;
            textBlockArray[1] = YAxisPosArea;
            textBlockArray[2] = ZAxisPosArea;
            textBlockArray[3] = AAxisPosArea;
            for (int i = 0; i < 4; i++)
            {
                AxisNumbers[i] = new bindingNumber();
                var axisBingSetting = new Binding("DisplayNumber");
                axisBingSetting.Source = AxisNumbers[i];
                textBlockArray[i].SetBinding(TextBlock.TextProperty, axisBingSetting);
            }

        }

        bindingNumber manualMoveRate;
        private void bindingManualControlModeRate()
        {
            manualMoveRate = new bindingNumber();
            var sliderBindSetting = new Binding("Value");
            sliderBindSetting.Source = manualMoveRate;
            ManualMoveSlider.SetBinding(Slider.ValueProperty, sliderBindSetting);

            var textBlockBindSetting = new Binding("Percentage");
            textBlockBindSetting.Source = manualMoveRate;
            ManualMoveRateTextBlock.SetBinding(TextBlock.TextProperty, textBlockBindSetting);
            manualMoveRate.Value = 5;
        }

        ushort OutPutValStore;
        void LedTimerTick(object sender, EventArgs e)
        {

            if (ledManager != null)
            {
                ushort OutPutVal = 0;
                int? InPutVal = 0;
                for (ushort i = 16; i < 32; i++)
                {
                    OutPutVal += (ledManager[i].EnableFlag) ? ((ushort)(1 << (i - 16))) : (ushort)0;
                }
                if (OutPutValStore != OutPutVal)
                {
                    eCutController.OutputIO = OutPutVal;
                    OutPutValStore = OutPutVal;
                }
                InPutVal = eCutController.InputIO;
                if (InPutVal != null)
                {
                    for (ushort i = 0; i < 16; i++)
                    {
                        ledManager[i].EnableFlag = ((InPutVal & (1 << i)) == 0) ? false : true;
                    }
                }
            }
        }

        int LEDCount = 32;
        LEDManager[] ledManager;
        /// <summary>
        /// IO区域初始化，使用绑定
        /// </summary>
        private void initIOLed()
        {
            ledManager = new LEDManager[32];
            for (int i = 0; i < LEDCount; i++)
            {
                var grid = new Grid();
                grid.Style = (Style)FindResource("LEDGrid");

                var ellipse = new Ellipse();
                ellipse.Style = (Style)FindResource("LED");
                ledManager[i] = new LEDManager();
                var bindSetting = new Binding("EnableFlag");
                var convertor = new LEDConverter();
                bindSetting.Converter = convertor;
                bindSetting.Source = ledManager[i];
                ellipse.SetBinding(Ellipse.FillProperty, bindSetting);
                var label = new Label();
                label.Style = (Style)FindResource("LEDLable");

                grid.Children.Add(ellipse);
                grid.Children.Add(label);
                //为了一次性解决IO 不得已写成下面这样
                if (i < LEDCount / 2)
                {
                    label.Content = i.ToString();
                    InputLEDPanel.Children.Add(grid);
                }
                else
                {
                    var checkBox = new CheckBox();
                    checkBox.Style = (Style)FindResource("LEDCheckBox");
                    var checkBoxBinding = new Binding("EnableFlag");
                    checkBoxBinding.Source = ledManager[i];
                    checkBoxBinding.Mode = BindingMode.OneWayToSource;
                    checkBox.SetBinding(CheckBox.IsCheckedProperty, checkBoxBinding);

                    grid.Children.Add(checkBox);

                    label.Content = (i - 16).ToString();
                    OutPutLEDPanel.Children.Add(grid);
                }
            }
        }

        private void ConnetEcut(object sender, RoutedEventArgs e)
        {
            if (eCutController.IsOpen())
            {
                eCutController.Close();
                ECutConnecter.Content = "连接e-Cut";
                infoBorad.AddInfo("已断开连接");
                timer.Tick -= AxistTick;
                timer.Tick -= LedTimerTick;
                ControlTab.IsEnabled = false;
                ConnectLED.Fill = new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80));
            }
            else
            {
                if (eCutController.GetSumNumberOfEcut() == 0)
                {
                    infoBorad.AddInfo("没有找到e-Cut");
                    return;
                }
                try
                {
                    ConnectLED.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    
                    ControlTab.IsEnabled = true;
                    eCutController.Open(XmlUtility.GetConfig(), XmlUtility.GetOutPutPinSetting(), 0);
                    ECutConnecter.Content = "断开连接";
                    infoBorad.AddInfo("成功连接");
                    timer.Tick += AxistTick;
                    timer.Tick += LedTimerTick;
                    ManualAUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualXUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualYUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualZUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualADown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualXDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualYDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualZDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);

                    ManualAUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualXUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualYUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualZUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualADown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualXDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualYDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualZDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);

                    RotateAddBt.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(AddGlRotateDown), true);
                    RotateAddBt.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(AddGlRotateUp), true);

                    RotateSubBt.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(SubGlRotateDown), true);
                    RotateSubBt.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(SubGlRotateUp), true);
                }
                catch (Exception)
                {
                    infoBorad.OutPutNormalError();
                    return;
                }
            }
        }

        private void ManualMouseDown(object sender, RoutedEventArgs e)
        {
            var rate = double.Parse(manualMoveRate.Percentage.Replace("%", "")) / 100.0;
            const double valueForUpMove = 9999999;
            const double valueForDownMove = -9999999;
            double value = 0;
            ushort taskAxis = 0;
            switch ((sender as Button).Name)
            {
                case "ManualXDown":
                    taskAxis = 1;
                    value = valueForDownMove;
                    break;
                case "ManualXUp":
                    taskAxis = 1;
                    value = valueForUpMove;
                    break;
                case "ManualYDown":
                    taskAxis = 2;
                    value = valueForDownMove;
                    break;
                case "ManualYUp":
                    taskAxis = 2;
                    value = valueForUpMove;
                    break;
                case "ManualZDown":
                    taskAxis = 3;
                    value = valueForDownMove;
                    break;
                case "ManualZUp":
                    taskAxis = 3;
                    value = valueForUpMove;
                    break;
                case "ManualADown":
                    taskAxis = 4;
                    value = valueForDownMove;
                    break;
                case "ManualAUp":
                    taskAxis = 4;
                    value = valueForUpMove;
                    break;
                default:
                    break;
            }
            //TODO
            //eCutController.AddLineWithCertainAxis(taskAxis, value, rate);
        }

        private void ManualMouseUp(object sender, RoutedEventArgs e)
        {
            eCutController.StopAll();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            eCutController.Close();
        }

        private void ShowAxisSetting(object sender, RoutedEventArgs e)
        {
            var axisSettingWindow = new AxisSetting();
            axisSettingWindow.ShowDialog();
        }

        private void ShowOutPutPinSetting(object sender, RoutedEventArgs e)
        {
            var outPutPinSettingWindow = new OutPutPinSettingWindow();
            outPutPinSettingWindow.ShowDialog();
        }


        private void AllAxisToZero(object sender, RoutedEventArgs e)
        {
            ALLToZero();
            ManualGrid.IsEnabled = false;
            zeroModeCounter = 0;
        }

        private void ALLToZero()
        {
            var stepNumer = eCutController.GetSteps();
            var stepsPerUnit = eCutController.StepsPerUnit;
            var smooth = (double)eCutController.SmoothCoff;
            if(stepNumer != null && stepsPerUnit != null)
            {
                var taskLengthArray = new double[4]{
                -(double)stepNumer[0] / (double)stepsPerUnit[0] / (double)smooth,
                -(double)stepNumer[1] / (double)stepsPerUnit[1] / (double)smooth,
                -(double)stepNumer[2] / (double)stepsPerUnit[2] / (double)smooth,
                -(double)stepNumer[3] / (double)stepsPerUnit[3] / (double)smooth
                };
                //TODO
                eCutController.AddLine(taskLengthArray, eCutController.MaxSpeed.Average(), 100);
            }
        }

        private void ConfigCoordinate(object sender, RoutedEventArgs e)
        {
            eCutController.MachinePostion = (new double[9] { double.Parse(XCoordinateInput.Text), double.Parse(YCoordinateInput.Text), double.Parse(ZCoordinateInput.Text), 
            double.Parse(ACoordinateInput.Text),0, 0, 0, 0, 0});
        }

        private List<MoveInfoStruct> moveInfoList;
        /// <summary>
        /// TODO：G代码解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGcodeFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "文本文件|*.txt";
            if (fileDialog.ShowDialog() == true)
            {
                var Text = System.IO.File.ReadAllText(fileDialog.FileName);
                //moveInfoList = gCodeParser.ParseCode(Text);
                
                if (moveInfoList == null)
                {
                    infoBorad.AddInfo("请输入正确的G代码文件");
                    return;
                }
                foreach (var item in moveInfoList)
                {
                    gCodeBorad.AddInfo(item.Gcode.Replace("\r", ""));
                }
                loadReady = true;
            }
        }

        private void RunGcode(object sender, RoutedEventArgs e)
        {
            foreach (var item in moveInfoList)
            {
                //TODO
                if (item.Type == 1)
                {
                    //eCutController.AddLine(new double[4]{item.Position[0],
                    //item.Position[1],item.Position[2],item.Position[3]},);
                }
            }
        }

        bool GlInit = false;
        double[] lastAxisVal = new double[3];
        double RotateAngle = 0;
        bool OnRotating = false;

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = GlArea.OpenGL;
            if (!GlInit || OnRotating)
            {
                //  Clear the color and depth buffers.
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                //  Reset the modelview matrix.
                //将当前点移到了屏幕中心，X坐标轴从左至右，Y坐标轴从下至上，Z坐标轴从里至外。OpenGL屏幕中心的坐标值是X和Y轴上的0.0f点。
                gl.LoadIdentity();

                //Move the geometry into a fairly central position.
                gl.Translate(0f, 0.0f, -2.0f);

                //  Draw a pyramid. First, rotate the modelview matrix.
                gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);

                gl.LineWidth(2);

                gl.Begin(OpenGL.GL_LINES);
                gl.Color(0f, 0f, 1.0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(0.0f, 1.0f, 0f);
                gl.End();
                gl.Flush();

                gl.Begin(OpenGL.GL_LINES);
                gl.Color(1f, 0f, 0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(0.0f, 0f, 1.0f);
                gl.End();
                gl.Flush();

                gl.Begin(OpenGL.GL_LINES);
                gl.Color(0f, 1f, 0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(1.0f, 0f, 0f);
                gl.End();
                gl.Flush();

                gl.Flush();
                GlInit = true;
            }
            else
            {
                gl.LoadIdentity();
                gl.Translate(0f, 0.0f, -2.0f);
                gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);

                gl.Begin(OpenGL.GL_LINES);
                gl.Color(1f, 1f, 1f);

                gl.Vertex(lastAxisVal[0] / 40, lastAxisVal[1] / 40, lastAxisVal[2] / 40);
                gl.Vertex(AxisNumbers[0].Value / 40, AxisNumbers[1].Value / 40, AxisNumbers[2].Value / 40);
                gl.End();
                gl.Flush();
                lastAxisVal[0] = AxisNumbers[0].Value;
                lastAxisVal[1] = AxisNumbers[1].Value;
                lastAxisVal[2] = AxisNumbers[2].Value;
            }
            if (loadReady || OnRotating)
            {
                if (moveInfoList != null)
                { 
                    var array = moveInfoList.ToArray();
                    for (int i = 0; i < array.Length - 1; i++)
                    {
                        gl.LoadIdentity();
                        gl.Translate(0f, 0.0f, -2.0f);
                        gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);
                        //##007acc
                        if (array[i].Type == 1)
                        {
                            gl.Begin(OpenGL.GL_LINES);
                            gl.Color((float)(0x68) / 255.0, (float)(0x7a) / 255.0, (float)(0xcc) / 255.0);

                            gl.Vertex(array[i].Position[0] / 40, array[i].Position[1] / 40, array[i].Position[2] / 40);
                            gl.Vertex(array[i + 1].Position[0] / 40, array[i + 1].Position[1] / 40, array[i + 1].Position[2] / 40);
                            gl.End();
                            gl.Flush();
                        }
                    }
                    loadReady = false;
                }
            }
        }

        /// <summary>
        /// 重绘图像用到的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearGl(object sender, RoutedEventArgs e)
        {
            GlInit = false;
        }

        private void AddGlRotateUp(object sender, RoutedEventArgs e)
        {
            timer.Tick -= AddGlTick;
            OnRotating = false;
        }

        private void AddGlTick(object sender, EventArgs e)
        {
            OnRotating = true;
            RotateAngle += 1.0;
        }

        private void AddGlRotateDown(object sender, RoutedEventArgs e)
        {
            timer.Tick += AddGlTick;
            OnRotating = false;
        }

        private void SubGlRotateUp(object sender, RoutedEventArgs e)
        {
            timer.Tick -= SubGlTick;
        }

        private void SubGlTick(object sender, EventArgs e)
        {
            OnRotating = true;
            RotateAngle -= 1.0;
        }

        private void SubGlRotateDown(object sender, RoutedEventArgs e)
        {
            timer.Tick += SubGlTick;
        }

        public bool loadReady { get; set; }
    }

    #region 系统提示坂处理
    public class InfoBorad
    {
        private ListBox listBox;

        public InfoBorad(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public void SelectItem(int index)
        {
            this.listBox.SelectedIndex = index;
        }

        public void AddInfo(string targetInfo)
        {
            this.listBox.Items.Add(targetInfo);

            var border = (Border)VisualTreeHelper.GetChild(this.listBox, 0);
            var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            scrollViewer.ScrollToBottom();
        }

        public void OutPutNormalError()
        {
            AddInfo("软件异常，请与技术人员练习");
        }
    }
    #endregion 
}
