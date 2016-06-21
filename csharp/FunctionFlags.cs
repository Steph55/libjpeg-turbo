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
	[Flags]
	internal enum FunctionFlags : int
	{
		/// <summary>
		/// The uncompressed source/destination image is stored in bottom-up (Windows,
		/// OpenGL) order, not top-down (X11) order.
		/// </summary>
		BottomUp = 2,

		/// <summary>
		/// Turn off CPU auto-detection and force TurboJPEG to use MMX code (if the underlying codec supports it.)
		/// </summary>
		[Obsolete]
		ForceMMX = 8,

		/// <summary>
		/// Turn off CPU auto-detection and force TurboJPEG to use SSE code (if the underlying codec supports it.)
		/// </summary>
		[Obsolete]
		ForceSSE = 16,

		/// <summary>
		/// Turn off CPU auto-detection and force TurboJPEG to use SSE2 code (if the underlying codec supports it.)
		/// </summary>
		[Obsolete]
		ForceSSE2 = 32,

		/// <summary>
		/// Turn off CPU auto-detection and force TurboJPEG to use SSE3 code (if the underlying codec supports it.)
		/// </summary>
		[Obsolete]
		ForceSSE3 = 128,

		/// <summary>
		/// When decompressing an image that was compressed using chrominance
		/// subsampling, use the fastest chrominance upsampling algorithm available in
		/// the underlying codec.  The default is to use smooth upsampling, which
		/// creates a smooth transition between neighboring chrominance components in
		/// order to reduce upsampling artifacts in the decompressed image.
		/// </summary>
		FastUpSample = 256,

		/// <summary>
		/// Disable buffer (re)allocation.  If passed to <see cref="Library.tjCompress2"/> or
		/// <see cref="Library.tjTransform"/>, this flag will cause those functions to generate an error if
		/// the JPEG image buffer is invalid or too small rather than attempting to
		/// allocate or reallocate that buffer.  This reproduces the behavior of earlier
		/// versions of TurboJPEG.
		/// </summary>
		NoRealloc = 1024,

		/// <summary>
		/// Use the fastest DCT/IDCT algorithm available in the underlying codec.  The
		/// default if this flag is not specified is implementation-specific.  For
		/// example, the implementation of TurboJPEG for libjpeg[-turbo] uses the fast
		/// algorithm by default when compressing, because this has been shown to have
		/// only a very slight effect on accuracy, but it uses the accurate algorithm
		/// when decompressing, because this has been shown to have a larger effect.
		/// </summary>
		FastDCT = 2048,

		/// <summary>
		/// Use the most accurate DCT/IDCT algorithm available in the underlying codec.
		/// The default if this flag is not specified is implementation-specific.  For
		/// example, the implementation of TurboJPEG for libjpeg[-turbo] uses the fast
 		/// algorithm by default when compressing, because this has been shown to have
 		/// only a very slight effect on accuracy, but it uses the accurate algorithm
 		/// when decompressing, because this has been shown to have a larger effect.
		/// </summary>
		AccurateDCT = 4096
	}
}
