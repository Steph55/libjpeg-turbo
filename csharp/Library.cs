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
	/// Contains entrypoints to the libjpeg-turbo library.
	/// </summary>
	/// <remarks>
	/// Due to the use of "unsigned long" in the C API, some functions in this class
	/// have the wrong prototypes on certain 64-bit systems, specifically, systems which
	/// use the LP64 data model. In practice, Linux and Unix systems (including Mac OS X)
	/// fall into this category.
	/// 
	/// The <see cref="Library_LP64"/> class contains variants of these functions for use on
	/// those systems.
	/// </remarks>
	static class Library
	{
		/// <summary>
		/// Pad the given width to the nearest 32-bit boundary.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <returns>The width, padded to the nearest 32-bit boundary.</returns>
		internal static int TJPAD(int width)
		{
			return ((width) + 3) & (~3);
		}

		/// <summary>
		/// Create a TurboJPEG compressor instance.
		/// </summary>
		/// <returns>a handle to the newly-created instance, or NULL if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		[DllImport("libjpeg-turbo.so")]
		internal static extern IntPtr tjInitCompress();

		/// <summary>
		/// Compress an RGB or grayscale image into a JPEG image.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG compressor or transformer instance.</param>
		/// <param name="srcBuf">pointer to an image buffer containing RGB or grayscale pixels to be compressed.</param>
		/// <param name="width">width (in pixels) of the source image.</param>
		/// <param name="pitch">
		/// bytes per line of the source image. Normally, this should be
		/// <paramref name="width"/> * <see cref="PixelFormatExtensions.GetPixelSize"/>(<paramref name="pixelFormat"/>) if the image is unpadded, or
		/// <see cref="TJPAD"/>(<paramref name="width"/> * <see cref="PixelFormatExtensions.GetPixelSize"/>(<paramref name="pixelFormat"/>)) if each line of the image
		/// is padded to the nearest 32-bit boundary, as is the case for Windows
		/// bitmaps.  You can also be clever and use this parameter to skip lines, etc.
		/// Setting this parameter to 0 is the equivalent of setting it to
		/// <paramref name="width"/> * <see cref="PixelFormatExtensions.GetPixelSize"/>(<paramref name="pixelFormat"/>).
		/// </param>
		/// <param name="height">height (in pixels) of the source image.</param>
		/// <param name="pixelFormat">pixel format of the source image (see <see cref="PixelFormat"/>.).</param>
		/// <param name="jpegBuf">
		/// address of a pointer to an image buffer that will receive the
		/// JPEG image.  TurboJPEG has the ability to reallocate the JPEG buffer to
		/// accommodate the size of the JPEG image.  Thus, you can choose to:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// pre-allocate the JPEG buffer with an arbitrary size using
		/// <see cref="tjAlloc"/> and let TurboJPEG grow the buffer as needed,
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// set <paramref name="jpegBuf"/> to NULL to tell TurboJPEG to allocate the buffer
		/// for you, or
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// pre-allocate the buffer to a "worst case" size determined by calling
		/// <see cref="tjBufSize"/>.  This should ensure that the buffer never has to be
		/// re-allocated (setting <see cref="FunctionFlags.NoRealloc"/> guarantees this.)
		/// </description>
		/// </item>
		/// </list>
		/// If you choose option 1, <paramref name="jpegSize"/> should be set to the size of your
		/// pre-allocated buffer.  In any case, unless you have set <see cref="FunctionFlags.NoRealloc"/>,
		/// you should always check <c>jpegBuf</c> upon return from this function, as
		/// it may have changed.
		/// </param>
		/// <param name="jpegSize">
		/// pointer to an unsigned long variable that holds the size of
		/// the JPEG image buffer.  If <paramref name="jpegBuf"/> points to a pre-allocated
		/// buffer, then <c>jpegSize</c> should be set to the size of the buffer.
		/// Upon return, <c>jpegSize</c> will contain the size of the JPEG image (in
		/// bytes.)  If <paramref name="jpegBuf"/> points to a JPEG image buffer that is being
		/// reused from a previous call to one of the JPEG compression functions, then
		/// <c>jpegSize</c> is ignored.
		/// </param>
		/// <param name="jpegSubsamp">
		/// the level of chrominance subsampling to be used when
		/// generating the JPEG image (see <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <param name="jpegQual">the image quality of the generated JPEG image (1 = worst, 100 = best)</param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjCompress2 (tjhandle handle, const unsigned char* srcBuf,
		///   int width, int pitch, int height, int pixelFormat, unsigned char** jpegBuf,
		///   unsigned long* jpegSize, int jpegSubsamp, int jpegQual, int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjCompress2(IntPtr handle, byte[] srcBuf,
			int width, int pitch, int height, PixelFormat pixelFormat, ref IntPtr jpegBuf,
			ref uint jpegSize, ChrominanceSubsampling jpegSubsamp, int jpegQual, FunctionFlags flags);

		/// <summary>Compress a YUV planar image into a JPEG image.</summary>
		/// <param name="handle">a handle to a TurboJPEG compressor or transformer instance</param>
		/// <param name="srcBuf">
		/// pointer to an image buffer containing a YUV planar image to be
		/// compressed.  The size of this buffer should match the value returned by
		/// <see cref="tjBufSizeYUV2"/> for the given image width, height, padding, and level of
		/// chrominance subsampling.  The Y, U (Cb), and V (Cr) image planes should be
		/// stored sequentially in the source buffer (refer to @ref YUVnotes
		/// "YUV Image Format Notes".)
		/// </param>
		/// <param name="width">
		/// width (in pixels) of the source image.  If the width is not an
		/// even multiple of the MCU block width (see <see cref="ChrominanceSubsamplingExtensions.GetMCUWidth"/>), then an intermediate
		/// buffer copy will be performed within TurboJPEG.
		/// </param>
		/// <param name="pad">
		/// the line padding used in the source image.  For instance, if each
		/// line in each plane of the YUV image is padded to the nearest multiple of 4
		/// bytes, then <c>pad</c> should be set to 4.
		/// </param>
		/// <param name="height">
		/// height (in pixels) of the source image.  If the height is not
		/// an even multiple of the MCU block height (see <see cref="ChrominanceSubsamplingExtensions.GetMCUHeight"/>), then an
		/// intermediate buffer copy will be performed within TurboJPEG.
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling used in the source
		/// image (see <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <param name="jpegBuf">
		/// address of a pointer to an image buffer that will receive the
		/// JPEG image.  TurboJPEG has the ability to reallocate the JPEG buffer to
		/// accommodate the size of the JPEG image.  Thus, you can choose to:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// pre-allocate the JPEG buffer with an arbitrary size using
		/// <see cref="tjAlloc"/> and let TurboJPEG grow the buffer as needed,
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// set <paramref name="jpegBuf"/> to NULL to tell TurboJPEG to allocate the buffer
		/// for you, or
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// pre-allocate the buffer to a "worst case" size determined by calling
		/// <see cref="tjBufSize"/>.  This should ensure that the buffer never has to be
		/// re-allocated (setting <see cref="FunctionFlags.NoRealloc"/> guarantees this.)		/// </item>
		/// </description>
		/// </item>
		/// </list>
		/// If you choose option 1, <paramref name="jpegSize"/> should be set to the size of your
		/// pre-allocated buffer.  In any case, unless you have set <see cref="FunctionFlags.NoRealloc"/>,
		/// you should always check <c>jpegBuf</c> upon return from this function, as
		/// it may have changed.
		/// </param>
		/// <param name="jpegSize">
		/// pointer to an unsigned long variable that holds the size of
		/// the JPEG image buffer.  If <paramref name="jpegBuf"/> points to a pre-allocated
		/// buffer, then <c>jpegSize</c> should be set to the size of the buffer.
		/// Upon return, <c>jpegSize</c> will contain the size of the JPEG image (in
		/// bytes.)  If <paramref name="jpegBuf"/> points to a JPEG image buffer that is being
		/// reused from a previous call to one of the JPEG compression functions, then
		/// <c>jpegSize</c> is ignored.
		/// </param>
		/// <param name="jpegQual">The image quality of the generated JPEG image (1 = worst, 100 = best)</param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjCompressFromYUV(tjhandle handle,
		///   const unsigned char* srcBuf, int width, int pad, int height, int subsamp,
		///   unsigned char** jpegBuf, unsigned long* jpegSize, int jpegQual, int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjCompressFromYUV(
			IntPtr handle,
			byte[] srcBuf, int width, int pad, int height, ChrominanceSubsampling subsamp,
			ref IntPtr jpegBuf, ref uint jpegSize, int jpegQual, FunctionFlags flags);

		/// <summary>
		/// Compress a set of Y, U (Cb), and V (Cr) image planes into a JPEG image.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG compressor or transformer instance</param>
		/// <param name="srcPlanes">
		/// an array of pointers to Y, U (Cb), and V (Cr) image planes
		/// (or just a Y plane, if compressing a grayscale image) that contain a YUV
		/// image to be compressed.  These planes can be contiguous or non-contiguous in
		/// memory.  The size of each plane should match the value returned by
		/// <see cref="tjPlaneSizeYUV"/> for the given image width, height, strides, and level of
		/// chrominance subsampling.  Refer to @ref YUVnotes "YUV Image Format Notes"
		/// for more details.
		/// </param>
		/// <param name="width">
		/// width (in pixels) of the source image.  If the width is not an
		/// even multiple of the MCU block width (see <see cref="ChrominanceSubsamplingExtensions.GetMCUWidth"/>), then an intermediate
		/// buffer copy will be performed within TurboJPEG.
		/// </param>
		/// <param name="strides">
		/// an array of integers, each specifying the number of bytes per
		/// line in the corresponding plane of the YUV source image.  Setting the stride
		/// for any plane to 0 is the same as setting it to the plane width (see
		/// @ref YUVnotes "YUV Image Format Notes".)  If <c>strides</c> is NULL, then
		/// the strides for all planes will be set to their respective plane widths.
		/// You can adjust the strides in order to specify an arbitrary amount of line
		/// padding in each plane or to create a JPEG image from a subregion of a larger
		/// YUV planar image.
		/// </param>
		/// <param name="height">
		/// height (in pixels) of the source image.  If the height is not
		/// an even multiple of the MCU block height (see <see cref="ChrominanceSubsamplingExtensions.GetMCUHeight"/>), then an
		/// intermediate buffer copy will be performed within TurboJPEG.
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling used in the source
		/// image (see <see cref="ChrominanceSubsampling"/>.) 
		/// </param>
		/// <param name="jpegBuf">
		/// address of a pointer to an image buffer that will receive the
		/// JPEG image.  TurboJPEG has the ability to reallocate the JPEG buffer to
		/// accommodate the size of the JPEG image.  Thus, you can choose to:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// pre-allocate the JPEG buffer with an arbitrary size using
		/// <see cref="tjAlloc"/> and let TurboJPEG grow the buffer as needed,
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// set <paramref name="jpegBuf"/> to NULL to tell TurboJPEG to allocate the buffer
		/// for you, or
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// pre-allocate the buffer to a "worst case" size determined by calling
		/// <see cref="tjBufSize"/>.  This should ensure that the buffer never has to be
		/// re-allocated (setting <see cref="FunctionFlags.NoRealloc"/> guarantees this.)		/// </item>
		/// </description>
		/// </item>
		/// </list>
		/// If you choose option 1, <paramref name="jpegSize"/> should be set to the size of your
		/// pre-allocated buffer.  In any case, unless you have set <see cref="FunctionFlags.NoRealloc"/>,
		/// you should always check <c>jpegBuf</c> upon return from this function, as
		/// it may have changed.
		/// </param>
		/// <param name="jpegSize">
		/// Pointer to an unsigned long variable that holds the size of
		/// the JPEG image buffer.  If <paramref name="jpegBuf"/> points to a pre-allocated
		/// buffer, then <c>jpegSize</c> should be set to the size of the buffer.
		/// Upon return, <c>jpegSize</c> will contain the size of the JPEG image (in
		/// bytes.)  If <paramref name="jpegBuf"/> points to a JPEG image buffer that is being
		/// reused from a previous call to one of the JPEG compression functions, then
		/// <c>jpegSize</c> is ignored.
		/// </param>
		/// <param name="jpegQual">The image quality of the generated JPEG image (1 = worst, 100 = best)</param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjCompressFromYUVPlanes(tjhandle handle,
		///   const unsigned char** srcPlanes, int width, const int* strides, int height,
		///   int subsamp, unsigned char** jpegBuf, unsigned long* jpegSize, int jpegQual,
		///   int flags);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjCompressFromYUVPlanes(
			IntPtr handle,
			IntPtr [] srcPlanes, int width, int [] strides, int height,
			ChrominanceSubsampling subsamp, ref IntPtr jpegBuf, ref uint jpegSize, int jpegQual,
			FunctionFlags flags);

		/// <summary>
		/// The maximum size of the buffer (in bytes) required to hold a JPEG image with
		/// the given parameters.  The number of bytes returned by this function is
		/// larger than the size of the uncompressed source image.  The reason for this
		/// is that the JPEG format uses 16-bit coefficients, and it is thus possible
		/// for a very high-quality JPEG image with very high-frequency content to
		/// expand rather than compress when converted to the JPEG format.  Such images
		/// represent a very rare corner case, but since there is no way to predict the
		/// size of a JPEG image prior to compression, the corner case has to be
		/// handled.
		/// </summary>
		/// <param name="width">width (in pixels) of the image</param>
		/// <param name="height">height (in pixels) of the image</param>
		/// <param name="jpegSubsamp">
		/// the level of chrominance subsampling to be used when
		/// generating the JPEG image (see <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <returns>
		/// the maximum size of the buffer (in bytes) required to hold the
		/// image, or -1 if the arguments are out of bounds.
		/// </returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT unsigned long DLLCALL tjBufSize(int width, int height,
		///   int jpegSubsamp);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern uint tjBufSize(int width, int height,
		                                      ChrominanceSubsampling jpegSubsamp);

		/// <summary>
		/// The size of the buffer (in bytes) required to hold a YUV planar image with
		/// the given parameters.
		/// </summary>
		/// <param name="width">width (in pixels) of the image</param>
		/// <param name="pad">
		/// the width of each line in each plane of the image is padded to
		/// the nearest multiple of this number of bytes (must be a power of 2.)
		/// </param>
		/// <param name="height">height (in pixels) of the image</param>
		/// <param name="subsamp">
		/// level of chrominance subsampling in the image (see
		/// <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <returns>
		/// the size of the buffer (in bytes) required to hold the image, or
		/// -1 if the arguments are out of bounds.
		/// </returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT unsigned long DLLCALL tjBufSizeYUV2(int width, int pad, int height,
		///   int subsamp);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern uint tjBufSizeYUV2(int width, int pad, int height,
		                                          ChrominanceSubsampling subsamp);

		/// <summary>
		/// The size of the buffer (in bytes) required to hold a YUV image plane with
		/// the given parameters.
		/// </summary>
		/// <param name="componentID">ID number of the image plane (0 = Y, 1 = U/Cb, 2 = V/Cr)</param>
		/// <param name="width">
		/// width (in pixels) of the YUV image.  NOTE: this is the width of
		/// the whole image, not the plane width.
		/// </param>
		/// <param name="stride">
		/// bytes per line in the image plane.  Setting this to 0 is the
		/// equivalent of setting it to the plane width.
		/// </param>
		/// <param name="height">
		/// height (in pixels) of the YUV image.  NOTE: this is the height
		/// of the whole image, not the plane height.
		/// </param>
		/// <param name="subsamp">
		/// level of chrominance subsampling in the image (see
		/// <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <returns>
		/// the size of the buffer (in bytes) required to hold the YUV image
		/// plane, or -1 if the arguments are out of bounds.
		/// </returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT unsigned long DLLCALL tjPlaneSizeYUV(int componentID, int width,
		///   int stride, int height, int subsamp);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern uint tjPlaneSizeYUV(
			int componentID, int width,
			int stride, int height, ChrominanceSubsampling subsamp);

		/// <summary>
		/// The plane width of a YUV image plane with the given parameters.  Refer to
		/// @ref YUVnotes "YUV Image Format Notes" for a description of plane width.
		/// </summary>
		/// <param name="componentID">ID number of the image plane (0 = Y, 1 = U/Cb, 2 = V/Cr)</param>
		/// <param name="width">width (in pixels) of the YUV image</param>
		/// <param name="subsamp">
		/// level of chrominance subsampling in the image (see
		/// <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <returns>
		/// the plane width of a YUV image plane with the given parameters, or
		/// -1 if the arguments are out of bounds.
		/// </returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int tjPlaneWidth(int componentID, int width, int subsamp);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjPlaneWidth(int componentID, int width, ChrominanceSubsampling subsamp);

		/// <summary>
		/// The plane height of a YUV image plane with the given parameters.  Refer to
		/// @ref YUVnotes "YUV Image Format Notes" for a description of plane height.
		/// </summary>
		/// <param name="componentID">ID number of the image plane (0 = Y, 1 = U/Cb, 2 = V/Cr)</param>
		/// <param name="height">height (in pixels) of the YUV image</param>
		/// <param name="subsamp">
		/// level of chrominance subsampling in the image (see
		/// <see cref="ChrominanceSubsampling"/>.)
		/// </param>
		/// <returns>
		/// the plane height of a YUV image plane with the given parameters, or
		/// -1 if the arguments are out of bounds.
		/// </returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int tjPlaneHeight(int componentID, int height, int subsamp);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjPlaneHeight(int componentID, int height, ChrominanceSubsampling subsamp);

		/// <summary>
		/// Encode an RGB or grayscale image into a YUV planar image.  This function
		/// uses the accelerated color conversion routines in the underlying
		/// codec but does not execute any of the other steps in the JPEG compression
		/// process.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG compressor or transformer instance.</param>
		/// <param name="srcBuf">
		/// pointer to an image buffer containing RGB or grayscale pixels
		/// to be encoded
		/// </param>
		/// <param name="width">width (in pixels) of the source image.</param>
		/// <param name="pitch">
		/// bytes per line in the source image.  Normally, this should be
		/// <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>() if the image is unpadded, or
		/// <see cref="TJPAD"/>(<paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()) if each line of the image
		/// is padded to the nearest 32-bit boundary, as is the case for Windows
		/// bitmaps.  You can also be clever and use this parameter to skip lines, etc.
		/// Setting this parameter to 0 is the equivalent of setting it to
		/// <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>().
		/// </param>
		/// <param name="height">height (in pixels) of the source image.</param>
		/// <param name="pixelFormat">pixel format of the source image (see <see cref="PixelFormat"/>.)</param>
		/// <param name="dstBuf">
		/// pointer to an image buffer that will receive the YUV image.
		/// Use <see cref="tjBufSizeYUV2"/> to determine the appropriate size for this buffer
		/// based on the image width, height, and level of chrominance subsampling.
		/// The Y, U (Cb), and V (Cr) image planes will be stored sequentially in the
		/// buffer (refer to @ref YUVnotes "YUV Image Format Notes".)
		/// </param>
		/// <param name="pad">
		/// pad the width of each line in each plane of the YUV image will be
		/// padded to the nearest multiple of this number of bytes (must be a power of
		/// 2.)  To generate images suitable for X Video, <c>pad</c> should be set to
		/// 4.
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling to be used when
		/// generating the YUV image (see <see cref="ChrominanceSubsampling"/>
		/// "Chrominance subsampling options".)  To generate images suitable for X
		/// Video, <c>subsamp</c> should be set to <see cref="ChrominanceSubsampling.SAMP_420"/>.  This produces an
		/// image compatible with the I420 (AKA "YUV420P") format.
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjEncodeYUV3(tjhandle handle,
		///   const unsigned char* srcBuf, int width, int pitch, int height,
		///   int pixelFormat, unsigned char* dstBuf, int pad, int subsamp, int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjEncodeYUV3(IntPtr handle,
			IntPtr srcBuf, int width, int pitch, int height, PixelFormat pixelFormat,
			IntPtr dstBuf, int pad, ChrominanceSubsampling subsamp, FunctionFlags flags);

		/// <summary>
		/// Encode an RGB or grayscale image into separate Y, U (Cb), and V (Cr) image
		/// planes.  This function uses the accelerated color conversion routines in the
		/// underlying codec but does not execute any of the other steps in the JPEG
		/// compression process.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG compressor or transformer instance</param>
		/// <param name="srcBuf">
		/// pointer to an image buffer containing RGB or grayscale pixels
		/// to be encoded
		/// </param>
		/// <param name="width">width (in pixels) of the source image</param>
		/// <param name="pitch">
		/// bytes per line in the source image.  Normally, this should be
		/// <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>() if the image is unpadded, or
		/// <see cref="TJPAD"/>(<paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()) if each line of the image
		/// is padded to the nearest 32-bit boundary, as is the case for Windows
		/// bitmaps.  You can also be clever and use this parameter to skip lines, etc.
		/// Setting this parameter to 0 is the equivalent of setting it to
		/// <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>().
		/// </param>
		/// <param name="height">height (in pixels) of the source image</param>
		/// <param name="pixelFormat">pixel format of the source image (see <see cref="PixelFormat"/>.)</param>
		/// <param name="dstPlanes">
		/// an array of pointers to Y, U (Cb), and V (Cr) image planes
		/// (or just a Y plane, if generating a grayscale image) that will receive the
		/// encoded image.These planes can be contiguous or non-contiguous in memory.
		/// Use <see cref="tjPlaneSizeYUV"/> to determine the appropriate size for each plane based
		/// on the image width, height, strides, and level of chrominance subsampling.
		/// Refer to @ref YUVnotes "YUV Image Format Notes" for more details.
		/// </param>
		/// <param name="strides">
		/// an array of integers, each specifying the number of bytes per
		/// line in the corresponding plane of the output image.  Setting the stride for
		/// any plane to 0 is the same as setting it to the plane width (see
		/// @ref YUVnotes "YUV Image Format Notes".)  If <c>strides</c> is <c>null</c>, then
		/// the strides for all planes will be set to their respective plane widths.
		/// You can adjust the strides in order to add an arbitrary amount of line
		/// padding to each plane or to encode an RGB or grayscale image into a
		/// subregion of a larger YUV planar image.
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling to be used when
		/// generating the YUV image (see <see cref="ChrominanceSubsampling"/>
		/// "Chrominance subsampling options".)  To generate images suitable for X
		/// Video, <c>subsamp</c> should be set to <see cref="ChrominanceSubsampling.SAMP_420"/>.  This produces an
		/// image compatible with the I420 (AKA "YUV420P") format.
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjEncodeYUVPlanes (tjhandle handle,
		///   const unsigned char* srcBuf, int width, int pitch, int height,
		///   int pixelFormat, unsigned char** dstPlanes, int* strides, int subsamp,
		///   int flags);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjEncodeYUVPlanes(
			IntPtr handle,
			byte[] srcBuf, int width, int pitch, int height,
			PixelFormat pixelFormat, byte[][] dstPlanes, int[] strides, ChrominanceSubsampling subsamp,
			FunctionFlags flags);

		/// <summary>
		/// Create a TurboJPEG decompressor instance.
		/// </summary>
		/// <returns>a handle to the newly-created instance, or NULL if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		[DllImport("libjpeg-turbo.so")]
		internal static extern IntPtr tjInitDecompress();

		/// <summary>
		/// Retrieve information about a JPEG image without decompressing it.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance.</param>
		/// <param name="jpegBuf">pointer to a buffer containing a JPEG image.</param>
		/// <param name="jpegSize">size of the JPEG image (in bytes).</param>
		/// <param name="width">an integer variable that will receive the width (in pixels) of the JPEG image.</param>
		/// <param name="height">an integer variable that will receive the height (in pixels) of the JPEG image.</param>
		/// <param name="jpegSubsamp">
		/// an integer variable that will receive the
		/// level of chrominance subsampling used when the JPEG image was compressed
		/// (see <see cref="ChrominanceSubsampling"/> .)
		/// </param>
		/// <param name="jpegColorspace">
		/// an integer variable that will receive one
		/// of the JPEG colorspace constants, indicating the colorspace of the JPEG
		/// image (see <see cref="Colorspace"/>.)
		/// </param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecompressHeader3(tjhandle handle,
		///   const unsigned char* jpegBuf, unsigned long jpegSize, int* width,
		///   int* height, int* jpegSubsamp, int* jpegColorspace);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjDecompressHeader3(IntPtr handle,
			byte[] jpegBuf, uint jpegSize, out int width, out int height,
			out ChrominanceSubsampling jpegSubsamp, out Colorspace jpegColorspace);

		/// <summary>
		/// Returns a list of fractional scaling factors that the JPEG decompressor in
		/// this implementation of TurboJPEG supports.
		/// </summary>
		/// <param name="numscalingfactors">pointer to an integer variable that will receive the number of elements in the list.</param>
		/// <returns>a pointer to a list of fractional scaling factors, or NULL if an error is encountered (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT tjscalingfactor* DLLCALL tjGetScalingFactors(int *numscalingfactors);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		[return : MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
		internal static extern ScalingFactor[] tjGetScalingFactors(out int numscalingfactors);

		/// <summary>
		/// Decompress a JPEG image to an RGB, grayscale, or CMYK image.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance.</param>
		/// <param name="jpegBuf">pointer to a buffer containing the JPEG image to decompress.</param>
		/// <param name="jpegSize">size of the JPEG image (in bytes).</param>
		/// <param name="dstBuf">
		/// pointer to an image buffer that will receive the decompressed
		/// image.  This buffer should normally be <c><paramref name="pitch"/> * scaledHeight</c>
		/// bytes in size, where <c>scaledHeight</c> can be determined by
		/// calling #TJSCALED() with the JPEG image height and one of the scaling
		/// factors returned by <see cref="tjGetScalingFactors"/>.  The <c>dstBuf</c>
		/// pointer may also be used to decompress into a specific region of a
		/// larger buffer.
		/// </param>
		/// <param name="width">
		/// desired width (in pixels) of the destination image.  If this is
		/// different than the width of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired width.  If <c>width</c> is
		/// set to 0, then only the height will be considered when determining the
		/// scaled image size.
		/// </param>
		/// <param name="pitch">
		/// bytes per line of the destination image.  Normally, this is
		/// <c>scaledWidth * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()</c> if the decompressed
		/// image is unpadded, else <c><see cref="TJPAD"/>(scaledWidth *
		/// <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>())</c> if each line of the decompressed
		/// image is padded to the nearest 32-bit boundary, as is the case for
		/// Windows bitmaps.  (NOTE: <c>scaledWidth</c> can be determined by
		/// calling #TJSCALED() with the JPEG image width and one of the scaling
		/// factors returned by <see cref="tjGetScalingFactors"/>.)  You can also be clever
		/// and use the pitch parameter to skip lines, etc.  Setting this
		/// parameter to 0 is the equivalent of setting it to <c>scaledWidth
		/// * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()</c>.
		/// </param>
		/// <param name="height">
		/// desired height (in pixels) of the destination image.  If this
		/// is different than the height of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired height.  If <c>height</c>
		/// is set to 0, then only the width will be considered when determining the
		/// scaled image size.
		/// </param>
		/// <param name="pixelFormat">pixel format of the destination image (see <see cref="PixelFormat"/>.)</param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecompress2(tjhandle handle,
		///   const unsigned char* jpegBuf, unsigned long jpegSize, unsigned char* dstBuf,
		///   int width, int pitch, int height, int pixelFormat, int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjDecompress2(IntPtr handle,
			byte[] jpegBuf, uint jpegSize, byte[] dstBuf,
			int width, int pitch, int height, PixelFormat pixelFormat, FunctionFlags flags);

		/// <summary>
		/// Decompress a JPEG image to a YUV planar image.  This function performs JPEG
		/// decompression but leaves out the color conversion step, so a planar YUV
		/// image is generated instead of an RGB image.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance.</param>
		/// <param name="jpegBuf">pointer to a buffer containing the JPEG image to decompress.</param>
		/// <param name="jpegSize">size of the JPEG image (in bytes).</param>
		/// <param name="dstBuf">
		/// pointer to an image buffer that will receive the YUV image.
		/// Use <see cref="tjBufSizeYUV2"/> to determine the appropriate size for this buffer based
		/// on the image width, height, padding, and level of subsampling.  The Y,
		/// U (Cb), and V (Cr) image planes will be stored sequentially in the buffer
		/// (refer to @ref YUVnotes "YUV Image Format Notes".)
		/// </param>
		/// <param name="width">
		/// width desired width (in pixels) of the YUV image.  If this is
		/// different than the width of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired width.  If <c>width</c> is
		/// set to 0, then only the height will be considered when determining the
		/// scaled image size.  If the scaled width is not an even multiple of the MCU
		/// block width (see #tjMCUWidth), then an intermediate buffer copy will be
		/// performed within TurboJPEG.
		/// </param>
		/// <param name="pad">
		/// the width of each line in each plane of the YUV image will be
		/// padded to the nearest multiple of this number of bytes (must be a power of
		/// 2.)  To generate images suitable for X Video, <tt>pad</tt> should be set to
		/// 4.
		/// </param>
		/// <param name="height">
		/// desired height (in pixels) of the YUV image.  If this is
		/// different than the height of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired height.If<tt>height</tt>
		/// is set to 0, then only the width will be considered when determining the
		/// scaled image size.If the scaled height is not an even multiple of the MCU
		/// block height (see #tjMCUHeight), then an intermediate buffer copy will be
		/// performed within TurboJPEG.
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecompressToYUV2(tjhandle handle,
		///   const unsigned char* jpegBuf, unsigned long jpegSize, unsigned char* dstBuf,
		///   int width, int pad, int height, int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjDecompressToYUV2(IntPtr handle,
			byte[] jpegBuf, uint jpegSize, byte[] dstBuf,
			int width, int pad, int height,
			FunctionFlags flags);

		/// <summary>
		/// Decompress a JPEG image into separate Y, U (Cb), and V (Cr) image
		/// planes.  This function performs JPEG decompression but leaves out the color
		/// conversion step, so a planar YUV image is generated instead of an RGB image.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance.</param>
		/// <param name="jpegBuf">pointer to a buffer containing the JPEG image to decompress</param>
		/// <param name="jpegSize">size of the JPEG image (in bytes)</param>
		/// <param name="dstPlanes">
		/// an array of pointers to Y, U (Cb), and V (Cr) image planes
		/// (or just a Y plane, if decompressing a grayscale image) that will receive
		/// the YUV image.These planes can be contiguous or non-contiguous in memory.
		/// Use <see cref="tjPlaneSizeYUV"/> to determine the appropriate size for each plane based
		/// on the scaled image width, scaled image height, strides, and level of
		/// chrominance subsampling.Refer to @ref YUVnotes "YUV Image Format Notes"
		/// for more details.
		/// </param>
		/// <param name="width">
		/// desired width (in pixels) of the YUV image.  If this is
		/// different than the width of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired width.  If <c>width</c> is
		/// set to 0, then only the height will be considered when determining the
		/// scaled image size.  If the scaled width is not an even multiple of the MCU
		/// block width (see #tjMCUWidth), then an intermediate buffer copy will be
		/// performed within TurboJPEG.
		/// </param>
		/// <param name="strides">
		/// an array of integers, each specifying the number of bytes per
		/// line in the corresponding plane of the output image.  Setting the stride for
		/// any plane to 0 is the same as setting it to the scaled plane width (see
		/// @ref YUVnotes "YUV Image Format Notes".)  If <c>strides</c> is NULL, then
		/// the strides for all planes will be set to their respective scaled plane
		/// widths.  You can adjust the strides in order to add an arbitrary amount of
		/// line padding to each plane or to decompress the JPEG image into a subregion
		/// of a larger YUV planar image.
		/// </param>
		/// <param name="height">
		/// desired height (in pixels) of the YUV image.  If this is
		/// different than the height of the JPEG image being decompressed, then
		/// TurboJPEG will use scaling in the JPEG decompressor to generate the largest
		/// possible image that will fit within the desired height.If<tt>height</tt>
		/// is set to 0, then only the width will be considered when determining the
		/// scaled image size.If the scaled height is not an even multiple of the MCU
		/// block height (see #tjMCUHeight), then an intermediate buffer copy will be
		/// performed within TurboJPEG.
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecompressToYUVPlanes (tjhandle handle,
		///   const unsigned char* jpegBuf, unsigned long jpegSize,
		///   unsigned char** dstPlanes, int width, int* strides, int height, int flags);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjDecompressToYUVPlanes(
			IntPtr handle,
			byte[] jpegBuf, uint jpegSize,
			byte[][] dstPlanes, int width, int[] strides, int height, FunctionFlags flags);

		/// <summary>
		/// Decode a YUV planar image into an RGB or grayscale image.  This function
		/// uses the accelerated color conversion routines in the underlying
		/// codec but does not execute any of the other steps in the JPEG decompression
		/// process.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance</param>
		/// <param name="srcBuf">
		/// pointer to an image buffer containing a YUV planar image to be
		/// decoded.The size of this buffer should match the value returned by
		/// <see cref="tjBufSizeYUV2"/> for the given image width, height, padding, and level of
		/// chrominance subsampling.  The Y, U (Cb), and V (Cr) image planes should be
		/// stored sequentially in the source buffer (refer to @ref YUVnotes
		/// "YUV Image Format Notes".)
		/// </param>
		/// <param name="pad">
		/// Use this parameter to specify that the width of each line in each
		/// plane of the YUV source image is padded to the nearest multiple of this
		/// number of bytes (must be a power of 2.)
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling used in the YUV source
		/// image (see @ref TJSAMP "Chrominance subsampling options".)
		/// </param>
		/// <param name="dstBuf">
		/// pointer to an image buffer that will receive the decoded
		/// image.  This buffer should normally be <paramref name="pitch"/> * <paramref name="height"/> bytes in
		/// size, but the <c>dstBuf</c> pointer can also be used to decode into a
		/// specific region of a larger buffer.
		/// </param>
		/// <param name="width">width (in pixels) of the source and destination images</param>
		/// <param name="pitch">
		/// bytes per line in the destination image.  Normally, this should
		/// be <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>() if the destination image is
		/// unpadded, or <see cref="TJPAD"/>(<paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()) if each line
		/// of the destination image should be padded to the nearest 32-bit boundary, as
		/// is the case for Windows bitmaps.You can also be clever and use the pitch
		/// parameter to skip lines, etc.  Setting this parameter to 0 is the equivalent
		/// of setting it to <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>().
		/// </param>
		/// <param name="height">height (in pixels) of the source and destination images</param>
		/// <param name="pixelFormat">
		/// pixel format of the destination image (see @ref TJPF "Pixel formats".)
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecodeYUV (tjhandle handle, const unsigned char* srcBuf,
		///   int pad, int subsamp, unsigned char* dstBuf, int width, int pitch,
		///   int height, int pixelFormat, int flags);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjDecodeYUV(
			IntPtr handle, byte[] srcBuf,
			int pad, ChrominanceSubsampling subsamp, IntPtr dstBuf, int width, int pitch,
			int height, PixelFormat pixelFormat, FunctionFlags flags);

		/// <summary>
		/// Decode a set of Y, U (Cb), and V (Cr) image planes into an RGB or grayscale
		/// image.  This function uses the accelerated color conversion routines in the
		/// underlying codec but does not execute any of the other steps in the JPEG
		/// decompression process.
		/// </summary>
		/// <param name="handle">a handle to a TurboJPEG decompressor or transformer instance</param>
		/// <param name="srcPlanes">
		/// an array of pointers to Y, U (Cb), and V (Cr) image planes
		/// (or just a Y plane, if decoding a grayscale image) that contain a YUV image
		/// to be decoded.These planes can be contiguous or non-contiguous in memory.
		/// The size of each plane should match the value returned by <see cref="tjPlaneSizeYUV"/>
		/// for the given image width, height, strides, and level of chrominance
		/// subsampling.  Refer to @ref YUVnotes "YUV Image Format Notes" for more
		/// details.
		/// </param>
		/// <param name="strides">
		/// an array of integers, each specifying the number of bytes per
		/// line in the corresponding plane of the YUV source image.Setting the stride
		/// for any plane to 0 is the same as setting it to the plane width (see
		/// @ref YUVnotes "YUV Image Format Notes".)  If <c>strides</c> is NULL, then
		/// the strides for all planes will be set to their respective plane widths.
		/// You can adjust the strides in order to specify an arbitrary amount of line
		/// padding in each plane or to decode a subregion of a larger YUV planar image.
		/// </param>
		/// <param name="subsamp">
		/// the level of chrominance subsampling used in the YUV source
		/// image (see <see cref="ChrominanceSubsampling"/> "Chrominance subsampling options".)
		/// </param>
		/// <param name="dstBuf">
		/// pointer to an image buffer that will receive the decoded
		/// image.  This buffer should normally be <paramref name="pitch"/> * <paramref name="height"/> bytes in
		/// size, but the <c>dstBuf</c> pointer can also be used to decode into a
		/// specific region of a larger buffer.
		/// </param>
		/// <param name="width">width (in pixels) of the source and destination images</param>
		/// <param name="pitch">
		/// bytes per line in the destination image.  Normally, this should
		/// be <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>() if the destination image is
		/// unpadded, or <see cref="TJPAD"/>(<paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>()) if each line
		/// of the destination image should be padded to the nearest 32-bit boundary, as
		/// is the case for Windows bitmaps.You can also be clever and use the pitch
		/// parameter to skip lines, etc.  Setting this parameter to 0 is the equivalent
		/// of setting it to <paramref name="width"/> * <paramref name="pixelFormat"/>.<see cref="PixelFormatExtensions.GetPixelSize"/>().
		/// </param>
		/// <param name="height">height (in pixels) of the source and destination images</param>
		/// <param name="pixelFormat">
		/// pixel format of the destination image (see <see cref="PixelFormat"/>.)
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDecodeYUVPlanes (tjhandle handle,
		///   const unsigned char** srcPlanes, const int* strides, int subsamp,
		///   unsigned char* dstBuf, int width, int pitch, int height, int pixelFormat,
		///   int flags);
		/// </remarks>
		[DllImport ("libjpeg-turbo.so")]
		internal static extern int tjDecodeYUVPlanes (
			IntPtr handle,
			byte[][] srcPlanes, int[] strides, ChrominanceSubsampling subsamp,
			byte[] dstBuf, int width, int pitch, int height, PixelFormat pixelFormat,
			FunctionFlags flags);

		/// <summary>
		/// Create a new TurboJPEG transformer instance.
		/// </summary>
		/// <returns>a handle to the newly-created instance, or NULL if an error occurred (see <see cref="tjGetErrorStr"/>.).</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT tjhandle DLLCALL tjInitTransform(void);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern IntPtr tjInitTransform();

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
		/// <param name="handle">a handle to a TurboJPEG transformer instance.</param>
		/// <param name="jpegBuf">pointer to a buffer containing the JPEG image to transform</param>
		/// <param name="jpegSize">size of the JPEG image (in bytes)</param>
		/// <param name="n">the number of transformed JPEG images to generate</param>
		/// <param name="dstBufs">
		/// pointer to an array of <paramref name="n"/> image buffers.  <paramref name="dstBufs"/>[i]
		/// will receive a JPEG image that has been transformed using the
		/// parameters in <paramref name="transforms"/>[i].  TurboJPEG has the ability to
		/// reallocate the JPEG buffer to accommodate the size of the JPEG image.
		/// Thus, you can choose to:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// pre-allocate the JPEG buffer with an arbitrary size using
		/// <see cref="tjAlloc"/> and let TurboJPEG grow the buffer as needed,
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// set <paramref name="dstBufs"/>[i] to NULL to tell TurboJPEG to allocate the
		/// buffer for you, or
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// pre-allocate the buffer to a "worst case" size determined by
		/// calling <see cref="tjBufSize"/> with the transformed or cropped width and
		/// height.  This should ensure that the buffer never has to be
		/// re-allocated (setting <see cref="FunctionFlags.NoRealloc"/> guarantees this.)
		/// </description>
		/// </item>
		/// </list>
		/// If you choose option 1, <paramref name="dstSizes"/>[i] should be set to
		/// the size of your pre-allocated buffer.  In any case, unless you have set
		/// <see cref="FunctionFlags.NoRealloc"/>, you should always check <paramref name="dstBufs"/>[i]
		/// upon return from this function, as it may have changed.
		/// </param>
		/// <param name="dstSizes">
		/// pointer to an array of <paramref name="n"/> unsigned long variables that will
		/// receive the actual sizes (in bytes) of each transformed JPEG image.  If
		/// <paramref name="dstBufs"/>[i] points to a pre-allocated buffer, then
		/// <paramref name="dstSizes"/>[i] should be set to the size of the buffer.  Upon return,
		/// <paramref name="dstSizes"/>[i] will contain the size of the JPEG image (in bytes.)
		/// </param>
		/// <param name="transforms">
		/// pointer to an array of <paramref name="n"/> <see cref="Transform"/> structures, each of
		/// which specifies the transform parameters and/or cropping region for
		/// the corresponding transformed output image.
		/// </param>
		/// <param name="flags">the bitwise OR of one or more of the <see cref="FunctionFlags"/>.</param>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjTransform(tjhandle handle,
		///   const unsigned char* jpegBuf, unsigned long jpegSize, int n,
		///   unsigned char** dstBufs, unsigned long* dstSizes, tjtransform *transforms,
		///   int flags);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjTransform(IntPtr handle, byte[] jpegBuf,
			uint jpegSize, int n, IntPtr[] dstBufs, uint[] dstSizes, Transform[] transforms, FunctionFlags flags);

		/// <summary>
		/// Destroy a TurboJPEG compressor, decompressor, or transformer instance.
		/// </summary>
		/// <returns>0 if successful, or -1 if an error occurred (see <see cref="tjGetErrorStr"/>.)</returns>
		/// <param name="handle">a handle to a TurboJPEG compressor, decompressor or transformer instance.</param>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT int DLLCALL tjDestroy(tjhandle handle);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern int tjDestroy(IntPtr handle);

		/// <summary>
		/// Allocate an image buffer for use with TurboJPEG.  You should always use
		/// this function to allocate the JPEG destination buffer(s) for <see cref="tjCompress2"/>
		/// and <see cref="tjTransform"/> unless you are disabling automatic buffer
		/// (re)allocation (by setting <see cref="FunctionFlags.NoRealloc"/>.)
		/// </summary>
		/// <returns>a pointer to a newly-allocated buffer with the specified number of bytes.</returns>
		/// <param name="bytes">the number of bytes to allocate.</param>
		/// <seealso cref="tjFree"/>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT unsigned char* DLLCALL tjAlloc(int bytes);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern IntPtr tjAlloc(int bytes);

		/// <summary>
		/// Free an image buffer previously allocated by TurboJPEG.  You should always
		/// use this function to free JPEG destination buffer(s) that were automatically
		/// (re)allocated by <see cref="tjCompress2"/> or <see cref="tjTransform"/> or that were manually
		/// allocated using <see cref="tjAlloc"/>.
		/// </summary>
		/// <param name="buffer">buffer address of the buffer to free.</param>
		/// <seealso cref="tjAlloc"/>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT void DLLCALL tjFree(unsigned char *buffer);
		/// </remarks>
		[DllImport("libjpeg-turbo.so")]
		internal static extern void tjFree(IntPtr buffer);

		/// <summary>
		/// Returns a descriptive error message explaining why the last command failed.
		/// </summary>
		/// <returns>a string containing a descriptive error message explaining why the last command failed.</returns>
		/// <remarks>
		/// Defined in turbojpeg.h as:
		/// 
		/// DLLEXPORT char* DLLCALL tjGetErrorStr(void);
		/// </remarks>
		[DllImport("libjpeg-turbo.so", CharSet=CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		internal static extern string tjGetErrorStr();
	}
}
