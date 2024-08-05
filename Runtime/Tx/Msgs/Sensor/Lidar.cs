using System;
using UnityEngine;
using ProBridge.Utils;

namespace ProBridge.Tx.Sensor
{
    [AddComponentMenu("ProBridge/Tx/Sensor/Lidar")]
    public class Lidar : ProBridgeTxStamped<ROS.Msgs.Sensors.Lidar>
    {
        public enum LidarType
        {
            LaserScan,
            PointCloud2
        }
        
        // PointCloud2 Specific

        public int height;
        public int width;

        // LaserScan Specific

        public float angleMin;
        public float angleMax;
        public float angleIncrement;
        public float timeIncrement;
        public float scanTime;


        public LidarType lidarType = LidarType.PointCloud2;

        protected override void OnStart()
        {

        }

        protected override ProBridge.Msg GetMsg(TimeSpan ts)
        {
            switch (lidarType)
            {
                case LidarType.LaserScan:
                    data.angle_min.data = angleMin;
                    data.angle_max.data = angleMax;
                    data.angle_increment.data = angleIncrement;
                    data.time_increment.data = timeIncrement;
                    data.scan_time.data = scanTime;
                    
                    // TODO implement actual LaserScan scanning logic
                    
                    break;
                case LidarType.PointCloud2:
                    data.height.data = height;
                    data.width.data = width;
                    
                    // TODO implement actual PointCloud2 scanning logic

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return base.GetMsg(ts);
        }
    }
}