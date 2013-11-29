using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Kinect;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KinectBasicHandTrackingFramework;

namespace kinectControlRobot
{
    public partial class MainWindow : Window
    {
        #region Member Variables
        private KinectSensor kinectDevice;
        private Skeleton[] frameSkeletons;
        private int[] instructionSequence;
        private int currentLevel;
        private Random rnd = new Random();
        private Pose[] poseLibrary;
        private Pose currentPose;
        private int j;
        private int rightKey;
        private Pose[] tempPose;
        private int tempnum;
        const int pagenum = 15;
        const int posenum = 7;
     //   private int portnum;
    //    private string ipnum;
        Socket clientSocket;
        Thread clientThread;
    //    private int rightwave = 1;
        private int socketopen = 1;
        private WaveGesture _WaveGesture;
        #endregion Member Variables
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();       

            clientThread = new Thread(new ThreadStart(ConnectToServer));
            clientThread.Start();

            this._WaveGesture = new WaveGesture();
            this._WaveGesture.GestureDetected += new EventHandler(_WaveGesture_GestureDetected);
            //this.kinectDevice = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
            //this.kinectDevice.SkeletonFrameReady += KinectDevice_SkeletonFrameReady;

            this.poseLibrary = new Pose[posenum];
            this.currentLevel = 1;
            this.rightKey = 0;
            PopulatePoseLibrary();
            this.currentPose = this.poseLibrary[4];
            this.tempPose = new Pose[pagenum];
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.KinectDevice = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }
        #endregion Constructor


        #region Methods

