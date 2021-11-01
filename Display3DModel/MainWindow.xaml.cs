using HelixToolkit.Wpf;
using System;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.Wpf.SharpDX.Assimp;
using HelixToolkit.Wpf.SharpDX.Controls;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
//using Media3D = System.Windows.Media.Media3D;
using Vector3 = global::SharpDX.Vector3;



namespace Display3DModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Path to the model file
        //private const string MODEL_PATH = "3DModel/File01.obj";
        //private const string MODEL_PATH = "C:/Users/KUMRAI Teerawat/source/repos/display3D-master/Display3DModel/bin/Debug/Model/Model.stl";
        private const string path = "C:/Users/thili/source/repos/2D_3D_integration/Display3DModel/bin/Debug/Model/woman_dance/women1_dance5_v5-2.fbx"; //Solus_The_Knight.fbx";//
        //private const string path = "C:/Users/KUMRAI Teerawat/source/repos/display3D-test/Display3DModel/bin/Debug/Model/walk_fbx/rp_nathan_animated_003_walking.fbx";
        //private const string MODEL_PATH = "C:/Users/KUMRAI Teerawat/source/repos/display3D-master/Display3DModel/bin/Debug/Model/Walk.stl";
        //private const string MODEL_PATH = "C:/Users/KUMRAI Teerawat/source/repos/display3D-master/Display3DModel/bin/Debug/Model/Walk.fbx";

        public SceneNodeGroupModel3D ModelGroup { get; } = new SceneNodeGroupModel3D();
        public AnimationRepeatMode[] RepeatModes { get; } = new AnimationRepeatMode[] { AnimationRepeatMode.Loop, AnimationRepeatMode.PlayOnce, AnimationRepeatMode.PlayOnceHold };

        private HelixToolkitScene scene;
        private NodeAnimationUpdater animationUpdater;

        private CompositionTargetEx compositeHelper = new CompositionTargetEx();
        public string[] Animations { set; get; }



        /*private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the MaskedTextBox control.
            MaskedTextBox mtbDate = new MaskedTextBox("00/00/0000");

            // Assign the MaskedTextBox control as the host control's child.
            host.Child = mtbDate;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.grid1.Children.Add(host);
        }*/


        public bool playing = false;
        public MainWindow()
        {

            InitializeComponent();

            // Create the interop host control.
            //System.Windows.Forms.Integration.WindowsFormsHost host =
            //new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the MaskedTextBox control.
            //MaskedTextBox mtbDate = new MaskedTextBox("00/00/0000");



            // Assign the MaskedTextBox control as the host control's child.
            //host.Child = mtbDate;
            //MainWindow mainWindow = new MainWindow();


            // Add the interop host control to the Grid
            // control's collection of child controls.
            //this.grid1.Children.Add(host);

            

            MyPersCam.Position = new Point3D(0,1200,0);
            //MyPersCam.FarPlaneDistance = ;

            string path2 = Directory.GetCurrentDirectory();

            GroupModel3D Group = new GroupModel3D();

            var importer = new Importer();
            scene = importer.Load(path2 + "/women1_dance5_v7.fbx");
            Animations = scene.Animations.Select(x => x.Name).ToArray();


            System.Uri sVideo = new System.Uri(path2 + "/final.mp4");
            mePlayer.Source = sVideo;

            string valueString = "Your string";
            Console.WriteLine(valueString);
            

            //var curr = scene.Animations.Where(x => x.Name == Animations[0]).FirstOrDefault();
            //animationUpdater = new NodeAnimationUpdater(curr);

            //animationUpdater.RepeatMode = AnimationRepeatMode.PlayOnce;
            

            ModelGroup.AddNode(scene.Root);

            //Group.Children.Add(ModelGroup);
            //compositeHelper.Rendering += CompositeHelper_Rendering;
            // Add to view port
            viewport3DX.Items.Add(ModelGroup);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            //mePlayer.Stop();
            //animationUpdater.Reset();
            //animationUpdater.Speed = 0;
            //playing = false;

            //Autoplay the video
            //mePlayer.Play();
            //Auto replay the video
            //mePlayer.MediaEnded += new RoutedEventHandler(mePlayer_MediaEnded);
        }

        void mePlayer_MediaEnded(object sender, EventArgs e)
        {
            mePlayer.Position = TimeSpan.FromSeconds(0);
            mePlayer.Play();
        }

        private void CompositeHelper_Rendering(object sender, System.Windows.Media.RenderingEventArgs e)
        {
            if (animationUpdater != null)
            {
                animationUpdater.Update(Stopwatch.GetTimestamp(), Stopwatch.Frequency);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (playing == false)
            {
                playing = true;
                var curr = scene.Animations.Where(x => x.Name == Animations[0]).FirstOrDefault();
                animationUpdater = new NodeAnimationUpdater(curr);
                //animationUpdater.Animation.StartTime = animationUpdater.Animation.StartTime + 3;
                //string valueString = "Animation starttime";
                Console.WriteLine("Animation starttime");
                Console.WriteLine(value: animationUpdater.Animation.StartTime);
                Console.WriteLine("Animation endtime");
                Console.WriteLine(value: animationUpdater.Animation.EndTime);
                animationUpdater.RepeatMode = AnimationRepeatMode.PlayOnce;
                animationUpdater.Speed = 1.005f;
                compositeHelper.Rendering += CompositeHelper_Rendering;

                //var task = ChattyWriter();
                //task.Wait();
                //System.Threading.Thread.Sleep(2000);
                //Task.Delay(4000).Wait();
                mePlayer.Play();
            }

            else
            {
                animationUpdater.Speed = 1.005f;
            }
            
            
            //viewport3DX.Items.Remove(ModelGroup);
            //viewport3DX.Items.Add(ModelGroup);
            
            
        }

        public async Task ChattyWriter()
        {
            int count = 0;
            while (true)
            {
                var message = String.Format("Chatty Writer number {0}", count);
                Trace.WriteLine(message);
                count++;
                await Task.Delay(1000);
    }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
            animationUpdater.Speed = 0;
            //var curr = scene.Animations.Where(x => x.Name == Animations[0]).FirstOrDefault();
            //Console.WriteLine("Value of curr is ");
            
            
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
            //viewport3DX.Items.Remove(ModelGroup);
            animationUpdater.Reset();
            animationUpdater.Speed = 0;
            playing = false;
        }

        private void checkReplay_Checked(object sender, RoutedEventArgs e)
        {
            mePlayer.Position = TimeSpan.FromSeconds(0);
            mePlayer.MediaEnded += new RoutedEventHandler(mePlayer_MediaEnded);
            viewport3DX.Items.Remove(ModelGroup);
            viewport3DX.Items.Add(ModelGroup);
            var curr = scene.Animations.Where(x => x.Name == Animations[0]).FirstOrDefault();
            animationUpdater = new NodeAnimationUpdater(curr);
            animationUpdater.RepeatMode = AnimationRepeatMode.Loop;

        }

        private void checkReplay_Unchecked(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }
    }
}