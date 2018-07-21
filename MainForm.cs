using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Data;
using System.ComponentModel;
using System.Text; 
namespace MultiFaceRec

{
    public partial class FrmPrincipal : Form
    {

        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;

        private SpeechRecognitionEngine recognitionEngine; 
        public FrmPrincipal()
        {
            InitializeComponent();
          
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels+1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            
            }
            catch(Exception e)
            {
                
                MessageBox.Show("Lets Add Some face and eyes", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           
            }

            recognitionEngine = new SpeechRecognitionEngine();


            recognitionEngine.SetInputToDefaultAudioDevice();

            recognitionEngine.SpeechRecognized += (s, args) =>
            {

                foreach (RecognizedWordUnit word in args.Result.Words)
                {

                    if (word.Confidence > 0.8f)


                        textBox2.Text += word.Text + " ";

                }

                textBox3.Text += Environment.NewLine;

            };

            recognitionEngine.LoadGrammar(new DictationGrammar()); 

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            grabber = new Capture();
            grabber.QueryFrame();
            
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            if (textBox1.Text == "")
            {

                MessageBox.Show("Please enter your name", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            else
            {
                //try
                //{

                    ContTrain = ContTrain + 1;


                    gray = grabber.QueryGrayFrame().Resize(800, 660, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(40, 40));


                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }


                    //TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    trainingImages.Add(TrainedFace);
                    labels.Add(textBox1.Text);


                    imageBox1.Image = TrainedFace;


                    File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");


                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                        File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                    }

                    MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                //}
                //catch
                //{
                //    MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            
            currentFrame = grabber.QueryFrame().Resize(800, 660, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    
                    gray = currentFrame.Convert<Gray, Byte>();

                   
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                  face,
                  1.2,
                  10,
                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                  new Size(20, 20));

                   
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        t = t + 1;
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    
                        currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                        if (trainingImages.ToArray().Length != 0)
                        {
                           
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                      
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);

                            
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                        }

                            NamePersons[t-1] = name;
                            NamePersons.Add("");


                  
                        label3.Text = facesDetected[0].Length.ToString();
                       
                       
                         

                    }

                        t = 0;

                       
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                    }
                    
                    imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
                    
                    NamePersons.Clear();

                }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void imageBoxFrameGrabber_Click(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            //grabber = new Capture();
            //grabber.QueryFrame();

            //Application.Idle += new EventHandler(FrameGrabber);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            recognitionEngine.RecognizeAsyncStop(); 
        }

    }
}