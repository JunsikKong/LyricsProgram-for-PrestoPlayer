﻿#pragma checksum "..\..\LyricsWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "599B4725E955F16D07B0191A2CB8440BC848CC60"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using Presto.SDK;
using Presto.SWCamp.Lyrics;
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


namespace Presto.SWCamp.Lyrics {
    
    
    /// <summary>
    /// LyricsWindow
    /// </summary>
    public partial class LyricsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbk1;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbk2;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbk3;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSyncUp;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSyncDn;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSyncRst;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLoadLrc;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\LyricsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbkSync;
        
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
            System.Uri resourceLocater = new System.Uri("/Presto.SWCamp.Lyrics;component/lyricswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LyricsWindow.xaml"
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
            this.tbk1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.tbk2 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.tbk3 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btnSyncUp = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\LyricsWindow.xaml"
            this.btnSyncUp.Click += new System.Windows.RoutedEventHandler(this.btnSyncUp_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnSyncDn = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\LyricsWindow.xaml"
            this.btnSyncDn.Click += new System.Windows.RoutedEventHandler(this.btnSyncDn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnSyncRst = ((System.Windows.Controls.Button)(target));
            
            #line 77 "..\..\LyricsWindow.xaml"
            this.btnSyncRst.Click += new System.Windows.RoutedEventHandler(this.btnSyncRst_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnLoadLrc = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\LyricsWindow.xaml"
            this.btnLoadLrc.Click += new System.Windows.RoutedEventHandler(this.btnLoadLrc_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tbkSync = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

