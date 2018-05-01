using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Mercator.Mathematics.LinearAlgebra;

namespace Mercator.GIS.Coordinate
{
    /// <summary>
    /// 线性转换
    /// </summary>
    public class LinearTransformation
    {
        /// <summary>
        /// 获取转换参数
        /// </summary>
        /// <param name="sourceCoordinates">源坐标系统的坐标集</param>
        /// <param name="targetCoordinates">目标坐标系统的坐标集</param>
        /// <returns></returns>
        public static Matrix GetTransformationParameter(List<HorizontalCoordinate> sourceCoordinates, List<HorizontalCoordinate> targetCoordinates)
        {
            var X = new Matrix(new double[4, 1]);
            var B = new Matrix(new double[sourceCoordinates.Count * 2, 4]);
            var L = new Matrix(new double[sourceCoordinates.Count * 2, 1]);
            var P = new Matrix(new double[sourceCoordinates.Count * 2, sourceCoordinates.Count * 2]);

            for (int i = 0; i < sourceCoordinates.Count * 2; i++)
            {
                for (int j = 0; j < sourceCoordinates.Count * 2; j++)
                {
                    if (i == j) { P[i, j] = 1; } else { P[i, j] = 0; }
                }
            }

            var index = 0;
            for (int i = 0; i < sourceCoordinates.Count; i++)
            {
                var sourceCoordinate = sourceCoordinates[i];
                var targetCoordinate = targetCoordinates[i];

                B[index, 0] = 1;
                B[index, 1] = 0;
                B[index, 2] = targetCoordinate.X;
                B[index, 3] = targetCoordinate.Y;

                B[index + 1, 0] = 0;
                B[index + 1, 1] = 1;
                B[index + 1, 2] = targetCoordinate.Y;
                B[index + 1, 3] = targetCoordinate.X * -1;

                L[index, 0] = sourceCoordinate.X;
                L[index + 1, 0] = sourceCoordinate.Y;

                index = index + 2;
            }

            X = (B.Transpose() * P * B).Inverse() * B.Transpose() * P * L;

            return X;
        }

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="source">源坐标系统的坐标</param>
        /// <param name="transformationParameter">转换参数</param>
        /// <returns></returns>
        public static HorizontalCoordinate Transform(HorizontalCoordinate source, Matrix transformationParameter)
        {
            var x = transformationParameter[0, 0] + source.X * transformationParameter[2, 0] + source.Y * transformationParameter[3, 0];
            var y = transformationParameter[1, 0] + source.Y * transformationParameter[2, 0] - source.X * transformationParameter[3, 0];

            return new HorizontalCoordinate(x, y);
        }
    }
}
