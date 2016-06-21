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
	/// A .NET wrapper for TurboJPEG's decompression routines.
	/// </summary>
	public class Decompressor
	{
		IntPtr _handle;

		/// <summary>
		/// Initializes a new instance of the <see cref="TurboJPEG.Decompressor"/> class.
		/// </summary>
		public Decompressor ()
		{
			_handle = Library.tjInitDecompress();
			if (_handle == IntPtr.Zero)
				throw new Exception(Library.tjGetErrorStr());
		}

		/// <summary>
		/// Releases the TurboJPEG handle before the <see cref="TurboJPEG.Decompressor"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~Decompressor()
		{
			if (_handle != IntPtr.Zero) {
				Library.tjDestroy(_handle);
				_handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Decompress a JPEG image to an RGB or grayscale image.
		/// </summary>
		/// <returns>A byte buffer containing the decompressed image.</returns>
		/// <param name="jpegBuf">A buffer containing the JPEG image to decompress.</param>
		/// <param name="pixelFormat">Desired pixel format of the destination image.</param>
		/// <param name="bottomUp">If true, then the uncompressed result should be stored in bottom-up (Windows, OpenGL) order, not top-down (X11) order.</param>
		public byte[] Decompress(byte[] jpegBuf, PixelFormat pixelFormat = PixelFormat.RGB, bool bottomUp = false)
		{
			if (jpegBuf == null)
				throw new ArgumentNullException(nameof (jpegBuf));

			ChrominanceSubsampling subsamp;
			Colorspace colorspace;
			int width, height;
			DecompressHeader(jpegBuf, out width, out height, out subsamp, out colorspace);

			int pitch = pixelFormat.GetPixelSize() * width;

			byte[] dstBuf = new byte[pitch * height];
			int retval = 0;

			FunctionFlags flags = FunctionFlags.NoRealloc;
			if (bottomUp)
				flags |= FunctionFlags.BottomUp;

			retval = Library.tjDecompress2(_handle, jpegBuf, (uint)jpegBuf.Length, dstBuf, width, pitch, height, pixelFormat, flags);

			if (retval != 0)
				throw new Exception(Library.tjGetErrorStr());

			return dstBuf;
		}

		/// <summary>
		/// Decompress a JPEG image to a YUV planar image.  This function performs JPEG
		/// decompression but leaves out the color conversion step, so a planar YUV
		/// image is generated instead of an RGB image.
		/// </summary>
		/// <returns>A byte buffer containing the decompressed image in planar YUV format.</returns>
		/// <param name="jpegBuf">A buffer containing the JPEG image to decompress.</param>
		/// <param name="pad">
		/// The width of each line in each plane of the YUV image will be padded to the nearest multiple of this number
		/// of bytes (must be a power of 2.) To generate images suitable for X Video, this should be set to 4.
		/// </param>
		/// <param name="bottomUp">If true, then the uncompressed result should be stored in bottom-up (Windows, OpenGL) order, not top-down (X11) order.</param>
		/// <remarks>
		/// Note that, if the width or height of the image is not an even multiple of
		/// the MCU block size (see <see cref="ChrominanceSubsamplingExtensions.GetMCUWidth"/>
		/// and <see cref="ChrominanceSubsamplingExtensions.GetMCUHeight"/>), then an
		/// intermediate buffer copy will be performed within TurboJPEG.
		/// </remarks>
		public byte[] DecompressToYUV(byte[] jpegBuf, int pad = 4, bool bottomUp = false)
		{
			if (jpegBuf == null)
				throw new ArgumentNullException(nameof (jpegBuf));

			ChrominanceSubsampling subsamp;
			Colorspace colorspace;
			int width, height;
			DecompressHeader(jpegBuf, out width, out height, out subsamp, out colorspace);

			FunctionFlags flags = FunctionFlags.NoRealloc;
			if (bottomUp)
				flags |= FunctionFlags.BottomUp;

			uint outputSize = Library.tjBufSizeYUV2(width, pad, height, subsamp);
			if (outputSize == UInt32.MaxValue)
				throw new Exception(Library.tjGetErrorStr());

			byte[] output = new byte[outputSize];
			int retval = Library.tjDecompressToYUV2(_handle, jpegBuf, (uint)jpegBuf.Length, output, width, pad, height, flags);

			if (retval != 0)
				throw new Exception(Library.tjGetErrorStr());

			return output;
		}

		/// <summary>
		/// Retrieve information about a JPEG image without decompressing it.
		/// </summary>
		/// <param name="jpegBuf">A buffer containing a JPEG image.</param>
		/// <param name="width">An output variable that will receive the width (in pixels) of the JPEG image.</param>
		/// <param name="height">An output variable that will receive the height (in pixels) of the JPEG image.</param>
		/// <param name="subsamp">An output variable that will receive the the level of chrominance subsampling used when compressing the JPEG image.</param>
		/// <param name="colorspace">An output variable that will receive one of the JPEG <see cref="Colorspace"/> constants, indicating the colorspace of the JPEG image.</param>
		public void DecompressHeader(byte[] jpegBuf, out int width, out int height, out ChrominanceSubsampling subsamp, out Colorspace colorspace)
		{
			if (jpegBuf == null)
				throw new ArgumentNullException(nameof (jpegBuf));

			int retval = Library.tjDecompressHeader3(_handle, jpegBuf, (uint)jpegBuf.Length, out width, out height, out subsamp, out colorspace);

			if (retval != 0)
				throw new Exception(Library.tjGetErrorStr());
		}

	}
}
