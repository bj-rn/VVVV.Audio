using System;
using VVVV.Audio;
using VVVV.PluginInterfaces.V2;
using VVVV.Nodes.Generic;

namespace VVVV.Nodes
{
    //1.) do a 'replace all' for AudioSignal with the name of your own type
    
    //2.) do a 'replace all' for VAudio to set the version and the class name prefix for all node (e.g. Value, Color...)
    
    //3.) you might also override the Equals method of your type which is used to check whether two instances are the same

    #region SingleValue
    
    [PluginInfo(Name = "Equals", 
                Category = "VAudio",
                Help = "returns true if the values at the inputs are equal",
                Tags = "compare, same"
               )]
    public class AudioSignalEqualsNode: Equals<AudioSignal> {}
    
    #endregion SingleValue
    
    #region SpreadOps
    
    [PluginInfo(Name = "Occurrence", 
                Category = "VAudio",
                Help = "Counts the occurrence of equal slices.",
                Tags = "count, spectral, generic",
                   Author = "woei"
               )]
    public class AudioSignalOccurrenceNode: Occurrence<AudioSignal> 
    {
        //uncomment this method to override the equals directly if you can't or dont want to override it for the class
//        public override bool Equals(AudioSignal a, AudioSignal b)
//        {
//            return a.Equals(b);
//        }
    }
    
    #endregion SpreadOps

}

