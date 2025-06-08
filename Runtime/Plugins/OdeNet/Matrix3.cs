using System;
using System.Globalization;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif
namespace Ode.Net
{
    /// <summary>
    /// Represents a 3x3 matrix.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        /// <summary>
        /// Represents the identity matrix.
        /// </summary>
        public static readonly Matrix3 Identity = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        /// <summary>
        /// Specifies the first row of the matrix.
        /// </summary>
        public Vector3 Row1;

        /// <summary>
        /// Specifies the second row of the matrix.
        /// </summary>
        public Vector3 Row2;

        /// <summary>
        /// Specifies the third row of the matrix.
        /// </summary>
        public Vector3 Row3;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix3"/> structure
        /// with the specified element values.
        /// </summary>
        /// <param name="m11">The value at the first row and first column of the matrix.</param>
        /// <param name="m12">The value at the first row and second column of the matrix.</param>
        /// <param name="m13">The value at the first row and third column of the matrix.</param>
        /// <param name="m21">The value at the second row and first column of the matrix.</param>
        /// <param name="m22">The value at the second row and second column of the matrix.</param>
        /// <param name="m23">The value at the second row and third column of the matrix.</param>
        /// <param name="m31">The value at the third row and first column of the matrix.</param>
        /// <param name="m32">The value at the third row and second column of the matrix.</param>
        /// <param name="m33">The value at the third row and third column of the matrix.</param>
        public Matrix3(
            dReal m11, dReal m12, dReal m13,
            dReal m21, dReal m22, dReal m23,
            dReal m31, dReal m32, dReal m33)
        {
            Row1.X = m11;
            Row1.Y = m12;
            Row1.Z = m13;
            Row1.W = 0;
            Row2.X = m21;
            Row2.Y = m22;
            Row2.Z = m23;
            Row2.W = 0;
            Row3.X = m31;
            Row3.Y = m32;
            Row3.Z = m33;
            Row3.W = 0;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Matrix3"/> value.
        /// </summary>
        /// <param name="other">A <see cref="Matrix3"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same element values as
        /// this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(Matrix3 other)
        {
            return Row1 == other.Row1 && Row2 == other.Row2 && Row3 == other.Row3;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="Matrix3"/> and
        /// has the same element values as this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Matrix3)
            {
                return Equals((Matrix3)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Row1.GetHashCode() ^ Row2.GetHashCode() ^ Row3.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of all the elements in the matrix.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", Row1, Row2, Row3);
        }

        /// <summary>
        /// Negates all the elements in the source matrix and returns the result
        /// as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <returns>The result of negating all the elements in the source matrix.</returns>
        public static Matrix3 Negate(Matrix3 value)
        {
            Matrix3 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Negates all the elements in the source matrix and returns the result
        /// as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <param name="result">The result of negating all the elements in the source matrix.</param>
        public static void Negate(ref Matrix3 value, out Matrix3 result)
        {
            Vector3.Negate(ref value.Row1, out result.Row1);
            Vector3.Negate(ref value.Row2, out result.Row2);
            Vector3.Negate(ref value.Row3, out result.Row3);
        }

        /// <summary>
        /// Adds a matrix to another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The first matrix to add.</param>
        /// <param name="value2">The second matrix to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix3 Add(Matrix3 value1, Matrix3 value2)
        {
            Matrix3 result;
            Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds a matrix to another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The first matrix to add.</param>
        /// <param name="value2">The second matrix to add.</param>
        /// <param name="result">The sum of the two matrices.</param>
        public static void Add(ref Matrix3 value1, ref Matrix3 value2, out Matrix3 result)
        {
            Vector3.Add(ref value1.Row1, ref value2.Row1, out result.Row1);
            Vector3.Add(ref value1.Row2, ref value2.Row2, out result.Row2);
            Vector3.Add(ref value1.Row3, ref value2.Row3, out result.Row3);
        }

        /// <summary>
        /// Subtracts a matrix from another matrix and returns the result as a new
        /// <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The matrix from which the other matrix will be subtracted.</param>
        /// <param name="value2">The matrix that is to be subtracted.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix3 Subtract(Matrix3 value1, Matrix3 value2)
        {
            Matrix3 result;
            Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts a matrix from another matrix and returns the result as a new
        /// <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The matrix from which the other matrix will be subtracted.</param>
        /// <param name="value2">The matrix that is to be subtracted.</param>
        /// <param name="result">The result of the subtraction.</param>
        public static void Subtract(ref Matrix3 value1, ref Matrix3 value2, out Matrix3 result)
        {
            Vector3.Subtract(ref value1.Row1, ref value2.Row1, out result.Row1);
            Vector3.Subtract(ref value1.Row2, ref value2.Row2, out result.Row2);
            Vector3.Subtract(ref value1.Row3, ref value2.Row3, out result.Row3);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix3 Multiply(Matrix3 value1, dReal value2)
        {
            Matrix3 result;
            Multiply(ref value1, value2, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scalar value.</param>
        /// <param name="result">The result of the multiplication.</param>
        public static void Multiply(ref Matrix3 value1, dReal value2, out Matrix3 result)
        {
            Vector3.Multiply(ref value1.Row1, value2, out result.Row1);
            Vector3.Multiply(ref value1.Row2, value2, out result.Row2);
            Vector3.Multiply(ref value1.Row3, value2, out result.Row3);
        }

        /// <summary>
        /// Multiplies a matrix by another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The first matrix to multiply.</param>
        /// <param name="value2">The second matrix to multiply.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix3 Multiply(Matrix3 value1, Matrix3 value2)
        {
            Matrix3 result;
            Multiply(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The first matrix to multiply.</param>
        /// <param name="value2">The second matrix to multiply.</param>
        /// <param name="result">The result of the multiplication.</param>
        public static void Multiply(ref Matrix3 value1, ref Matrix3 value2, out Matrix3 result)
        {
            result.Row1.X = value1.Row1.X * value2.Row1.X + value1.Row1.Y * value2.Row2.X + value1.Row1.Z * value2.Row3.X;
            result.Row1.Y = value1.Row1.X * value2.Row1.Y + value1.Row1.Y * value2.Row2.Y + value1.Row1.Z * value2.Row3.Y;
            result.Row1.Z = value1.Row1.X * value2.Row1.Z + value1.Row1.Y * value2.Row2.Z + value1.Row1.Z * value2.Row3.Z;
            result.Row1.W = 0;
            result.Row2.X = value1.Row2.X * value2.Row1.X + value1.Row2.Y * value2.Row2.X + value1.Row2.Z * value2.Row3.X;
            result.Row2.Y = value1.Row2.X * value2.Row1.Y + value1.Row2.Y * value2.Row2.Y + value1.Row2.Z * value2.Row3.Y;
            result.Row2.Z = value1.Row2.X * value2.Row1.Z + value1.Row2.Y * value2.Row2.Z + value1.Row2.Z * value2.Row3.Z;
            result.Row2.W = 0;
            result.Row3.X = value1.Row3.X * value2.Row1.X + value1.Row3.Y * value2.Row2.X + value1.Row3.Z * value2.Row3.X;
            result.Row3.Y = value1.Row3.X * value2.Row1.Y + value1.Row3.Y * value2.Row2.Y + value1.Row3.Z * value2.Row3.Y;
            result.Row3.Z = value1.Row3.X * value2.Row1.Z + value1.Row3.Y * value2.Row2.Z + value1.Row3.Z * value2.Row3.Z;
            result.Row3.W = 0;
        }

        /// <summary>
        /// Divides a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        public static Matrix3 Divide(Matrix3 value1, dReal value2)
        {
            Matrix3 result;
            Divide(ref value1, value2, out result);
            return result;
        }

        /// <summary>
        /// Divides a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scalar value.</param>
        /// <param name="result">The result of the division.</param>
        public static void Divide(ref Matrix3 value1, dReal value2, out Matrix3 result)
        {
            Vector3.Divide(ref value1.Row1, value2, out result.Row1);
            Vector3.Divide(ref value1.Row2, value2, out result.Row2);
            Vector3.Divide(ref value1.Row3, value2, out result.Row3);
        }

        /// <summary>
        /// Divides the components of a matrix by the components of another matrix
        /// and returns the result as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The matrix whose components will be divided.</param>
        /// <param name="value2">The divisor matrix.</param>
        /// <returns>The result of the division.</returns>
        public static Matrix3 Divide(Matrix3 value1, Matrix3 value2)
        {
            Matrix3 result;
            Divide(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Divides the components of a matrix by the components of another matrix
        /// and returns the result as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value1">The matrix whose components will be divided.</param>
        /// <param name="value2">The divisor matrix.</param>
        /// <param name="result">The result of the division.</param>
        public static void Divide(ref Matrix3 value1, ref Matrix3 value2, out Matrix3 result)
        {
            Vector3.Divide(ref value1.Row1, ref value2.Row1, out result.Row1);
            Vector3.Divide(ref value1.Row2, ref value2.Row2, out result.Row2);
            Vector3.Divide(ref value1.Row3, ref value2.Row3, out result.Row3);
        }

        /// <summary>
        /// Tests whether two <see cref="Matrix3"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// all their elements equal; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(Matrix3 left, Matrix3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="Matrix3"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ
        /// in any of their elements; <b>false</b> if <paramref name="left"/> and
        /// <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(Matrix3 left, Matrix3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Negates all the elements in the source matrix and returns the result
        /// as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <returns>The result of negating all the elements in the source matrix.</returns>
        public static Matrix3 operator -(Matrix3 value)
        {
            Matrix3 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Adds a matrix to another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the addition operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the addition operator.
        /// </param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix3 operator +(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Subtracts a matrix from another matrix and returns the result as a new
        /// <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the subtraction operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the subtraction operator.
        /// </param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix3 operator -(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The scalar value on the left of the multiplication operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the multiplication operator.
        /// </param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix3 operator *(dReal left, Matrix3 right)
        {
            Matrix3 result;
            Multiply(ref right, left, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the multiplication operator.
        /// </param>
        /// <param name="right">
        /// The scalar value on the right of the multiplication operator.
        /// </param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix3 operator *(Matrix3 left, dReal right)
        {
            Matrix3 result;
            Multiply(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the multiplication operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the multiplication operator.
        /// </param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Divides a matrix by a scalar value and returns the result as
        /// a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the division operator.
        /// </param>
        /// <param name="right">
        /// The scalar value on the right of the division operator.
        /// </param>
        /// <returns>The result of the division.</returns>
        public static Matrix3 operator /(Matrix3 left, dReal right)
        {
            Matrix3 result;
            Divide(ref left, right, out result);
            return result;
        }

        /// <summary>
        /// Divides the components of a matrix by the components of another matrix
        /// and returns the result as a new <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix3"/> structure on the left of the division operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix3"/> structure on the right of the division operator.
        /// </param>
        /// <returns>The result of the division.</returns>
        public static Matrix3 operator /(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Divide(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Creates a Matrix3 using the specified axis and angle
        /// </summary>
        public static Matrix3 FromAxisAndAngle(Vector3 axis, dReal angle)
        {
            Matrix3 o;
            Native.NativeMethods.dRFromAxisAndAngle(out o, axis.X, axis.Y, axis.Z, angle);
            return o;
        }

        /// <summary>
        /// Creates a Matrix3 using the specified axis
        /// </summary>
        public static Matrix3 FromZAxis(Vector3 axis)
        {
            Matrix3 o;
            Native.NativeMethods.dRFromZAxis(out o, axis.X, axis.Y, axis.Z);
            return o;
        }
    }
}
