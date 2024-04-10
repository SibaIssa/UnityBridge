using System;
using ProBridge.ROS.Msgs.Std;

namespace ProBridge.ROS
{
    public static class QoS
    {
        public static readonly string DEFAULT = "qos_profile_system_default";
        public static readonly string BEST_EFFORT = "qos_profile_sensor_data";
    }
}

namespace ProBridge.ROS.Msgs
{
    public interface IRosMsg
    {
        public string GetRosType();
    }

#if ROS_V2
    public class Time
    {
        public uint sec;
        public uint nanosec;

        public static implicit operator Time(TimeSpan value)
        {
            return new Time()
            {
                sec = (uint)(value.TotalSeconds - 62135596800L), // convert seconds into unix time
                nanosec = (uint)((value.Ticks - ((long)value.TotalSeconds * TimeSpan.TicksPerSecond)) * (1e9d / TimeSpan.TicksPerSecond))
            };
        }

        public static implicit operator TimeSpan(Time value)
        {
            return new TimeSpan((long)((value.sec + 62135596800L) * TimeSpan.TicksPerSecond + (value.nanosec / 1e6 * TimeSpan.TicksPerMillisecond)));
        }
    }
#else
    public class Time
    {
        public uint secs;
        public uint nsecs;

        public static implicit operator Time(TimeSpan value)
        {
            return new Time()
            {
                secs = (uint)(value.TotalSeconds - 62135596800L), // convert seconds into unix time
                nsecs = (uint)((value.Ticks - ((long)value.TotalSeconds * TimeSpan.TicksPerSecond)) * (1e9d / TimeSpan.TicksPerSecond))
            };
        }

        public static implicit operator TimeSpan(Time value)
        {
            return new TimeSpan((long)((value.secs + 62135596800L) * TimeSpan.TicksPerSecond + (value.nsecs / 1e6 * TimeSpan.TicksPerMillisecond)));
        }
    }
#endif
}

namespace ProBridge.ROS.Msgs.Std
{
    public abstract class StdMsg<T> : IRosMsg
    {
        public T data;

        public abstract string GetRosType();
    }

    public class StdTime : StdMsg<Time>
    {
        public override string GetRosType() { return "std_msgs.msg.Time"; }

        public StdTime(TimeSpan time)
        {
            data = time;
        }
    }

    public class StdBool : StdMsg<bool>
    {
        public override string GetRosType() { return "std_msgs.msg.Bool"; }
    }

    public class StdFloat : StdMsg<float>
    {
        public override string GetRosType() { return "std_msgs.msg.Float32"; }
    }

    public class StdInt : StdMsg<int>
    {
        public override string GetRosType() { return "std_msgs.msg.Int32"; }
    }

    public class StdString : StdMsg<string>
    {
        public override string GetRosType() { return "std_msgs.msg.String"; }
    }

    public class Header : IRosMsg
    {
        string IRosMsg.GetRosType() { return "std_msgs.msg.Header"; }

        public Time stamp;
#if ROS_V2
#else
        public UInt32 seq;
#endif
        public string frame_id;
    }

    public interface IStamped
    {
        Header header { get; }
    }
}

namespace ProBridge.ROS.Msgs.Rosgraph
{
    public class Clock : IRosMsg
    {
        string IRosMsg.GetRosType() { return "rosgraph_msgs.msg.Clock"; }

        public Time clock = new Time();

        public Clock() { }

        public Clock(TimeSpan time)
        {
            clock = time;
        }
    }
}

namespace ProBridge.ROS.Msgs.Geometry
{
    public class Point : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.Point"; }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public static implicit operator Point(Vector3 value) { return new Point() { x = value.x, y = value.y, z = value.z }; }
        public static implicit operator Vector3(Point value) { return new Vector3() { x = value.x, y = value.y, z = value.z }; }
    }

    public class Vector3 : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.Vector3"; }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class PointStamped : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.PointStamped"; }

        public Header header { get; set; } = new Header();
        public Point point = new Point();
    }

    public class Quaternion : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.Quaternion"; }

        public double x;
        public double y;
        public double z;
        public double w;
    }

    public class Pose : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.Pose"; }

        public Point position = new Point();
        public Quaternion orientation = new Quaternion() { w = 1 };
    }

    public class PoseStamped : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.PoseStamped"; }

        public Header header { get; set; } = new Header();
        public Pose pose = new Pose();
    }

    public class PoseWithCovariance : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.PoseWithCovariance"; }

        public Pose pose = new Pose();
        public float[] covariance = new float[36];
    }

    public class Twist : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.Twist"; }

        public Point linear = new Point();
        public Point angular = new Point();
    }

    public class TwistStamped : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.TwistStamped"; }

        public Header header { get; set; } = new Header();
        public Twist twist = new Twist();
    }

    public class TwistithCovariance : IRosMsg
    {
        string IRosMsg.GetRosType() { return "geometry_msgs.msg.TwistithCovariance"; }

        public Twist twist = new Twist();
        public float[] covariance = new float[36];
    }
}

