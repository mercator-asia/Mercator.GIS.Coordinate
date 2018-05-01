using Mercator.GIS.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mercator.GIS.Coordinate.KVS
{
    /// <summary>
    /// 公共点文件管理
    /// </summary>
    public class KVS
    {
        /// <summary>
        /// 打开KVS文件
        /// </summary>
        /// <param name="kvsFileName">文件名</param>
        /// <param name="sourceCoordinates"></param>
        /// <param name="targetCoordinates"></param>
        public static void Open(string kvsFileName, out List<HorizontalCoordinate> sourceCoordinates, out List<HorizontalCoordinate> targetCoordinates)
        {
            sourceCoordinates = new List<HorizontalCoordinate>();
            targetCoordinates = new List<HorizontalCoordinate>();

            var projection = new GaussKrugerProjection();
            projection.Ellipsoid = ReferenceEllipsoid.International1975;

            var document = new XmlDocument();
            document.Load(kvsFileName);
            var root = document.DocumentElement;
            foreach (XmlNode coordinateNode in root.ChildNodes)
            {
                foreach (XmlNode node in coordinateNode.ChildNodes)
                {
                    double lng = double.MinValue, lat = double.MinValue;
                    foreach (XmlNode lbNode in node.ChildNodes)
                    {
                        switch (lbNode.Name.ToLower())
                        {
                            case "longitude":
                                lng = Convert.ToDouble(lbNode.InnerText.Trim());
                                break;
                            case "latitude":
                                lat = Convert.ToDouble(lbNode.InnerText.Trim());
                                break;
                        }
                    }

                    projection.LongitudeOfOrigin = (int)Math.Round(lng / 3) * 3;
                    double E, N;
                    projection.Forward(lat, lng, out E, out N);

                    switch (node.Name.ToLower())
                    {
                        case "source":
                            sourceCoordinates.Add(new HorizontalCoordinate(E, N));
                            break;
                        case "target":
                            targetCoordinates.Add(new HorizontalCoordinate(E, N));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 创建KVS文件
        /// </summary>
        /// <param name="kvsFileName">文件名</param>
        /// <param name="sourceCoordinates"></param>
        /// <param name="targetCoordinates"></param>
        public static void Create(string kvsFileName, List<GeocentricCoordinate> sourceCoordinates, List<GeocentricCoordinate> targetCoordinates)
        {
            var document = new XmlDocument();
            var header = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(header);

            var root = document.CreateElement("Coordinates");
            document.AppendChild(root);

            for (int i = 0; i < sourceCoordinates.Count; i++)
            {
                var coordinateNode = document.CreateElement("Coordinate");
                root.AppendChild(coordinateNode);

                var pixelNode = document.CreateElement("Source");
                var referenceNode = document.CreateElement("Target");
                coordinateNode.AppendChild(pixelNode);
                coordinateNode.AppendChild(referenceNode);

                var pixelLongitudeNode = document.CreateElement("Longitude");
                pixelLongitudeNode.InnerText = sourceCoordinates[i].Longitude.ToDigitalString();
                var pixelLatitudeNode = document.CreateElement("Latitude");
                pixelLatitudeNode.InnerText = sourceCoordinates[i].Latitude.ToDigitalString();
                pixelNode.AppendChild(pixelLatitudeNode);
                pixelNode.AppendChild(pixelLongitudeNode);

                var referenceLongitudeNode = document.CreateElement("Longitude");
                referenceLongitudeNode.InnerText = targetCoordinates[i].Longitude.ToDigitalString();
                var referenceLatitudeNode = document.CreateElement("Latitude");
                referenceLatitudeNode.InnerText = targetCoordinates[i].Latitude.ToDigitalString();
                referenceNode.AppendChild(referenceLatitudeNode);
                referenceNode.AppendChild(referenceLongitudeNode);
            }

            document.Save(kvsFileName);
        }
    }
}
