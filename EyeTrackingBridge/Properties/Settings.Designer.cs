﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EyeTrackingBridge.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string OscHostname {
            get {
                return ((string)(this["OscHostname"]));
            }
            set {
                this["OscHostname"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9000")]
        public int OscPort {
            get {
                return ((int)(this["OscPort"]));
            }
            set {
                this["OscPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string BrokenEyeHostname {
            get {
                return ((string)(this["BrokenEyeHostname"]));
            }
            set {
                this["BrokenEyeHostname"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5555")]
        public int BrokenEyePort {
            get {
                return ((int)(this["BrokenEyePort"]));
            }
            set {
                this["BrokenEyePort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("40")]
        public float DirectionLimitIn {
            get {
                return ((float)(this["DirectionLimitIn"]));
            }
            set {
                this["DirectionLimitIn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("40")]
        public float DirectionLimitOut {
            get {
                return ((float)(this["DirectionLimitOut"]));
            }
            set {
                this["DirectionLimitOut"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public float DirectionLimitUp {
            get {
                return ((float)(this["DirectionLimitUp"]));
            }
            set {
                this["DirectionLimitUp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public float DirectionLimitDown {
            get {
                return ((float)(this["DirectionLimitDown"]));
            }
            set {
                this["DirectionLimitDown"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.25")]
        public float OpennessThresholdClose {
            get {
                return ((float)(this["OpennessThresholdClose"]));
            }
            set {
                this["OpennessThresholdClose"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.75")]
        public float OpennessThresholdOpen {
            get {
                return ((float)(this["OpennessThresholdOpen"]));
            }
            set {
                this["OpennessThresholdOpen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.03")]
        public float TransitionTimeClose {
            get {
                return ((float)(this["TransitionTimeClose"]));
            }
            set {
                this["TransitionTimeClose"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.05")]
        public float TransitionTimeOpen {
            get {
                return ((float)(this["TransitionTimeOpen"]));
            }
            set {
                this["TransitionTimeOpen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.2")]
        public float BlinkThreshold {
            get {
                return ((float)(this["BlinkThreshold"]));
            }
            set {
                this["BlinkThreshold"] = value;
            }
        }
    }
}
