﻿//-----------------------------------------------------------------------
// <copyright file="DVector4.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Tango
{
    using System;

    /// <summary>
    /// Double precision vector in 4D.
    /// </summary>
    public struct DVector4
    {
        public double x;
        public double y;
        public double z;
        public double w;

        /// <summary>
        /// Creates a new double-precision vector with given x, y, z, w components.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        public DVector4(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Gets the length of this vector (Read Only).
        /// </summary>
        /// <value>The length.</value>
        public double Magnitude
        {
            get { return Math.Sqrt(SqrMagnitude); }
        }

        /// <summary>
        /// Gets the squared length of this vector (Read Only).
        /// </summary>
        /// <value>The squared length.</value>
        public double SqrMagnitude 
        {
            get { return (x * x) + (y * y) + (z * z) + (w * w); }
        }

        /// <summary>
        /// Access the x, y, z, w components using [0], [1], [2], [3] respectively.
        /// </summary>
        /// <param name="index">Vector index.</param>
        /// <returns>The value at the specified index.</returns>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid vector index!");
                }
            }
        }

        /// <summary>
        /// Returns the distance between a and b.
        /// </summary>
        /// <returns>Euclidean distance as a double.</returns>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        public static double Distance(DVector4 a, DVector4 b)
        {
            return (a - b).Magnitude;
        }

        /// <summary>
        /// Dot Product of two vectors.
        /// </summary>
        /// <returns>Dot product as a double.</returns>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        public static double Dot(DVector4 a, DVector4 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z) + (a.w * b.w);
        }

        /// <summary>
        /// Subtracts one vector from another.
        /// 
        /// Subtracts each component of b from a.
        /// </summary>
        /// <returns>Subtraction result.</returns>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        public static DVector4 operator -(DVector4 a, DVector4 b)
        {
            return new DVector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        /// <summary>
        /// Adds two vectors.
        /// 
        /// Adds corresponding components together.        
        /// </summary>
        /// <returns>Addition result.</returns>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        public static DVector4 operator +(DVector4 a, DVector4 b)
        {
            return new DVector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }
    }
}
