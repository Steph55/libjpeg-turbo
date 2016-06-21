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
	/// A .NET wrapper for TurboJPEG's compression routines.
	/// </summary>
	public class Compressor
	{
		IntPtr _handle;

		/// <summary>
		/// Initializes a new instance of the <see cref="TurboJPEG.Compressor"/> class.
		/// </summary>
		public Compressor ()
		{
			_handle = Library.tjInitCompress();
			if (_handle == IntPtr.Zero)
				throw new Exception(Library.tjGetErrorStr());
		}

		/// <summary>
		/// Releases the TurboJPEG handle before the <see cref="TurboJPEG.Compressor"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~Compressor()
		{
			if (_handle != IntPtr.Zero) {
				Library.tjDestroy(_handle);
				_handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Compress an RGB, grayscale, or CMYK image into a JPEG image.
		/// </summary>
		/// <returns>A byte array containing the compressed JPEG result.</returns>
		/// <param name="srcBuf">An image buffer containing RGB, grayscale, or CMYK pixels to be compressed.</param>
		/// <param name="width">Width (in pixels) of the source image.</param>
		/// <param name="pitch">
		///  Bytes per line of the source image.  Normally, this should be
		///  <c>width * format.GetPixelSize()</c> if the image is unpadded,
		///  or <c><see cref="Library.TJPAD"/>(width * format.GetPixelSize())</c> if each line of
		///  the image is padded to the nearest 32-bit boundary, as is the case
		///  for Windows bitmaps.  You can also be clever and use this parameter
		///  to skip lines, etc.  Setting this parameter to 0 is the equivalent of
		///  setting it to <c>width * format.GetPixelSize()</c>.
		/// </param>
		/// <param name="height">Height (in pixels) of the source image.</param>
		/// <param name="format">Pixel format of the source image.</param>
		/// <param name="jpegQual">The image quality of the generated JPEG image (1 = worst, 100 = best).</param>
		/// <param name="jpegSubsamp">The level of chrominance subsampling to be used when generating the JPEG image.</param>
		/// <param name="bottomUp">If true, then the uncompressed source image is stored in bottom-up (Windows, OpenGL) order, not top-down (X11) order.</param>
		public byte[] Compress(byte[] srcBuf, int width, int pitch, int height, PixelFormat format, int jpegQual, ChrominanceSubsampling jpegSubsamp = ChrominanceSubsampling.SAMP_420, bool bottomUp = false)
		{
			if (srcBuf == null)
				throw new ArgumentNullException(nameof (srcBuf));

			IntPtr jpegBuf = IntPtr.Zero;
			byte[] output;
			FunctionFlags flags = FunctionFlags.NoRealloc;
			if (bottomUp)
				flags |= FunctionFlags.BottomUp;

			try {
				// Although tjBufSize almost always returns a bigger buffer than we need,
				// it's better to have extra and only allocate once, instead of incurring
				// the cost of realloc and a mid compress buffer copy.
				uint jpegSize = Library.tjBufSize(width, height, jpegSubsamp);
				if (jpegSize == uint.MaxValue)
					throw new Exception(Library.tjGetErrorStr());
				jpegBuf = Library.tjAlloc((int)jpegSize);
				if (jpegBuf == IntPtr.Zero)
					throw new Exception(String.Format("tjAlloc failed to allocate {0} bytes", jpegSize));

				Library.tjCompress2(_handle, srcBuf, width, pitch, height, format, ref jpegBuf, ref jpegSize, jpegSubsamp, jpegQual, flags);

				output = new byte[jpegSize];

				Marshal.Copy(jpegBuf, output, 0, output.Length);
			} finally {
				if (jpegBuf != IntPtr.Zero)
					Library.tjFree(jpegBuf);
			}

			return output;
		}

	}
}
