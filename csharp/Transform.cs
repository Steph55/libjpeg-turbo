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
	/// A callback function that can be used to modify the DCT coefficients
	/// after they are losslessly transformed but before they are transcoded to a
	/// new JPEG file.  This allows for custom filters or other transformations to
	/// be applied in the frequency domain.
	/// </summary>
	/// <returns>0 if the callback was successful, or -1 if an error occurred.</returns>
	/// <param name="coeffs">
	///  pointer to an array of shorts of transformed DCT coefficients.  (NOTE:
	///  this pointer is not guaranteed to be valid once the callback
	///  returns, so applications wishing to hand off the DCT coefficients
	///  to another function or library should make a copy of them within
	///  the body of the callback.)
	/// </param>
	/// <param name="arrayRegion">
	///  structure containing the width and height of
	///  the array pointed to by <paramref name="coeffs"/> as well as its offset
	///  relative to the component plane.  TurboJPEG implementations may
	///  choose to split each component plane into multiple DCT coefficient
	///  arrays and call the callback function once for each array.
	/// </param>
	/// <param name="planeRegion">
	///  structure containing the width and height of
	///  the component plane to which <paramref name="coeffs"/> belongs.
	/// </param>
	/// <param name="componentIndex">
	///  ID number of the component plane to which
	///  <paramref name="coeffs"/> belongs (Y, U, and V have, respectively, ID's of
	///  0, 1, and 2 in typical JPEG images.)
	/// </param>
	/// <param name="transformIndex">
	///  ID number of the transformed image to which
	///  <paramref name="coeffs"/> belongs.  This is the same as the index of the
	///  transform in the <c>transforms</c> array that was passed to
	///  <see cref="Transformer.Transform"/>.
	/// </param>
	/// <param name="transform">A structure that specifies the parameters and/or cropping region for this transform.</param>
	public delegate int CustomFilter(short[] coeffs, Region arrayRegion, Region planeRegion, int componentIndex, int transformIndex, ref Transform transform);

	/// <summary>
	/// Lossless transform
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Transform
	{
		/// <summary>
		/// Cropping region
		/// </summary>
		public Region r;

		/// <summary>
		/// One of the <see cref="TransformOp"/> "transform operations"
		/// </summary>
		public TransformOp op;

		/// <summary>
		/// The bitwise OR of one or more <see cref="TransformFlags"/> "transform options"
		/// </summary>
		public int options;

		/// <summary>
		/// Arbitrary data that can be accessed within the body of the callback function.
		/// </summary>
		public IntPtr data;

		/// <summary>
		/// A callback function that can be used to modify the DCT coefficients
		/// after they are losslessly transformed but before they are transcoded to a
		/// new JPEG file.  This allows for custom filters or other transformations to
		/// be applied in the frequency domain.
		/// </summary>
		/// <seealso cref="CustomFilter"/>
		public IntPtr customFilter;
	}
}
