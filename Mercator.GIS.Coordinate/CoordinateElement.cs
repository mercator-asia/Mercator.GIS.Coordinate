using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercator.GIS.Coordinate
{
    /// <summary>
    /// 平面（直角）坐标
    /// </summary>
    public class HorizontalCoordinate
    {
        /// <summary>
        /// 横坐标
        /// </summary>
        public double X;

        /// <summary>
        /// 纵坐标
        /// </summary>
        public double Y;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HorizontalCoordinate()
        {
            X = 0.0;
            Y = 0.0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public HorizontalCoordinate(double x, double y) : this()
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// 空间（直角）坐标系
    /// </summary>
    public class SpatialCoordinate
    {
        /// <summary>
        /// X轴坐标
        /// </summary>
        public double X;

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public double Y;

        /// <summary>
        /// Z轴坐标
        /// </summary>
        public double Z;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SpatialCoordinate()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">X轴坐标</param>
        /// <param name="y">Y轴坐标</param>
        /// <param name="z">Z轴坐标</param>
        public SpatialCoordinate(double x, double y, double z):this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    /// <summary>
    /// 大地坐标
    /// </summary>
    public class GeocentricCoordinate
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public Degree Latitude;

        /// <summary>
        /// 经度
        /// </summary>
        public Degree Longitude;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GeocentricCoordinate()
        {
            Latitude = new Degree();
            Longitude = new Degree();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="latitude">经度</param>
        /// <param name="longitude">纬度</param>
        public GeocentricCoordinate(double latitude, double longitude) : this()
        {
            Latitude.Digital = latitude;
            Longitude.Digital = longitude;
        }
    }

    /// <summary>
    /// 度（°）
    /// </summary>
    public class Degree
    {
        /// <summary>
        /// 数值
        /// </summary>
        public double Digital;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Degree()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="digital"></param>
        public Degree(double digital)
        {
            Digital = digital;
        }

        /// <summary>
        /// 重载函数
        /// </summary>
        /// <returns>度°分′秒″</returns>
        public override string ToString()
        {
            return Digital2DMS(Digital);
        }

        /// <summary>
        /// 转为弧度
        /// </summary>
        /// <returns></returns>
        public double ToRadian()
        {
            return Digital * Math.PI / 180;
        }

        /// <summary>
        /// 小数形式转换为度°分′秒″
        /// </summary>
        /// <param name="digital">小数</param>
        /// <returns>度°分′秒″</returns>
        public static string Digital2DMS(double digital)
        {
            int Degree = Convert.ToInt32(Math.Truncate(digital));//度
            digital = digital - Degree;
            int M = Convert.ToInt32(Math.Truncate((digital) * 60));//分
            int S = Convert.ToInt32(Math.Round((digital * 60 - M) * 60));

            if (S == 60)
            {
                M = M + 1;
                S = 0;
            }
            if (M == 60)
            {
                M = 0;
                Degree = Degree + 1;
            }
            string rstr = Degree.ToString() + "°";
            if (M < 10)
            {
                rstr = rstr + "0" + M.ToString();
            }
            else
            {
                rstr = rstr + M.ToString();
            }

            rstr += "′";

            if (S < 10)
            {
                rstr = rstr + "0" + S.ToString();
            }
            else
            {
                rstr = rstr + S.ToString();
            }

            rstr += "″";

            return rstr;
        }

        /// <summary>
        /// 度°分′秒″转为数字形式
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double DMS2Digital(string degrees)
        {
            const double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf('°');           //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf('′');           //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.IndexOf('″');           //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }
    }
}
