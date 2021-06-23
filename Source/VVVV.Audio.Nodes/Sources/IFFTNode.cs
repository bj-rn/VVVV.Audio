﻿#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.Audio;
using VVVV.Core.Logging;

#endregion usings

namespace VVVV.Nodes
{
    
    [PluginInfo(Name = "IFFT", Category = "VAudio", Version = "Source", Help = "Creates an audio signal from spectrum data", AutoEvaluate = true, Tags = "additive, synthesis")]
    public class IFFTSignalNode : AutoAudioSignalNode<IFFTSignal>
    {
        
    }
}


