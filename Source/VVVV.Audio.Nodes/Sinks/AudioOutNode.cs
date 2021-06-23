﻿#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.Audio;


using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
    [PluginInfo(Name = "AudioOut", Category = "VAudio", Help = "Audio Out, first slice will go to first driver output channel, second slice will be second and so on... " +
                "With the offset pin the channels can be moved to other outputs: OutputChannel = SliceNumber + Offset", AutoEvaluate = true, Tags = "Asio")]
    public class AudioOutNode : IPluginEvaluate, IDisposable
    {
        #region fields & pins
        [Input("Input")]
        public IDiffSpread<AudioSignal> FInput;
        
        [Input("Channel Offset")]
        public IDiffSpread<int> FChannelOffsetIn;
        
        [Import()]
        ILogger FLogger;
        #endregion fields & pins
    
        private ISpread<MasterChannel> LastSignals;
        public AudioOutNode()
        {
            LastSignals = new Spread<MasterChannel>();
        }
        
        public void Dispose()
        {
            AudioService.Engine.RemoveOutput(LastSignals);
        }    
        
        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            if(FInput.IsChanged || FChannelOffsetIn.IsChanged)
            {
                AudioService.Engine.RemoveOutput(LastSignals);
                LastSignals.SliceCount = SpreadMax;
                for (int i = 0; i < SpreadMax; i++)
                {
                    LastSignals[i] = new MasterChannel(FInput[i], i + FChannelOffsetIn[i]);
                }
                
                AudioService.Engine.AddOutput(LastSignals);
            }
        }
    }
}