namespace ProBridge.ROS.Msgs.Sensors
{
    public class Imu : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "sensor_msgs.msg.Imu"; }

        public Header header { get; set; } = new Header();
        public Geometry.Quaternion orientation = new Geometry.Quaternion() { w = 1 };
        public float[] orientation_covariance = new float[9];
        public Geometry.Vector3 angular_velocity = new Geometry.Vector3();
        public float[] angular_velocity_covariance = new float[9];
        public Geometry.Vector3 linear_acceleration = new Geometry.Vector3();
        public float[] linear_acceleration_covariance = new float[9];
    }

    public class NavSatFix : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "sensor_msgs.msg.NavSatFix"; }

        public const byte COVARIANCE_TYPE_UNKNOWN = 0;
        public const byte COVARIANCE_TYPE_APPROXIMATED = 1;
        public const byte COVARIANCE_TYPE_DIAGONAL_KNOWN = 2;
        public const byte COVARIANCE_TYPE_KNOWN = 3;

        public Header header { get; set; } = new Header();

        /// <summary>
        /// Latitude [degrees]. Positive is north of equator; negative is south
        /// </summary>
        public double latitude;

        /// <summary>
        /// Longitude [degrees]. Positive is east of prime meridian; negative is west.
        /// </summary>
        public double longitude;

        /// <summary>
        /// Altitude [m]. Positive is above the WGS 84 ellipsoid 
        /// (quiet NaN if no altitude is available).
        /// </summary>
        public double altitude;

        /// <summary>
        /// // Position covariance [m^2] defined relative to a tangential plane
        /// through the reported position. The components are East, North, and
        /// Up (ENU), in row-major order.
        /// Beware: this coordinate system exhibits singularities at the poles.
        /// </summary>
        public double[] position_covariance = new double[9];

        /// <summary>
        /// If the covariance of the fix is known, fill it in completely. If the
        /// GPS receiver provides the variance of each measurement, put them
        /// along the diagonal. If only Dilution of Precision is available,
        /// estimate an approximate covariance from that.
        /// </summary>
        public byte position_covariance_type;
    }
}

namespace ProBridge.ROS.Msgs.Nav
{
    public class Odometry : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "nav_msgs.msg.Odometry"; }

        public Header header { get; set; } = new Header();
        public string child_frame_id;
        public Geometry.PoseWithCovariance pose = new Geometry.PoseWithCovariance();
        public Geometry.TwistithCovariance twist = new Geometry.TwistithCovariance();
    }
    public class Path : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "nav_msgs.msg.Path"; }

        public Header header { get; set; } = new Header();
        public Geometry.PoseStamped[] poses;
    }
}

namespace ProBridge.ROS.Msgs.Chassis
{
    public class ChassisStatus : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "chassis_msgs.msg.ChassisStatus"; }

        public Header header { get; set; } = new Header();

        public float battery;                           // ���������� ��� �����
        public float fuel_available;                    // ����� ������� �
        public float fuel_consumption;                  // ������� (����������� �� ���) ������ ������� �/�
        public sbyte engine_state;                      // ������ ���
        public sbyte engine_temp;                       // ����������� ��� ������
        public UInt16 engine_value;                     // ������� ��� ��/���
        public sbyte transmission_state;                // ������ ����
        public sbyte transmission_value;                // �������� �������� ����
        public sbyte transmission_temp;                 // ����������� ���� ������
        public sbyte transfer_value;                    // �������� ����������� �������
        public sbyte main_brake_state;                  // ������ ��������� �������
        public sbyte parking_brake_state;               // ������ ������������ �������
        public sbyte rail_state;                        // ������ ������� �����
        public sbyte[] parts_temp = new sbyte[0];       // ����������� ��������� ������ ��� ������
        public sbyte[] general_state = new sbyte[0];    // ������� ��������, �������� ���������, ����������, � �.�.
        public bool sto;                                // ���������� ��������
    }

    public class ChassisFeed : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "chassis_msgs.msg.ChassisFeed"; }

        public Header header { get; set; } = new Std.Header();
        public float speed;                         // ��������� ������� �������� [�/���]
        public Int16[] engine_value = new Int16[0]; // ������� ��������� [��/���]
        public float[] rail_value = new float[0];   // ���� �������� ������� ����� [������]
        public float[] rail_speed = new float[0];   // �������� �������� ������� ����� � [���/���]
        public float[] rail_target = new float[0];  // ������� �� ������� ����� [���. ��]
        public float[] accel_value = new float[0];  // ��������� ������ ���� [���. ��]
        public float[] accel_target = new float[0]; // ������� �� ������ ���� [���. ��]
        public float[] brake_value = new float[0];  // �������� ������� �������� ����� �� ��������� ������� (������������� �������� - ��� �������������) [���. ��]
        public float[] brake_target = new float[0]; // ������� ��� ��������� ������� [���. ��]
    }

    public class ChassisSignals : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "chassis_msgs.msg.ChassisSignals"; }

        public Header header { get; set; } = new Std.Header();
        public bool lights_side;            // ��������� ���������� �����
        public bool lights_head;            // ��������� ������� ��������� �����
        public bool lights_left_turn;       // ��������� ������ ��������� ��������
        public bool lights_right_turn;      // ��������� ������� ��������� ��������
        public bool sound_signal;           // ��������� ��������� �������
        public byte[] aux = new byte[0];     // ��������� ��������������� ����������� ������������
    }
}

namespace ProBridge.ROS.Msgs.Ackermann
{
    public class AckermannDrive : IRosMsg, IStamped
    {
        string IRosMsg.GetRosType() { return "ackermann_msgs.msg.AckermannDrive"; }

        public Header header { get; set; } = new Header();

        public float steering_angle;                  // desired virtual angle (radians)
        public float steering_angle_velocity;         // desired rate of change (radians/s)
        public float speed;                           // desired forward speed (m/s)
        public sbyte acceleration;                    // desired acceleration (m/s^2)
        public sbyte jerk;                            // desired jerk (m/s^3)
    }
}
