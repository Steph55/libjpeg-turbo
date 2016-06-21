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
	/// JPEG colorspaces
	/// </summary>
	public enum Colorspace
	{
		/// <summary>
		/// RGB colorspace.  When compressing the JPEG image, the R, G, and B
		/// components in the source image are reordered into image planes, but no
		/// colorspace conversion or subsampling is performed.  RGB JPEG images can be
		/// decompressed to any of the extended RGB pixel formats or grayscale, but
		/// they cannot be decompressed to YUV images.
		/// </summary>
		RGB=0,

		/// <summary>
		/// YCbCr colorspace.  YCbCr is not an absolute colorspace but rather a
		/// mathematical transformation of RGB designed solely for storage and
		/// transmission.  YCbCr images must be converted to RGB before they can
		/// actually be displayed.  In the YCbCr colorspace, the Y (luminance)
		/// component represents the black &amp; white portion of the original image, and
		/// the Cb and Cr (chrominance) components represent the color portion of the
		/// original image.  Originally, the analog equivalent of this transformation
		/// allowed the same signal to drive both black &amp; white and color televisions,
		/// but JPEG images use YCbCr primarily because it allows the color data to be
		/// optionally subsampled for the purposes of reducing bandwidth or disk
		/// space.  YCbCr is the most common JPEG colorspace, and YCbCr JPEG images
		/// can be compressed from and decompressed to any of the extended RGB pixel
		/// formats or grayscale, or they can be decompressed to YUV planar images.
		/// </summary>
		YCbCr,

		/// <summary>
		/// Grayscale colorspace.  The JPEG image retains only the luminance data (Y
		/// component), and any color data from the source image is discarded.
		/// Grayscale JPEG images can be compressed from and decompressed to any of
		/// the extended RGB pixel formats or grayscale, or they can be decompressed
		/// to YUV planar images.
		/// </summary>
		GRAY,

		/// <summary>
		/// CMYK colorspace.  When compressing the JPEG image, the C, M, Y, and K
		/// components in the source image are reordered into image planes, but no
		/// colorspace conversion or subsampling is performed.  CMYK JPEG images can
		/// only be decompressed to CMYK pixels.
		/// </summary>
		CMYK,

		/// <summary>
		/// YCCK colorspace.  YCCK (AKA "YCbCrK") is not an absolute colorspace but
		/// rather a mathematical transformation of CMYK designed solely for storage
		/// and transmission.  It is to CMYK as YCbCr is to RGB.  CMYK pixels can be
		/// reversibly transformed into YCCK, and as with YCbCr, the chrominance
		/// components in the YCCK pixels can be subsampled without incurring major
		/// perceptual loss.  YCCK JPEG images can only be compressed from and
		/// decompressed to CMYK pixels.
		/// </summary>
		YCCK
	}
}
