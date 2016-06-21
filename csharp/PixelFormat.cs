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
	/// Extensions class for the <see cref="PixelFormat"/> enumeration.
	/// </summary>
	public static class PixelFormatExtensions
	{
		/// <summary>
		/// Pixel size (in bytes) for a given pixel format.
		/// </summary>
		static readonly int[] tjPixelSize = {3, 3, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4};

		static readonly int[] tjRedOffset = { 0, 2, 0, 2, 3, 1, 0, 0, 2, 3, 1, -1 };

		static readonly int[] tjGreenOffset = { 1, 1, 1, 1, 2, 2, 0, 1, 1, 2, 2, -1 };

		static readonly int[] tjBlueOffset = { 2, 0, 2, 0, 1, 3, 0, 2, 0, 1, 3, -1 };

		/// <summary>
		/// Gets the number of bytes per pixel of this <see cref="PixelFormat"/>.
		/// </summary>
		/// <returns>The bytes per pixel.</returns>
		/// <param name="fmt">A pixel format.</param>
		public static int GetPixelSize(this PixelFormat fmt) { return tjPixelSize[(int)fmt]; }

		/// <summary>
		/// Gets the red offset (in bytes) for this pixel format.  This specifies the number
		/// of bytes that the red component is offset from the start of the pixel.  For
		/// instance, if a pixel of format <see cref="PixelFormat.BGRX"/> is stored in <c>byte pixel []</c>,
		/// then the red component will be <c>pixel [<see cref="PixelFormat.BGRX"/>.GetRedOffset()]</c>.
		/// </summary>
		public static int GetRedOffset(this PixelFormat fmt) { return tjRedOffset[(int)fmt]; }

		/// <summary>
		/// Gets the green offset (in bytes) for this pixel format.  This specifies the number
		/// of bytes that the green component is offset from the start of the pixel.  For
		/// instance, if a pixel of format <see cref="PixelFormat.BGRX"/> is stored in <c>byte pixel []</c>,
		/// then the red component will be <c>pixel [<see cref="PixelFormat.BGRX"/>.GetGreenOffset()]</c>.
		/// </summary>
		public static int GetGreenOffset(this PixelFormat fmt) { return tjGreenOffset[(int)fmt]; }

		/// <summary>
		/// Gets the blue offset (in bytes) for this pixel format.  This specifies the number
		/// of bytes that the blue component is offset from the start of the pixel.  For
		/// instance, if a pixel of format <see cref="PixelFormat.BGRX"/> is stored in <c>byte pixel []</c>,
		/// then the red component will be <c>pixel [<see cref="PixelFormat.BGRX"/>.GetBlueOffset()]</c>.
		/// </summary>
		public static int GetBlueOffset(this PixelFormat fmt) { return tjBlueOffset[(int)fmt]; }
	}

	/// <summary>
	/// Pixel formats.
	/// </summary>
	public enum PixelFormat
	{
		/// <summary>
		/// RGB pixel format.  The red, green, and blue components in the image are
		/// stored in 3-byte pixels in the order R, G, B from lowest to highest byte
		/// address within each pixel.
		/// </summary>
		RGB=0,

		/// <summary>
		/// BGR pixel format.  The red, green, and blue components in the image are
		/// stored in 3-byte pixels in the order B, G, R from lowest to highest byte
		/// address within each pixel.
		/// </summary>
		BGR,

		/// <summary>
		/// RGBX pixel format.  The red, green, and blue components in the image are
		/// stored in 4-byte pixels in the order R, G, B from lowest to highest byte
		/// address within each pixel.  The X component is ignored when compressing
		/// and undefined when decompressing.
		/// </summary>
		RGBX,

		/// <summary>
		/// BGRX pixel format.  The red, green, and blue components in the image are
		/// stored in 4-byte pixels in the order B, G, R from lowest to highest byte
		/// address within each pixel.  The X component is ignored when compressing
		/// and undefined when decompressing.
		/// </summary>
		BGRX,

		/// <summary>
		/// XBGR pixel format.  The red, green, and blue components in the image are
		/// stored in 4-byte pixels in the order R, G, B from highest to lowest byte
		/// address within each pixel.  The X component is ignored when compressing
		/// and undefined when decompressing.
		/// </summary>
		XBGR,

		/// <summary>
		/// XRGB pixel format.  The red, green, and blue components in the image are
		/// stored in 4-byte pixels in the order B, G, R from highest to lowest byte
		/// address within each pixel.  The X component is ignored when compressing
		/// and undefined when decompressing.
		/// </summary>
		XRGB,

		/// <summary>
		/// Grayscale pixel format.  Each 1-byte pixel represents a luminance
		/// (brightness) level from 0 to 255.
		/// </summary>
		GRAY,

		/// <summary>
		/// RGBA pixel format.  This is the same as <see cref="RGBX"/>, except that when
		/// decompressing, the X component is guaranteed to be 0xFF, which can be
		/// interpreted as an opaque alpha channel.
		/// </summary>
		RGBA,

		/// <summary>
		/// BGRA pixel format.  This is the same as <see cref="BGRX"/>, except that when
		/// decompressing, the X component is guaranteed to be 0xFF, which can be
		/// interpreted as an opaque alpha channel.
		/// </summary>
		BGRA,

		/// <summary>
		/// ABGR pixel format.  This is the same as <see cref="XBGR"/>, except that when
		/// decompressing, the X component is guaranteed to be 0xFF, which can be
		/// interpreted as an opaque alpha channel.
		/// </summary>
		ABGR,

		/// <summary>
		/// ARGB pixel format.  This is the same as <see cref="XRGB"/>, except that when
		/// decompressing, the X component is guaranteed to be 0xFF, which can be
		/// interpreted as an opaque alpha channel.
		/// </summary>
		ARGB,

		/// <summary>
		/// CMYK pixel format.  Unlike RGB, which is an additive color model used
		/// primarily for display, CMYK (Cyan/Magenta/Yellow/Key) is a subtractive
		/// color model used primarily for printing.  In the CMYK color model, the
		/// value of each color component typically corresponds to an amount of cyan,
		/// magenta, yellow, or black ink that is applied to a white background.  In
		/// order to convert between CMYK and RGB, it is necessary to use a color
		/// management system (CMS.)  A CMS will attempt to map colors within the
		/// printer's gamut to perceptually similar colors in the display's gamut and
		/// vice versa, but the mapping is typically not 1:1 or reversible, nor can it
		/// be defined with a simple formula.  Thus, such a conversion is out of scope
		/// for a codec library.  However, the TurboJPEG API allows for compressing
		/// CMYK pixels into a <see cref="Colorspace.YCCK"/> JPEG image and decompressing
		/// <see cref="Colorspace.YCCK"/> JPEG images into CMYK pixels.
		/// </summary>
		CMYK
	}
}

