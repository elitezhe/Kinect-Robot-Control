using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Microsoft.Kinect;


namespace kinectControlRobot
{
    /// <summary>
    /// </summary>
    public partial class SkeletonViewer : UserControl
    {
        #region Member Variables
        private readonly Brush[] _SkeletonBrushes = new Brush[] { Brushes.Black, Brushes.Crimson, Brushes.Indigo, Brushes.DodgerBlue, Brushes.Purple, Brushes.Pink };
        private Skeleton[] _FrameSkeletons;
        #endregion Member Variables


        #region Constructor
        public SkeletonViewer()
        {
            InitializeComponent();
        }
        #endregion Constructor


        #region Methods
        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonsPanel.Children.Clear();

            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    if (this.IsEnabled)
                    {
                        frame.CopySkeletonDataTo(this._FrameSkeletons);

                        for (int i = 0; i < this._FrameSkeletons.Length; i++)
                        {
                            DrawSkeleton(this._FrameSkeletons[i], this._SkeletonBrushes[i]);
                        }
                    }
                }
            }
        }


        private void DrawSkeleton(Skeleton skeleton, Brush brush)
        {
            if (skeleton != null && skeleton.TrackingState == SkeletonTrackingState.Tracked)
            {
                //Draw head and torso
                //     Polyline figure = CreateFigure(skeleton, brush, new [] { JointType.Head, JointType.ShoulderCenter, JointType.ShoulderLeft, JointType.Spine,
                //                                                                  JointType.ShoulderRight, JointType.ShoulderCenter, JointType.HipCenter,
                //                                                                  JointType.HipLeft, JointType.Spine, JointType.HipRight, JointType.HipCenter});  
                Polyline figure = CreateFigure(skeleton, brush, new[] { JointType.Head, JointType.ShoulderCenter, JointType.ShoulderLeft });
                SkeletonsPanel.Children.Add(figure);

                figure = CreateFigure(skeleton, brush, new[] { JointType.ShoulderRight, JointType.ShoulderCenter, JointType.Spine, JointType.HipCenter });
                SkeletonsPanel.Children.Add(figure);

                figure = CreateFigure(skeleton, brush, new[] { JointType.HipLeft, JointType.HipCenter, JointType.HipRight });
                SkeletonsPanel.Children.Add(figure);
                //Draw left leg
                figure = CreateFigure(skeleton, brush, new[] { JointType.HipLeft, JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft });
                SkeletonsPanel.Children.Add(figure);

                //Draw right leg
                figure = CreateFigure(skeleton, brush, new[] { JointType.HipRight, JointType.KneeRight, JointType.AnkleRight, JointType.FootRight });
                SkeletonsPanel.Children.Add(figure);

                //Draw left arm
                figure = CreateFigure(skeleton, brush, new[] { JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft });
                SkeletonsPanel.Children.Add(figure);

                //Draw right arm
                figure = CreateFigure(skeleton, brush, new[] { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight });
                SkeletonsPanel.Children.Add(figure);
            }
        }


        private Polyline CreateFigure(Skeleton skeleton, Brush brush, JointType[] joints)
        {
            Polyline figure = new Polyline();

            figure.StrokeThickness = 10;
            figure.Stroke = brush;

            for (int i = 0; i < joints.Length; i++)
            {
                figure.Points.Add(GetJointPoint(skeleton.Joints[joints[i]]));
            }

            return figure;
        }


        private Point GetJointPoint(Joint joint)
        {
           // DepthImagePoint point = this.KinectDevice.MapSkeletonPointToDepth(joint.Position, KinectDevice.DepthStream.Format);
            DepthImagePoint point = this.KinectDevice.CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, KinectDevice.DepthStream.Format);
            point.X *= (int)this.LayoutRoot.ActualWidth / KinectDevice.DepthStream.FrameWidth;          
            point.Y *= (int)this.LayoutRoot.ActualHeight / KinectDevice.DepthStream.FrameHeight;

            return new Point(point.X, point.Y);
        }
        #endregion Methods


        #region Properties
        #region KinectDevice
        protected const string KinectDevicePropertyName = "KinectDevice";
        public static readonly DependencyProperty KinectDeviceProperty = DependencyProperty.Register(KinectDevicePropertyName, typeof(KinectSensor), typeof(SkeletonViewer), new PropertyMetadata(null, KinectDeviceChanged));


        private static void KinectDeviceChanged(DependencyObject owner, DependencyPropertyChangedEventArgs e)
        {
            SkeletonViewer viewer = (SkeletonViewer)owner;

            if (e.OldValue != null)
            {
                ((KinectSensor)e.OldValue).SkeletonFrameReady -= viewer.KinectDevice_SkeletonFrameReady;
                viewer._FrameSkeletons = null;
            }

            if (e.NewValue != null)
            {
                viewer.KinectDevice = (KinectSensor)e.NewValue;
                viewer.KinectDevice.SkeletonFrameReady += viewer.KinectDevice_SkeletonFrameReady;
                viewer._FrameSkeletons = new Skeleton[viewer.KinectDevice.SkeletonStream.FrameSkeletonArrayLength];
            }
        }


        public KinectSensor KinectDevice
        {
            get { return (KinectSensor)GetValue(KinectDeviceProperty); }
            set { SetValue(KinectDeviceProperty, value); }
        }
        #endregion KinectDevice
        #endregion Properties
    }
}
