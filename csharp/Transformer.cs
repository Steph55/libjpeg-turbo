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
using System.Runtime.InteropServices;

namespace TurboJPEG
{
	/// <summary>
	/// A .NET wrapper for TurboJPEG's lossless JPEG transform routines.
	/// </summary>
	public class Transformer
	{
		IntPtr _handle;

		/// <summary>
		/// Initializes a new instance of the <see cref="TurboJPEG.Transformer"/> class.
		/// </summary>
		public Transformer ()
		{
			_handle = Library.tjInitTransform();
			if (_handle == IntPtr.Zero)
				throw new Exception(Library.tjGetErrorStr());
		}

		/// <summary>
		/// Releases the TurboJPEG handle before the <see cref="TurboJPEG.Transformer"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~Transformer()
		{
			if (_handle != IntPtr.Zero) {
				Library.tjDestroy(_handle);
				_handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Losslessly transform a JPEG image into another JPEG image.  Lossless
		/// transforms work by moving the raw coefficients from one JPEG image structure
		/// to another without altering the values of the coefficients.  While this is
		/// typically faster than decompressing the image, transforming it, and
		/// re-compressing it, lossless transforms are not free.  Each lossless
		/// transform requires reading and performing Huffman decoding on all of the
		/// coefficients in the source image, regardless of the size of the destination
		/// image.  Thus, this function provides a means of generating multiple
		/// transformed images from the same source or  applying multiple
		/// transformations simultaneously, in order to eliminate the need to read the
		/// source coefficients multiple times.
		/// </summary>
		/// <returns>An array of byte arrays containing the losslessly transformed JPEG images.</returns>
		/// <param name="jpegBuf">Buffer containing the JPEG image to transform.</param>
		/// <param name="transforms">An array of <see cref="Transform"/> structures, each of
		///        which specifies the transform parameters and/or cropping region for
		///        the corresponding transformed output image.</param>
		public byte[][] Transform(byte[] jpegBuf, Transform[] transforms)
		{
			if (jpegBuf == null)
				throw new ArgumentNullException(nameof (jpegBuf));
			if (transforms == null)
				throw new ArgumentNullException(nameof (transforms));

			IntPtr[] dstBufs = new IntPtr[transforms.Length];

			// Initialize all destination buffer pointers to NULL, to tell TurboJPEG to allocate the buffers for us
			for (int i = 0; i < dstBufs.Length; ++i)
			{
				dstBufs[i] = IntPtr.Zero;
			}

			uint[] dstSizes = new uint[transforms.Length];

			int retval = Library.tjTransform(_handle, jpegBuf, (uint)jpegBuf.Length, transforms.Length, dstBufs, dstSizes, transforms, 0);
			if (retval != 0)
				throw new Exception(Library.tjGetErrorStr());

			byte[][] output = new byte[transforms.Length][];

			for (int i = 0; i < transforms.Length; ++i)
			{
				output[i] = new byte[dstSizes[i]];

				Marshal.Copy(dstBufs[i], output[i], 0, output[i].Length);
				Library.tjFree(dstBufs[i]);
				dstBufs[i] = IntPtr.Zero;
			}

			return output;
		}
	}
}
