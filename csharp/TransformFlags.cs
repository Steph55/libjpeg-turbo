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

using System;

namespace TurboJPEG
{
	/// <summary>
	/// Flags for <see cref="Transform"/> structures passed to <see cref="Transformer.Transform"/>.
	/// </summary>
	[Flags]
	public enum TransformFlags
	{
		/// <summary>
		/// This option will cause <see cref="Transformer.Transform"/> to return an error if the transform is
		/// not perfect.  Lossless transforms operate on MCU blocks, whose size depends
		/// on the level of chrominance subsampling used (see <see cref="ChrominanceSubsamplingExtensions.GetMCUWidth"/>
		/// and <see cref="ChrominanceSubsamplingExtensions.GetMCUHeight"/>.)  If the image's width or height is not evenly divisible
		/// by the MCU block size, then there will be partial MCU blocks on the right
		/// and/or bottom edges.  It is not possible to move these partial MCU blocks to
		/// the top or left of the image, so any transform that would require that is
		/// "imperfect."  If this option is not specified, then any partial MCU blocks
		/// that cannot be transformed will be left in place, which will create
		/// odd-looking strips on the right or bottom edge of the image.
		/// </summary>
		Perfect = 1,

		/// <summary>
		/// This option will cause <see cref="Transformer.Transform"/> to discard any partial MCU blocks that
		/// cannot be transformed.
		/// </summary>
		Trim = 2,

		/// <summary>
		/// This option will enable lossless cropping.  See <see cref="Transformer.Transform"/> for more
		/// information.
		/// </summary>
		Crop = 4,

		/// <summary>
		/// This option will discard the color data in the input image and produce
		/// a grayscale output image.
		/// </summary>
		Gray = 8,

		/// <summary>
		/// This option will prevent <see cref="Transformer.Transform"/> from outputting a JPEG image for
		/// this particular transform (this can be used in conjunction with a custom
		/// filter to capture the transformed DCT coefficients without transcoding
		/// them.)
		/// </summary>
		NoOutput = 16
	}
}
