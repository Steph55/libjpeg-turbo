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
	/// Transform operations for <see cref="Transformer.Transform"/>.
	/// </summary>
	public enum TransformOp : int
	{
		/// <summary>
		/// Do not transform the position of the image pixels
		/// </summary>
		None=0,

		/// <summary>
		/// Flip (mirror) image horizontally.  This transform is imperfect if there
		/// are any partial MCU blocks on the right edge (see <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		HFlip,

		/// <summary>
		/// Flip (mirror) image vertically.  This transform is imperfect if there are
		/// any partial MCU blocks on the bottom edge (see <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		VFlip,

		/// <summary>
		/// Transpose image (flip/mirror along upper left to lower right axis.)  This
		/// transform is always perfect.
		/// </summary>
		Transpose,

		/// <summary>
		/// Transverse transpose image (flip/mirror along upper right to lower left
		/// axis.) This transform is imperfect if there are any partial MCU blocks in
		/// the image (see <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		Transverse,

		/// <summary>
		/// Rotate image clockwise by 90 degrees.  This transform is imperfect if
		/// there are any partial MCU blocks on the bottom edge (see
		/// <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		Rot90,

		/// <summary>
		/// Rotate image 180 degrees.  This transform is imperfect if there are any
		/// partial MCU blocks in the image (see <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		Rot180,

		/// <summary>
		/// TRotate image counter-clockwise by 90 degrees.  This transform is imperfect
		/// if there are any partial MCU blocks on the right edge (see
		/// <see cref="TransformFlags.Perfect"/>.)
		/// </summary>
		Rot270
	}
}
