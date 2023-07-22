using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Maths
{
    /// <summary>
    /// 数学関数を定義する
    /// </summary>
    public static class Math
    {
        #region Constants
        
        /// <summary>
        /// 小数点誤差の許容値の最小値
        /// </summary>
        public const float Epsilon = float.Epsilon == 0f ? 1.1754944E-38f : float.Epsilon;
        
        /// <summary>
        /// 小数点誤差の許容値
        /// </summary>
        public const float Tolerance = 0.0000001f;

        #endregion
        #region IsAlmostEqual
        
        /// <summary>
        /// 小数点誤差を許容した等値比較をする
        /// </summary>
        /// <param name="a">比較対象の左辺値</param>
        /// <param name="b">比較対象の右辺値</param>
        /// <returns>値がほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostEqual(float a, float b) =>
            MathF.Abs(b - a) < MathF.Max(Tolerance * MathF.Max(MathF.Abs(a), MathF.Abs(b)), Epsilon * 8f);
        
        /// <summary>
        /// 小数点誤差を許容した等値比較をする
        /// </summary>
        /// <param name="a">比較対象の左辺値</param>
        /// <param name="b">比較対象の右辺値</param>
        /// <returns>値がほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostEqual(Vector2 a, Vector2 b) =>
            IsAlmostEqual(a.x, b.x) && IsAlmostEqual(a.y, b.y);
        
        /// <summary>
        /// 小数点誤差を許容した等値比較をする
        /// </summary>
        /// <param name="a">比較対象の左辺値</param>
        /// <param name="b">比較対象の右辺値</param>
        /// <returns>値がほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostEqual(Vector3 a, Vector3 b) =>
            IsAlmostEqual(a.x, b.x) && IsAlmostEqual(a.y, b.y) && IsAlmostEqual(a.z, b.z);
        
        #endregion
        #region IsAlmostZero
        
        /// <summary>
        /// 小数点誤差を許容した0との等値比較をする
        /// </summary>
        /// <param name="value">比較対象</param>
        /// <returns>値が0とほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostZero(this float value) =>
            IsAlmostEqual(value, 0f);
        
        /// <summary>
        /// 小数点誤差を許容した0との等値比較をする
        /// </summary>
        /// <param name="value">比較対象</param>
        /// <returns>値が0とほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostZero(this Vector2 value) =>
            IsAlmostEqual(value.x, 0f) && IsAlmostEqual(value.y, 0f);
        
        /// <summary>
        /// 小数点誤差を許容した0との等値比較をする
        /// </summary>
        /// <param name="value">比較対象</param>
        /// <returns>値が0とほぼ等しいか</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlmostZero(this Vector3 value) =>
            IsAlmostEqual(value.x, 0f) && IsAlmostEqual(value.y, 0f) && IsAlmostEqual(value.z, 0f);
        
        #endregion
        #region Clamp
        
        /// <summary>
        /// 値を指定された範囲内に収める
        /// </summary>
        /// <param name="value">収める値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>収めた値</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max) =>
            MathF.Max(min, MathF.Min(max, value));
        
        #endregion
        #region ClampMagnitude

        /// <summary>
        /// ベクトルの大きさを指定された範囲内に収める
        /// </summary>
        /// <param name="value">収めるベクトル</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>収めたベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClampMagnitude(this Vector2 value, float min, float max)
        {
            var magnitude = value.magnitude;
            return magnitude == 0f ? Vector2.zero : value / magnitude * magnitude.Clamp(min, max);
        }
        
        /// <summary>
        /// ベクトルの大きさを指定された範囲内に収める
        /// </summary>
        /// <param name="value">収めるベクトル</param>
        /// <param name="max">最大値</param>
        /// <returns>収めたベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClampMagnitude(this Vector2 value, float max = 1f) =>
            ClampMagnitude(value, 0f, max);
        
        /// <summary>
        /// ベクトルの大きさを指定された範囲内に収める
        /// </summary>
        /// <param name="value">収めるベクトル</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>収めたベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClampMagnitude(this Vector3 value, float min, float max)
        {
            var magnitude = value.magnitude;
            return magnitude == 0f ? Vector3.zero : value / magnitude * magnitude.Clamp(min, max);
        }
        
        /// <summary>
        /// ベクトルの大きさを指定された範囲内に収める
        /// </summary>
        /// <param name="value">収めるベクトル</param>
        /// <param name="max">最大値</param>
        /// <returns>収めたベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClampMagnitude(this Vector3 value, float max = 1f) =>
            ClampMagnitude(value, 0f, max);
        
        #endregion
        #region Remap

        /// <summary>
        /// 値を指定された範囲から別の範囲に変換する
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="fromMin">変換前の最小値</param>
        /// <param name="fromMax">変換前の最大値</param>
        /// <param name="toMin">変換後の最小値</param>
        /// <param name="toMax">変換後の最大値</param>
        /// <returns>変換後の値</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax) =>
            (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;

        #endregion
        #region RemapMagnitude

        /// <summary>
        /// ベクトルの大きさを指定された範囲から別の範囲に変換する
        /// </summary>
        /// <param name="v">変換するベクトル</param>
        /// <param name="fromMin">変換前の最小値</param>
        /// <param name="fromMax">変換前の最大値</param>
        /// <param name="toMin">変換後の最小値</param>
        /// <param name="toMax">変換後の最大値</param>
        /// <returns>変換後のベクトル</returns>
        public static Vector2 RemapMagnitude(this Vector2 v, float fromMin, float fromMax, float toMin = 0f, 
            float toMax = 1f)
        {
            var magnitude = v.magnitude;
            return v / magnitude * magnitude.Remap(fromMin, fromMax, toMin, toMax);
        }

        /// <summary>
        /// ベクトルの大きさを指定された範囲から別の範囲に変換する
        /// </summary>
        /// <param name="value">変換するベクトル</param>
        /// <param name="fromMin">変換前の最小値</param>
        /// <param name="fromMax">変換前の最大値</param>
        /// <param name="toMin">変換後の最小値</param>
        /// <param name="toMax">変換後の最大値</param>
        /// <returns>変換後のベクトル</returns>
        public static Vector3 RemapMagnitude(this Vector3 value, float fromMin, float fromMax, float toMin = 0f, 
            float toMax = 1f)
        {
            var magnitude = value.magnitude;
            return value / magnitude * magnitude.Remap(fromMin, fromMax, toMin, toMax);
        }

        #endregion
        #region Repeat

        /// <summary>
        /// 指定された値を [min, max] の範囲内に繰り返すように丸めます
        /// </summary>
        /// <param name="value">丸められる値</param>
        /// <param name="min">丸められる値の下限</param>
        /// <param name="min">丸められる値の下限</param>
        /// <param name="max">丸められる値の上限</param>
        /// <returns>丸められた値</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(float value, float min, float max) =>
            value - MathF.Floor(value / (max - min)) * (max - min);

        /// <summary>
        /// 指定された値を [0, length] の範囲に繰り返すように丸めます
        /// </summary>
        /// <param name="value">丸められる値</param>
        /// <param name="max">丸められる値の上限</param>
        /// <returns>丸められた値</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(float value, float max = 1f) => Repeat(value, 0.0f, max);

        #endregion
        #region ToVector2
        
        /// <summary>
        /// Vector3(X, Y, Z)をVector2(X, Y)に変換する
        /// </summary>
        /// <param name="value">変換するベクトル</param>
        /// <returns>変換したベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XYZToXY(this Vector3 value) => new(value.x, value.y);

        /// <summary>
        /// Vector3(X, Y, Z)をVector2(X, Z)に変換する
        /// </summary>
        /// <param name="value">変換するベクトル</param>
        /// <returns>変換したベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XYZToXZ(this Vector3 value) => new(value.x, value.z);
        
        #endregion
        #region ToVector3
        
        /// <summary>
        /// Vector2(X, Y)をVector3(X, Y, 0)に変換する
        /// </summary>
        /// <param name="value">変換するベクトル</param>
        /// <returns>変換したベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 XYToXYZ(this Vector2 value) => new(value.x, value.y, 0f);

        /// <summary>
        /// Vector2(X, Y)をVector3(X, 0, Y)に変換する
        /// </summary>
        /// <param name="value">変換するベクトル</param>
        /// <returns>変換したベクトル</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 XZToXYZ(this Vector2 value) => new(value.x, 0f, value.y);
        
        #endregion
    }
}