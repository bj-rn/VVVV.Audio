﻿#region usings
using System;
using NAudio.Utils;
using VVVV.PluginInterfaces.V2;
#endregion
namespace VVVV.Audio
{
	public class MatrixMixerSignal : MultiChannelInputSignal
	{
		public ISpread<float> GainMatrix
		{
			get;
			protected set;
		}

		public int OutputChannelCount
		{
			get
			{
				return this.FOutputCount;
			}
			set
			{
				SetOutputCount(value);
				GainMatrix = new Spread<float>(value);
			}
		}

		float[] FTempBuffer = new float[1];

		protected override void FillBuffers(float[][] buffer, int offset, int count)
		{
			if (FInput != null && FInput.SliceCount != 0) 
			{
				FTempBuffer = BufferHelpers.Ensure(FTempBuffer, count);
				for (int outSlice = 0; outSlice < FOutputCount; outSlice++)
				{
					var outbuf = buffer[outSlice];
					for (int inSlice = 0; inSlice < FInput.SliceCount; inSlice++)
					{
						var gain = GainMatrix[outSlice + inSlice * FOutputCount];
						var inSig = FInput[inSlice];
						if (inSig != null)
						{
							inSig.Read(FTempBuffer, offset, count);
							if (inSlice == 0)
							{
								for (int j = 0; j < count; j++) 
								{
									outbuf[j] = FTempBuffer[j] * gain;
								}
							}
							else 
							{
								for (int j = 0; j < count; j++) 
								{
									outbuf[j] += FTempBuffer[j] * gain;
								}
							}
						}
					}
				}
			}
		}
	}
}




