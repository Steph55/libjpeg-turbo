/*
 * Copyright (C)2014-2016 APX Labs, Inc.  All Rights Reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * - Neither the name of the libjpeg-turbo Project nor the names of its
 *   contributors may be used to endorse or promote products derived from this
 *   software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS",
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System.Runtime.InteropServices;

namespace TurboJPEG
{
	/// <summary>
	/// Scaling factor.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ScalingFactor
	{
		static ScalingFactor()
		{
			int numScalingFactors;
			ScalingFactors = Library.tjGetScalingFactors(out numScalingFactors);
		}

		/// <summary>
		/// An array of fractional scaling factors that the JPEG decompressor in
		/// this implementation of TurboJPEG supports.
		/// </summary>
		public static readonly ScalingFactor[] ScalingFactors;

		/// <summary>
		/// Compute the scaled value of <paramref name="dimension"/> using this scaling factor.
		/// </summary>
		/// <param name="dimension">Image width or image height.</param>
		/// <returns>The scaled value of the supplied dimension.</returns>
		/// <remarks>
		/// This function performs the integer equivalent of
		/// <see cref="Math.Ceiling"/>((double)<paramref name="dimension"/> * (double)<see cref="Num"/> / (double)<see cref="Denom"/>).
		/// </remarks>
		public int Scaled(int dimension)
		{
			return (dimension * Num + Denom - 1) / Denom;
		}

		/// <summary>
		/// Numerator.
		/// </summary>
		public int Num { get; private set; }

		/// <summary>
		/// Denominator.
		/// </summary>
		public int Denom { get; private set; }
	}
}
