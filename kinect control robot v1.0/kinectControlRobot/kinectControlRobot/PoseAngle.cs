using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;


namespace kinectControlRobot
{
    public class PoseAngle
    {
        public PoseAngle(JointType centerJoint, JointType angleJoint, double angle, double threshold)
        {
            CenterJoint = centerJoint;
            AngleJoint = angleJoint;
            Angle = angle;
            Threshold = threshold;
        }


        public JointType CenterJoint { get; private set; }
        public JointType AngleJoint { get; private set; }
        public double Angle { get; private set; }
        public double Threshold { get; private set; }
    }
}
