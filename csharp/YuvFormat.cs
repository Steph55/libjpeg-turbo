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

namespace TurboJPEG
{
	public enum YuvFormat
	{
		/// <summary>
		/// YCbCr planar format with no chrominance subsampling. The Y, Cb, and Cr
		/// planes are identically sized, and appear in that order.
		/// </summary>
		YUV444P=0,

		/// <summary>
		/// YCbCr planar format with 4:2:2 chrominance subsampling. The Y plane is
		/// twice as large as the Cb and Cr planes that follow.
		/// </summary>
		YUV422P,

		/// <summary>
		/// YCbCr planar format with 4:2:0 chrominance subsampling. The Y plane is
		/// four times as large as the Cb and Cr planes that follow.  This format
		/// is identical to YV12 with the chrominance planes switched.
		/// 
		/// Also known as I420.
		/// </summary>
		YU12,

		/// <summary>
		/// Luminance plane only, no chrominance planes. Effectively grayscale.
		/// </summary>
		Y800,

		/// <summary>
		/// YCbCr planar format with 4:4:0 chrominance subsampling. The Y plane is
		/// twice as large as the Cb and Cr planes that follow.
		/// </summary>
		YUV440P,

		/// <summary>
		/// YCbCr planar format with 4:1:1 chrominance subsampling.  The Y plane is
		/// four times as large as the Cb and Cr planes that follow.
		/// </summary>
		YUV411P,

		/// <summary>
		/// YCrCb planar format with 4:2:0 chrominance subsampling.  The Y plane is
		/// four times as large as the Cr and Cb planes that follow.  This format
		/// is identical to YU12 with the chrominance planes switched.
		/// </summary>
		YV12,

		/// <summary>
		/// YCbCr planar format with 4:2:0 chrominance subsampling and interleaving
		/// of Cb and Cr samples.  The Y plane is twice as large as the combined CbCr
		/// plane, where each pair of bytes contains the Cb and Cr values of a 2x2
		/// block of pixels.  This format is identical to NV21, except that Cb values
		/// precede Cr values in each pixel of the chrominance plane.
		/// </summary>
		NV12,

		/// <summary>
		/// YCrCb planar format with 4:2:0 chrominance subsampling and interleaving
		/// of Cr and Cb samples.  The Y plane is twice as large as the combined CrCb
		/// plane, where each pair of bytes contains the Cr and Cb values of a 2x2
		/// block of pixels.  This format is identical to NV12, except that Cr values
		/// precede Cb values in each pixel of the chrominance plane.
		/// </summary>
		NV21,

		/// <summary>
		/// YCbCr planar format with 4:2:2 chrominance subsampling and interleaving
		/// of Cb and Cr samples.  The Y plane is the same size as the combined CbCr
		/// plane, where each pair of bytes contains the Cb and Cr values of a 2x1
		/// block of pixels.  This format is identical to NV61, except that Cb values
		/// precede Cr values in each pixel of the chrominance plane.
		/// </summary>
		NV16,

		/// <summary>
		/// YCrCb planar format with 4:2:2 chrominance subsampling and interleaving
		/// of Cr and Cb samples.  The Y plane is the same size as the combined CrCb
		/// plane, where each pair of bytes contains the Cr and Cb values of a 2x1
		/// block of pixels.  This format is identical to NV16, except that Cr values
		/// precede Cb values in each pixel of the chrominance plane.
		/// </summary>
		NV61
	}

	/// <summary>
	/// Methods to run on YuvFormat values.
	/// </summary>
	public static class YuvFormatExtensions
	{
		/// <summary>
		/// Gets the kind of <see cref="ChrominanceSubsampling"/> used by a given YuvFormat.
		/// </summary>
		/// <returns>The subsampling level.</returns>
		/// <param name="yuvFormat">The YuvFormat.</param>
		public static ChrominanceSubsampling GetSubsamplingLevel(this YuvFormat yuvFormat)
		{
			return YUVSubsamp[(int)yuvFormat];
		}

		static readonly ChrominanceSubsampling[] YUVSubsamp =
		{
			ChrominanceSubsampling.SAMP_444,
			ChrominanceSubsampling.SAMP_422,
			ChrominanceSubsampling.SAMP_420,
			ChrominanceSubsampling.SAMP_GRAY,
			ChrominanceSubsampling.SAMP_440,
			ChrominanceSubsampling.SAMP_411,
			ChrominanceSubsampling.SAMP_420,
			ChrominanceSubsampling.SAMP_420,
			ChrominanceSubsampling.SAMP_420,
			ChrominanceSubsampling.SAMP_422,
			ChrominanceSubsampling.SAMP_422
		};
	}

}
