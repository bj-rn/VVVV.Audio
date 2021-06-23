﻿using System;
using Jacobi.Vst.Core;
using Jacobi.Vst.Core.Host;

namespace VVVV.Audio.VST
{
    /// <summary>
    /// The HostCommandStub class represents the part of the host that a plugin can call.
    /// </summary>
    class HostCommandStub : IVstHostCommandStub 
    {
        /// <summary>
        /// Raised when one of the methods is called.
        /// </summary>
        public event EventHandler<PluginCalledEventArgs> PluginCalled;

        private void RaisePluginCalled(string message)
        {
            EventHandler<PluginCalledEventArgs> handler = PluginCalled;

            if(handler != null)
            {
                handler(this, new PluginCalledEventArgs(message));
            }
        }

        #region IVstHostCommandsStub Members

        /// <inheritdoc />
        public IVstPluginContext PluginContext { get; set; }
        
        #endregion

        #region IVstHostCommands20 Members

        bool FEditing;

        /// <inheritdoc />
        public bool BeginEdit(int index)
        {
            RaisePluginCalled("BeginEdit(" + index + ")");
            FEditing = true;

            return true;
        }

        /// <inheritdoc />
        public VstCanDoResult CanDo(string cando)
        {
            RaisePluginCalled("CanDo(" + cando + ")");
            if(cando == "supportShell") return VstCanDoResult.No;
            if(cando == "shellCategory") return VstCanDoResult.No;
            if(cando == "sendVstEvents") return VstCanDoResult.Yes;
            if(cando == "sendVstMidiEvent") return VstCanDoResult.Yes;
            if(cando == "sendVstTimeInfo") return VstCanDoResult.Yes;
            if(cando == "receiveVstEvents") return VstCanDoResult.Yes;
            if(cando == "receiveVstMidiEvent") return VstCanDoResult.Yes;
            if(cando == "receiveVstTimeInfo") return VstCanDoResult.Yes;
            if(cando == "acceptIOChanges") return VstCanDoResult.Yes;
           
            return Jacobi.Vst.Core.VstCanDoResult.Unknown;
        }

        /// <inheritdoc />
        public bool CloseFileSelector(VstFileSelect fileSelect)
        {
            RaisePluginCalled("CloseFileSelector(" + fileSelect.Command + ")");
            return false;
        }
        public Action<int> RaiseSave;
        /// <inheritdoc />
        public bool EndEdit(int index)
        {
            RaisePluginCalled("EndEdit(" + index + ")");
            FEditing = false;

            if (RaiseSave != null) RaiseSave(index);

            return true;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstAutomationStates GetAutomationState()
        {
            RaisePluginCalled("GetAutomationState()");
            return Jacobi.Vst.Core.VstAutomationStates.ReadWrite;
        }

        /// <inheritdoc />
        public int GetBlockSize()
        {
            RaisePluginCalled("GetBlockSize()");
            return AudioService.Engine.Settings.BufferSize;
        }

        /// <inheritdoc />
        public string GetDirectory()
        {
            RaisePluginCalled("GetDirectory()");
            return null;
        }

        /// <inheritdoc />
        public int GetInputLatency()
        {
            RaisePluginCalled("GetInputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public VstHostLanguage GetLanguage()
        {
            RaisePluginCalled("GetLanguage()");
            return VstHostLanguage.English;
        }

        /// <inheritdoc />
        public int GetOutputLatency()
        {
            RaisePluginCalled("GetOutputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public VstProcessLevels GetProcessLevel()
        {
            //RaisePluginCalled("GetProcessLevel()");
            return VstProcessLevels.Realtime;
        }

        /// <inheritdoc />
        public string GetProductString()
        {
            RaisePluginCalled("GetProductString()");
            return "VVVV";
        }

        /// <inheritdoc />
        public float GetSampleRate()
        {
            RaisePluginCalled("GetSampleRate()");
            return AudioService.Engine.Settings.SampleRate;
        }

        /// <inheritdoc />
        public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags)
        {
            //RaisePluginCalled("GetTimeInfo(" + filterFlags + ")");

            return VSTService.TimeInfo;
        }

        /// <inheritdoc />
        public string GetVendorString()
        {
            RaisePluginCalled("GetVendorString()");
            return "vvvv group";
        }

        /// <inheritdoc />
        public int GetVendorVersion()
        {
            RaisePluginCalled("GetVendorVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public bool IoChanged()
        {
            RaisePluginCalled("IoChanged()");
            return false;
        }

        /// <inheritdoc />
        public bool OpenFileSelector(VstFileSelect fileSelect)
        {
            RaisePluginCalled("OpenFileSelector(" + fileSelect.Command + ")");
            return false;
        }

        /// <inheritdoc />
        public Action<VstEvent[]> FProcessEventsAction;
        public bool ProcessEvents(Jacobi.Vst.Core.VstEvent[] events)
        {
            //RaisePluginCalled("ProcessEvents(" + events.Length + ")");
            if(FProcessEventsAction != null)
                FProcessEventsAction(events);
            return false;
        }
        

        /// <inheritdoc />
        public bool SizeWindow(int width, int height)
        {
            RaisePluginCalled("SizeWindow(" + width + ", " + height + ")");
            return false;
        }

        /// <inheritdoc />
        public bool UpdateDisplay()
        {
            RaisePluginCalled("UpdateDisplay()");
            return false;
        }

        #endregion

        #region IVstHostCommands10 Members

        /// <inheritdoc />
        public int GetCurrentPluginID()
        {
            RaisePluginCalled("GetCurrentPluginID()");
            return 0;
        }

        /// <inheritdoc />
        public int GetVersion()
        {
            RaisePluginCalled("GetVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public void ProcessIdle()
        {
            RaisePluginCalled("ProcessIdle()");
        }

        /// <inheritdoc />
        public void SetParameterAutomated(int index, float value)
        {
            RaisePluginCalled("SetParameterAutomated(" + index + ", " + value + ")");
            if (!FEditing)
            {
                if (RaiseSave != null)
                    RaiseSave(index);
            }

        }

        #endregion
    }

    /// <summary>
    /// Event arguments used when one of the mehtods is called.
    /// </summary>
    class PluginCalledEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new instance with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message"></param>
        public PluginCalledEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }
    }
}