        private void ConnectToServer()
        {
            //创建一个套接字
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001);
            // IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.197.1"), this.portnum);   
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //将套接字与远程服务器地址相连
            try
            {
                clientSocket.Connect(ipep);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("connect error: " + ex.Message);
                return;
            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                case KinectStatus.NotPowered:
                case KinectStatus.NotReady:
                case KinectStatus.DeviceNotGenuine:
                    this.KinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:
                    //TODO: Give the user feedback to plug-in a Kinect device.                    
                    this.KinectDevice = null;
                    break;
                default:
                    //TODO: Show an error state
                    break;
            }
        }


        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    this.frameSkeletons = new Skeleton[kinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                    frame.CopySkeletonDataTo(this.frameSkeletons);
                    Skeleton skeleton = GetPrimarySkeleton(this.frameSkeletons);

                    DateTime startMarker = DateTime.Now;
                    this._WaveGesture.Update(this.frameSkeletons, frame.Timestamp);

                    if (skeleton == null)
                    {
                    }
                    else
                    {
                        if (this.j == pagenum)
                        {
                            int ii;

                            for (ii = 0; ii <= (pagenum - 2); ii++)
                            {
                                if (this.tempPose[ii].num != this.tempPose[ii + 1].num) break;
                                //    CurrentPose0.Text = (GetJointAngle(skeleton.Joints[this.tempPose[ii].Angles[0].CenterJoint], skeleton.Joints[this.tempPose[ii].Angles[0].AngleJoint])).ToString();
                            }
                            if (ii == (pagenum - 1))
                            {
                              //  CurrentPose.Text = this.tempPose[ii].Title;
                                this.j = 0;                               
                                if (this.rightKey == 1)
                                {
                                    //system.windows.forms.sendkeys.sendwait("{right}");
                                    this.rightKey = 0;
                                  //  this.rightwave = 0;
                                    if (this.socketopen == 1)
                                    {
                                        byte[] data = new byte[1024];
                                        data = Encoding.ASCII.GetBytes(this.tempPose[ii].Title);
                                        clientSocket.Send(data, data.Length, SocketFlags.None);
                                        if (this.tempPose[ii].num == 5 || this.tempPose[ii].num == 6)
                                        {
                                            this.rightKey = 1;
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                else
                                {
                                  //  this.rightwave = 1;    
                                    if (this.tempPose[ii].num != this.tempnum)
                                    {
                                        this.rightKey = 1;                                  
                                    }
                                }
                                this.tempnum = this.tempPose[ii].num;
                            }
                            else
                            {
                                this.j = 0;
                            }
                        }


                        else
                        {
                            for (int iii = 0; iii < posenum; iii++)
                                if (IsPose(skeleton, this.poseLibrary[iii]))
                                {
                                    this.tempPose[this.j] = this.poseLibrary[iii];
                                    (this.j)++;
                                }
                        }

                    }
                }
            }
        }
        private static Point GetJointPoint(KinectSensor kinectDevice, Joint joint, Size containerSize, Point offset)
        {
            DepthImagePoint point = kinectDevice.CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, kinectDevice.DepthStream.Format);
            point.X = (int)((point.X * containerSize.Width / kinectDevice.DepthStream.FrameWidth) - offset.X);
            point.Y = (int)((point.Y * containerSize.Height / kinectDevice.DepthStream.FrameHeight) - offset.Y);

            return new Point(point.X, point.Y);
        }


        private double GetJointAngle(Joint centerJoint, Joint angleJoint)
        {
            Point primaryPoint = GetJointPoint(this.KinectDevice, centerJoint, this.LayoutRoot.RenderSize, new Point());
            Point anglePoint = GetJointPoint(this.KinectDevice, angleJoint, this.LayoutRoot.RenderSize, new Point());
            Point x = new Point(primaryPoint.X + anglePoint.X, primaryPoint.Y);

            double a;
            double b;
            double c;

            a = Math.Sqrt(Math.Pow(primaryPoint.X - anglePoint.X, 2) + Math.Pow(primaryPoint.Y - anglePoint.Y, 2));
            b = anglePoint.X;
            c = Math.Sqrt(Math.Pow(anglePoint.X - x.X, 2) + Math.Pow(anglePoint.Y - x.Y, 2));

            double angleRad = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
            double angleDeg = angleRad * 180 / Math.PI;

            if (primaryPoint.Y < anglePoint.Y)
            {
                angleDeg = 360 - angleDeg;
            }

            return angleDeg;
        }


        private void PopulatePoseLibrary()
        {


            //Pose 0 -举起双手 Both Hands Up
            this.poseLibrary[0] = new Pose();
            this.poseLibrary[0].Title = "nonee";
            this.poseLibrary[0].num = 0;
            this.poseLibrary[0].Angles = new PoseAngle[4];
            this.poseLibrary[0].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 100, 20);
            this.poseLibrary[0].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 95, 20);
            this.poseLibrary[0].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 80, 20);
            this.poseLibrary[0].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 85, 20);


            //Pose 1 - 双手放下 Both Hands Down
            this.poseLibrary[1] = new Pose();
            this.poseLibrary[1].Title = "nonee";
            this.poseLibrary[1].num = 1;
            this.poseLibrary[1].Angles = new PoseAngle[4];
            this.poseLibrary[1].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 260, 20);
            this.poseLibrary[1].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 255, 20);
            this.poseLibrary[1].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 280, 20);
            this.poseLibrary[1].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 285, 20);


            //Pose 2 - 举起左手 Left Up and Right Down
            this.poseLibrary[2] = new Pose();
            this.poseLibrary[2].Title = "gogog";
            this.poseLibrary[2].num = 2;
            this.poseLibrary[2].Angles = new PoseAngle[4];
            this.poseLibrary[2].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 100, 20);
            this.poseLibrary[2].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 95, 20);
            this.poseLibrary[2].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 270, 30);
            this.poseLibrary[2].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 270, 30);


            //Pose 3 - 举起右手 Right Up and Left Down
            this.poseLibrary[3] = new Pose();
            this.poseLibrary[3].Title = "backk";
            this.poseLibrary[3].num = 3;
            this.poseLibrary[3].Angles = new PoseAngle[4];
            this.poseLibrary[3].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 270, 30);
            this.poseLibrary[3].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 270, 30);
            this.poseLibrary[3].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 80, 20);
            this.poseLibrary[3].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 85, 20);


            //Pose 4 - 伸开双臂 Arms Extended
            this.poseLibrary[4] = new Pose();
            this.poseLibrary[4].Title = "stopp";
            this.poseLibrary[4].num = 4;
            this.poseLibrary[4].Angles = new PoseAngle[4];
            this.poseLibrary[4].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 185, 20);
            this.poseLibrary[4].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 190, 20);
            this.poseLibrary[4].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 355, 20);
            this.poseLibrary[4].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 350, 20);


            //Pose 5 - 平抬左手 Arms Extended
            this.poseLibrary[5] = new Pose();
            this.poseLibrary[5].Title = "leftt";
            this.poseLibrary[5].num = 5;
            this.poseLibrary[5].Angles = new PoseAngle[4];
            this.poseLibrary[5].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 185, 20);
            this.poseLibrary[5].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 190, 20);
            this.poseLibrary[5].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 270, 30);
            this.poseLibrary[5].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 270, 30);


            //Pose 6 - 平抬右手 Arms Extended
            this.poseLibrary[6] = new Pose();
            this.poseLibrary[6].Title = "right";
            this.poseLibrary[6].num = 6;
            this.poseLibrary[6].Angles = new PoseAngle[4];
            this.poseLibrary[6].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 270, 30);
            this.poseLibrary[6].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 270, 30);
            this.poseLibrary[6].Angles[2] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 355, 20);
            this.poseLibrary[6].Angles[3] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 350, 20);
        }


        private bool IsPose(Skeleton skeleton, Pose pose)
        {
            bool isPose = true;
            double angle;
            double poseAngle;
            double poseThreshold;
            double loAngle;
            double hiAngle;

            for (int i = 0; i < pose.Angles.Length && isPose; i++)
            {
                poseAngle = pose.Angles[i].Angle;
                poseThreshold = pose.Angles[i].Threshold;
                angle = GetJointAngle(skeleton.Joints[pose.Angles[i].CenterJoint], skeleton.Joints[pose.Angles[i].AngleJoint]);

                hiAngle = poseAngle + poseThreshold;
                loAngle = poseAngle - poseThreshold;

                if (hiAngle >= 360 || loAngle < 0)
                {
                    loAngle = (loAngle < 0) ? 360 + loAngle : loAngle;
                    hiAngle = hiAngle % 360;

                    isPose = !(loAngle > angle && angle > hiAngle);
                }
                else
                {
                    isPose = (loAngle <= angle && hiAngle >= angle);
                }
            }

            return isPose;
        }
        private void GenerateInstructions()
        {
            this.instructionSequence = new int[this.currentLevel];

            for (int i = 0; i < this.currentLevel; i++)
            {
                this.instructionSequence[i] = rnd.Next(0, this.poseLibrary.Length);
            }
        }


        private void DisplayInstructions()
        {
            StringBuilder text = new StringBuilder();
            int instructionsSeq;

            for (int i = 0; i < this.instructionSequence.Length; i++)
            {
                instructionsSeq = this.instructionSequence[i];
                text.AppendFormat("{0}, ", this.poseLibrary[instructionsSeq].Title);
            }

        }


        private static Skeleton GetPrimarySkeleton(Skeleton[] skeletons)
        {
            Skeleton skeleton = null;

            if (skeletons != null)
            {
                //Find the closest skeleton       
                for (int i = 0; i < skeletons.Length; i++)
                {
                    if (skeletons[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (skeleton == null)
                        {
                            skeleton = skeletons[i];
                        }
                        else
                        {
                            if (skeleton.Position.Z > skeletons[i].Position.Z)
                            {
                                skeleton = skeletons[i];
                            }
                        }
                    }
                }
            }

            return skeleton;
        }
        #endregion Methods


        #region Properties
        public KinectSensor KinectDevice
        {
            get { return this.kinectDevice; }
            set
            {
                if (this.kinectDevice != value)
                {
                    //Uninitialize
                    if (this.kinectDevice != null)
                    {
                        this.kinectDevice.Stop();
                        this.kinectDevice.SkeletonFrameReady -= KinectDevice_SkeletonFrameReady;
                        this.kinectDevice.SkeletonStream.Disable();
                        SkeletonViewerElement.KinectDevice = null;
                        this.frameSkeletons = null;
                    }

                    this.kinectDevice = value;

                    //Initialize
                    if (this.kinectDevice != null)
                    {
                        if (this.kinectDevice.Status == KinectStatus.Connected)
                        {
                            this.kinectDevice.SkeletonStream.Enable();
                            this.frameSkeletons = new Skeleton[this.kinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            this.kinectDevice.Start();

                            SkeletonViewerElement.KinectDevice = this.KinectDevice;
                            this.KinectDevice.SkeletonFrameReady += KinectDevice_SkeletonFrameReady;
                        }
                    }
                }
            }
        }
        #endregion Properties

        private void _WaveGesture_GestureDetected(object sender, EventArgs e)
        {

        //        listBox1.Items.Add(string.Format("Wave Detected {0}", DateTime.Now.ToLongTimeString()));
                if (this.socketopen == 1)
                {
                    byte[] data = new byte[1024];
                    data = Encoding.ASCII.GetBytes("wavee");
                    clientSocket.Send(data, data.Length, SocketFlags.None);

                }
            //}
        }
    }
}
