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
	/// <summary>
	/// Extensions class for the <see cref="ChrominanceSubsampling"/> enumeration.
	/// </summary>
	public static class ChrominanceSubsamplingExtensions
	{
		static readonly int[] MCUWidth = {8, 16, 16, 8, 8, 32};

		/// <summary>
		/// Gets the MCU block width for a given level of chrominance subsampling.
		/// </summary>
		/// <returns>The MCU block width, in pixels.</returns>
		/// <param name="samp">A subsampling level.</param>
		/// <remarks>
		/// MCU block sizes:
		///  * - 8x8 for no subsampling or grayscale 
		///  * - 16x8 for 4:2:2
		///  * - 8x16 for 4:4:0 
		///  * - 16x16 for 4:2:0
		///  * - 32x8 for 4:1:1
		/// </remarks>
		public static int GetMCUWidth(this ChrominanceSubsampling samp)
		{
			return MCUWidth[(int)samp];
		}

		static readonly int[] MCUHeight = {8, 8, 16, 8, 16, 8};

		/// <summary>
		/// Gets the MCU block height for a given level of chrominance subsampling.
		/// </summary>
		/// <returns>The MCU block height, in pixels.</returns>
		/// <param name="samp">A subsampling level.</param>
		/// <remarks>
		/// MCU block sizes:
		///  * - 8x8 for no subsampling or grayscale 
		///  * - 16x8 for 4:2:2
		///  * - 8x16 for 4:4:0 
		///  * - 16x16 for 4:2:0
		///  * - 32x8 for 4:1:1
		/// </remarks>
		public static int GetMCUHeight(this ChrominanceSubsampling samp)
		{
			return MCUHeight[(int)samp];
		}

	}

	/// <summary>
	/// Chrominance subsampling options.
	/// When pixels are converted from RGB to YCbCr (see <see cref="Colorspace.YCbCr"/>) or from CMYK
	/// to YCCK (see <see cref="Colorspace.YCCK"/>) as part of the JPEG compression process, some of
	/// the Cb and Cr (chrominance) components can be discarded or averaged together
	/// to produce a smaller image with little perceptible loss of image clarity
	/// (the human eye is more sensitive to small changes in brightness than to
	/// small changes in color.)  This is called "chrominance subsampling".
	/// </summary>
	public enum ChrominanceSubsampling : int
	{
		/// <summary>
		/// 4:4:4 chrominance subsampling (no chrominance subsampling). The JPEG or
		/// YUV image will contain one chrominance component for every pixel in the
		/// source image.
		/// </summary>
		SAMP_444=0,
		/// <summary>
		/// 4:2:2 chrominance subsampling. The JPEG or YUV image will contain one
		/// chrominance component for every 2x1 block of pixels in the source image.
		/// </summary>
		SAMP_422,
		/// <summary>
		/// 4:2:0 chrominance subsampling. The JPEG or YUV image will contain one
		/// chrominance component for every 2x2 block of pixels in the source image.
		/// </summary>
		SAMP_420,
		/// <summary>
		/// Grayscale. The JPEG or YUV image will contain no chrominance components.
		/// </summary>
		SAMP_GRAY,
		/// <summary>
		/// 4:4:0 chrominance subsampling. The JPEG or YUV image will contain one
		/// chrominance component for every 1x2 block of pixels in the source image.
		/// </summary>
		/// <remarks>4:4:0 subsampling is not fully accelerated in libjpeg-turbo.</remarks>
		SAMP_440,
		/// <summary>
		/// 4:1:1 chrominance subsampling. The JPEG or YUV image will contain one
		/// chrominance component for every 4x1 block of pixels in the source image.
		/// JPEG images compressed with 4:1:1 subsampling will be almost exactly the
		/// same size as those compressed with 4:2:0 subsampling, and in the
		/// aggregate, both subsampling methods produce approximately the same
		/// perceptual quality. However, 4:1:1 is better able to reproduce sharp
		/// horizontal features.
		/// </summary>
		/// <remarks>4:1:1 subsampling is not fully accelerated in libjpeg-turbo.</remarks>
		SAMP_411
	}
}
