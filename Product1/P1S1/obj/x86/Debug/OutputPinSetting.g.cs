﻿#pragma checksum "..\..\..\OutputPinSetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6DC7D41151375B80F525DC7BEACE81D1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace P1S1 {
    
    
    /// <summary>
    /// OutPutPinSettingWindow
    /// </summary>
    public partial class OutPutPinSettingWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox XStepCB;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox YStepCB;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ZStepCB;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AStepCB;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox XDirCB;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox YDirCB;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ZDirCB;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\OutputPinSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ADirCB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/P1S1;component/outputpinsetting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\OutputPinSetting.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.XStepCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.YStepCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.ZStepCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.AStepCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.XDirCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.YDirCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.ZDirCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.ADirCB = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            
            #line 79 "..\..\..\OutputPinSetting.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ConformClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 80 "..\..\..\OutputPinSetting.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

