using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.IO.Ports;
using tesseract;
using System.Threading;
using SPHS.AppWindow.parameters;
using SPHS.AppWindow.actions;
using SPHS.AppWindow.models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Globalization;
using ZXing;
using AForge.Video;
using AForge.Video.DirectShow;

namespace SPHS.AppWindow
{
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();
            tabControl1.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            setUp();
            CheckForIllegalCrossThreadCalls = false;
            new Thread(
                () =>
                {
                    while (true)
                    {
                        txtQRCode.Text = SPHSqrCode;
                        txtNumberPlate_in.Text = SPHSnumberPlateIn;
                    }
                })
            { IsBackground = true }.Start();
        }

        #region Define

        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        Image Plate_Draw;
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        public TesseractProcessor full_tesseract = null;
        public TesseractProcessor ch_tesseract = null;
        public TesseractProcessor num_tesseract = null;
        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";
        private static users customerFilter = new users();
        private static users customerGo = new users();
        private static parkingTickets parkingTicketGo = new parkingTickets();

        //int current = 0;
        Capture capture = null;

        // camera
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private List<string> textCameras = new List<string>();

        // variable static load
        private static string SPHSqrCode = "";
        private static int SPHScameraSwitch = 0;
        private static string SPHSnumberPlateIn = "";

        #endregion

        #region layout

        private void setInfomation(users _user, parkingTickets _parkingTicket, bool isIn)
        {
            customerGo = _user;
            parkingTicketGo = _parkingTicket;
            if (isIn)
            {
                lbNameIn.Text = _user.name;
                lbCMTIn.Text = _user.cmt;
                lbPhoneIn.Text = _user.phone;
                lbRoleIn.Text = _user.role;
                lbBalanceIn.Text = _user.balance.ToString();
            }
            else
            {
                lbNameOut.Text = _user.name;
                lbCMTOut.Text = _user.cmt;
                lbPhoneOut.Text = _user.phone;
                lbRoleOut.Text = _user.role;
                lbBalanceOut.Text = _user.balance.ToString();
                lbTimeIn.Text = DateTime.Parse(_parkingTicket.timeIn).ToString();
                int _time = (int)Utils.subDateTime(DateTime.Now.ToString(), _parkingTicket.timeIn);
                lbTimesOut.Text = Utils.convertTimeToString(_time);
                lbTotalOut.Text = Utils.getMoneyByDate(_time, _user.vehicleType).ToString();
                picHistory.Image = Image.FromFile(ParkingTicketAPI.DownLoadFile(_parkingTicket.imageIn));
                if (int.Parse(lbTotalOut.Text) > _user.balance)
                {
                    lbNotEnoughOut.Visible = true;
                }
            }
        }

        private void clearInformation(bool isIn)
        {
            customerGo = new users();
            parkingTicketGo = new parkingTickets();
            if (isIn)
            {
                lbNameIn.Text = Parameter_Special.UNKNOWN_STRING;
                lbCMTIn.Text = Parameter_Special.UNKNOWN_STRING;
                lbPhoneIn.Text = Parameter_Special.UNKNOWN_STRING;
                lbRoleIn.Text = Parameter_Special.UNKNOWN_STRING;
                lbBalanceIn.Text = Parameter_Special.UNKNOWN_STRING;
                txtNumberPlate_in.Text = null;
                pic_vehicle_in.Image = null;
                picNumberPlate_in.Image = null;
            }
            else
            {
                lbNameOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbCMTOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbPhoneOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbRoleOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbBalanceOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbTimeIn.Text = Parameter_Special.UNKNOWN_STRING;
                lbTotalOut.Text = Parameter_Special.UNKNOWN_STRING;
                lbTimesOut.Text = Parameter_Special.UNKNOWN_STRING;
                txtDescriptionOut.Text = null;
                txtNumberPlate_out.Text = null;
                picNumberPlate_out.Image = null;
                pic_vehicle_out.Image = null;
                picHistory.Image = null;
                lbNotEnoughOut.Visible = false;
            }

        }

        private void setUp()
        {
            lbNameEmployee.Text = Parameter_Special.USER_PRESENT.name;
            lbPhoneEmployee.Text = Parameter_Special.USER_PRESENT.phone;
            lbRoleEmployee.Text = Parameter_Special.USER_PRESENT.role;
            List<object> _companies = Utils.getAPI(COLLECTIONS.companies, $"_id={Parameter_Special.USER_PRESENT.companyId}");
            if (_companies.Count > 0)
            {
                Parameter_Special.COMPANY_PRESENT = (companies)_companies[0];
                lbCompanyEmployee.Text = Parameter_Special.COMPANY_PRESENT.name;
                lbAddressComEmployee.Text = Parameter_Special.COMPANY_PRESENT.address;
                cbPortsCompany.DataSource = Parameter_Special.COMPANY_PRESENT.ports;
            }
            cbTypeRegister.DataSource = new string[] {
                VEHICLETYPES.car.ToString(),
                VEHICLETYPES.motorbike.ToString()
            };
        }

        #endregion

        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();
            FileStream fs = new FileStream(urlImage, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            Bitmap image = new Bitmap(img);
            //IF.pictureBox2.Image = image;
            fs.Close();

            FindLicensePlate4(image, out Plate_Draw);
        }

        public void ProcessImage(Bitmap image)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();

            FindLicensePlate4(image, out Plate_Draw);
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private string Ocr(Bitmap image_s, bool isFull, bool isNum = false)
        {
            string temp = "";
            Image<Gray, byte> src = new Image<Gray, byte>(image_s);
            double ratio = 1;
            while (true)
            {
                ratio = (double)CvInvoke.cvCountNonZero(src) / (src.Width * src.Height);
                if (ratio > 0.5) break;
                src = src.Dilate(2);
            }
            Bitmap image = src.ToBitmap();

            TesseractProcessor ocr;
            if (isFull)
                ocr = full_tesseract;
            else if (isNum)
                ocr = num_tesseract;
            else
                ocr = ch_tesseract;

            int cou = 0;
            ocr.Clear();
            ocr.ClearAdaptiveClassifier();
            temp = ocr.Apply(image);
            while (temp.Length > 3)
            {
                Image<Gray, byte> temp2 = new Image<Gray, byte>(image);
                temp2 = temp2.Erode(2);
                image = temp2.ToBitmap();
                ocr.Clear();
                ocr.ClearAdaptiveClassifier();
                temp = ocr.Apply(image);
                cou++;
                if (cou > 10)
                {
                    temp = "";
                    break;
                }
            }
            return temp;

        }

        public void FindLicensePlate4(Bitmap image, out Image plateDraw)
        {
            plateDraw = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            Image dst = image;
            HaarCascade haar = new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml");
            for (float i = 0; i <= 20; i = i + 3)
            {
                for (float s = -1; s <= 1 && s + i != 1; s += 2)
                {
                    src = RotateImage(dst, i * s);
                    PlateImagesList.Clear();
                    frame = new Image<Bgr, byte>(src);
                    using (Image<Gray, byte> grayframe = new Image<Gray, byte>(src))
                    {
                        var faces =
                       grayframe.DetectHaarCascade(haar, 1.1, 8, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(0, 0))[0];
                        foreach (var face in faces)
                        {
                            Image<Bgr, byte> tmp = frame.Copy();
                            tmp.ROI = face.rect;

                            frame.Draw(face.rect, new Bgr(Color.Blue), 2);

                            PlateImagesList.Add(tmp);

                            isface = true;
                        }
                        if (isface)
                        {
                            Image<Bgr, byte> showimg = frame.Clone();
                            plateDraw = (Image)showimg.ToBitmap();
                            //IF.pictureBox2.Image = showimg.ToBitmap();
                            if (PlateImagesList.Count > 1)
                            {
                                for (int k = 1; k < PlateImagesList.Count; k++)
                                {
                                    if (PlateImagesList[0].Width < PlateImagesList[k].Width)
                                    {
                                        PlateImagesList[0] = PlateImagesList[k];
                                    }
                                }
                            }
                            PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                            return;
                        }


                    }
                }
            }


        }

        private void Reconize(bool picInorOut, string link, out Image hinhbienso, out string bienso, out string bienso_text)
        {
            for (int i = 0; i < box.Length; i++)
            {
                this.Controls.Remove(box[i]);
            }

            hinhbienso = null;
            bienso = "";
            bienso_text = "";
            ProcessImage(link);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                FindContours con = new FindContours();
                Bitmap color;
                int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);  // find contour
                if (picInorOut)
                    picNumberPlate_in.Image = color;
                else
                    picNumberPlate_out.Image = color;
                //IF.pictureBox1.Image = color;
                hinhbienso = Plate_Draw;
                //picNumberPlate_out.Image = grayframe;
                //IF.pictureBox3.Image = grayframe;
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                string zz = "";

                // filter and sort number
                List<Bitmap> bmp = new List<Bitmap>();
                List<int> erode = new List<int>();
                List<Rectangle> up = new List<Rectangle>();
                List<Rectangle> dow = new List<Rectangle>();
                int up_y = 0, dow_y = 0;
                bool flag_up = false;

                int di = 0;

                if (listRect == null) return;

                for (int i = 0; i < listRect.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(listRect[i], grayframe.PixelFormat);
                    int cou = 0;
                    full_tesseract.Clear();
                    full_tesseract.ClearAdaptiveClassifier();
                    string temp = full_tesseract.Apply(ch);
                    while (temp.Length > 3)
                    {
                        Image<Gray, byte> temp2 = new Image<Gray, byte>(ch);
                        temp2 = temp2.Erode(2);
                        ch = temp2.ToBitmap();
                        full_tesseract.Clear();
                        full_tesseract.ClearAdaptiveClassifier();
                        temp = full_tesseract.Apply(ch);
                        cou++;
                        if (cou > 10)
                        {
                            listRect.RemoveAt(i);
                            i--;
                            di = 0;
                            break;
                        }
                        di = cou;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    for (int j = i; j < listRect.Count; j++)
                    {
                        if (listRect[i].Y > listRect[j].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[j].Y;
                            dow_y = listRect[i].Y;
                            break;
                        }
                        else if (listRect[j].Y > listRect[i].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[i].Y;
                            dow_y = listRect[j].Y;
                            break;
                        }
                        if (flag_up == true) break;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    if (listRect[i].Y < up_y + 50 && listRect[i].Y > up_y - 50)
                    {
                        up.Add(listRect[i]);
                    }
                    else if (listRect[i].Y < dow_y + 50 && listRect[i].Y > dow_y - 50)
                    {
                        dow.Add(listRect[i]);
                    }
                }

                if (flag_up == false) dow = listRect;

                for (int i = 0; i < up.Count; i++)
                {
                    for (int j = i; j < up.Count; j++)
                    {
                        if (up[i].X > up[j].X)
                        {
                            Rectangle w = up[i];
                            up[i] = up[j];
                            up[j] = w;
                        }
                    }
                }
                for (int i = 0; i < dow.Count; i++)
                {
                    for (int j = i; j < dow.Count; j++)
                    {
                        if (dow[i].X > dow[j].X)
                        {
                            Rectangle w = dow[i];
                            dow[i] = dow[j];
                            dow[j] = w;
                        }
                    }
                }

                int x = 12;
                int c_x = 0;

                for (int i = 0; i < up.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(up[i], grayframe.PixelFormat);
                    Bitmap o = ch;
                    string temp;
                    if (i < 2)
                    {
                        temp = Ocr(ch, false, true); // nhan dien so
                    }
                    else
                    {
                        temp = Ocr(ch, false, false);// nhan dien chu
                    }

                    zz += temp;
                    box[i].Location = new Point(x + i * 50, 290);
                    box[i].Size = new Size(50, 100);
                    box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i].Image = ch;
                    box[i].Update();
                    //IF.Controls.Add(box[i]);
                    c_x++;
                }
                zz += "\r\n";
                for (int i = 0; i < dow.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(dow[i], grayframe.PixelFormat);
                    //ch = con.Erodetion(ch);
                    string temp = Ocr(ch, false, true); // nhan dien so
                    zz += temp;
                    box[i + c_x].Location = new Point(x + i * 50, 390);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;
                    box[i + c_x].Update();
                    //IF.Controls.Add(box[i + c_x]);
                }
                bienso = zz.Replace("\n", "");
                bienso = bienso.Replace("\r", "");
                //IF.textBox6.Text = zz;
                bienso_text = zz;

            }
        }

        private void Reconize(bool picInorOut, Bitmap imageInput, out Image hinhbienso, out string bienso, out string bienso_text)
        {
            for (int i = 0; i < box.Length; i++)
            {
                this.Controls.Remove(box[i]);
            }

            hinhbienso = null;
            bienso = "";
            bienso_text = "";
            ProcessImage(imageInput);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                FindContours con = new FindContours();
                Bitmap color;
                int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);  // find contour
                if (picInorOut)
                    picNumberPlate_in.Image = color;
                else
                    picNumberPlate_out.Image = color;
                //IF.pictureBox1.Image = color;
                hinhbienso = Plate_Draw;
                //picNumberPlate_out.Image = grayframe;
                //IF.pictureBox3.Image = grayframe;
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                string zz = "";

                // filter and sort number
                List<Bitmap> bmp = new List<Bitmap>();
                List<int> erode = new List<int>();
                List<Rectangle> up = new List<Rectangle>();
                List<Rectangle> dow = new List<Rectangle>();
                int up_y = 0, dow_y = 0;
                bool flag_up = false;

                int di = 0;

                if (listRect == null) return;

                for (int i = 0; i < listRect.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(listRect[i], grayframe.PixelFormat);
                    int cou = 0;
                    full_tesseract.Clear();
                    full_tesseract.ClearAdaptiveClassifier();
                    string temp = full_tesseract.Apply(ch);
                    while (temp.Length > 3)
                    {
                        Image<Gray, byte> temp2 = new Image<Gray, byte>(ch);
                        temp2 = temp2.Erode(2);
                        ch = temp2.ToBitmap();
                        full_tesseract.Clear();
                        full_tesseract.ClearAdaptiveClassifier();
                        temp = full_tesseract.Apply(ch);
                        cou++;
                        if (cou > 10)
                        {
                            listRect.RemoveAt(i);
                            i--;
                            di = 0;
                            break;
                        }
                        di = cou;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    for (int j = i; j < listRect.Count; j++)
                    {
                        if (listRect[i].Y > listRect[j].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[j].Y;
                            dow_y = listRect[i].Y;
                            break;
                        }
                        else if (listRect[j].Y > listRect[i].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[i].Y;
                            dow_y = listRect[j].Y;
                            break;
                        }
                        if (flag_up == true) break;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    if (listRect[i].Y < up_y + 50 && listRect[i].Y > up_y - 50)
                    {
                        up.Add(listRect[i]);
                    }
                    else if (listRect[i].Y < dow_y + 50 && listRect[i].Y > dow_y - 50)
                    {
                        dow.Add(listRect[i]);
                    }
                }

                if (flag_up == false) dow = listRect;

                for (int i = 0; i < up.Count; i++)
                {
                    for (int j = i; j < up.Count; j++)
                    {
                        if (up[i].X > up[j].X)
                        {
                            Rectangle w = up[i];
                            up[i] = up[j];
                            up[j] = w;
                        }
                    }
                }
                for (int i = 0; i < dow.Count; i++)
                {
                    for (int j = i; j < dow.Count; j++)
                    {
                        if (dow[i].X > dow[j].X)
                        {
                            Rectangle w = dow[i];
                            dow[i] = dow[j];
                            dow[j] = w;
                        }
                    }
                }

                int x = 12;
                int c_x = 0;

                for (int i = 0; i < up.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(up[i], grayframe.PixelFormat);
                    Bitmap o = ch;
                    string temp;
                    if (i < 2)
                    {
                        temp = Ocr(ch, false, true); // nhan dien so
                    }
                    else
                    {
                        temp = Ocr(ch, false, false);// nhan dien chu
                    }

                    zz += temp;
                    box[i].Location = new Point(x + i * 50, 290);
                    box[i].Size = new Size(50, 100);
                    box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i].Image = ch;
                    box[i].Update();
                    //IF.Controls.Add(box[i]);
                    c_x++;
                }
                zz += "\r\n";
                for (int i = 0; i < dow.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(dow[i], grayframe.PixelFormat);
                    //ch = con.Erodetion(ch);
                    string temp = Ocr(ch, false, true); // nhan dien so
                    zz += temp;
                    box[i + c_x].Location = new Point(x + i * 50, 390);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;
                    box[i + c_x].Update();
                    //IF.Controls.Add(box[i + c_x]);
                }
                bienso = zz.Replace("\n", "");
                bienso = bienso.Replace("\r", "");
                //IF.textBox6.Text = zz;
                bienso_text = zz;

            }
        }


        private void MainApp_Load(object sender, EventArgs e)
        {
            //capture = new Emgu.CV.Capture();
            //timer1.Enabled = true;

            //IF = new ImageForm();

            full_tesseract = new TesseractProcessor();
            bool succeed = full_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            full_tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTVXY1234567890").ToString();

            ch_tesseract = new TesseractProcessor();
            succeed = ch_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            ch_tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTUVXY").ToString();

            num_tesseract = new TesseractProcessor();
            succeed = num_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            num_tesseract.SetVariable("tessedit_char_whitelist", "1234567890").ToString();


            m_path = System.Environment.CurrentDirectory + "\\";
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < box.Length; i++)
            {
                box[i] = new PictureBox();
            }

            // init load camera
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                textCameras.Add(filterInfo.Name);
            }
            videoCaptureDevice = new VideoCaptureDevice();
        }

        private void btnLoadImageIn_Click(object sender, EventArgs e)
        {
            // SetupCameraCapture((int)CAMERASWITCH.CameraVehicleIn, 0);

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;

            Image temp1;
            string temp2, temp3;
            Reconize(true, startupPath, out temp1, out temp2, out temp3);
            pic_vehicle_in.Image = temp1;
            if (string.IsNullOrEmpty(temp3))
                txtNumberPlate_in.Text = "Cannot recognize license plate !";
            else
                txtNumberPlate_in.Text = temp3;

            string _numberPlate = Utils.convertNumberPlate(txtNumberPlate_in.Text);
            List<object> _users = Utils.getAPI(COLLECTIONS.users, $"numberPlate={_numberPlate}");
            if (_users.Count == 1)
            {
                users _user = (users)_users[0];
                setInfomation(_user, null, true);
                txtNumberPlate_in.Text = temp3;
            }
        }

        private void btnLoadImageOut_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;

            Image temp1;
            string temp2, temp3;
            Reconize(false, startupPath, out temp1, out temp2, out temp3);
            pic_vehicle_out.Image = temp1;
            if (temp3 == "")
                txtNumberPlate_out.Text = "Cannot recognize license plate !";
            else
                txtNumberPlate_out.Text = temp3;

            string _numberPlate = Utils.convertNumberPlate(txtNumberPlate_out.Text);
            List<object> _users = Utils.getAPI(COLLECTIONS.users, $"numberPlate={_numberPlate}");
            if (_users.Count == 1)
            {
                users _user = (users)_users[0];
                List<object> _parkingTickets = Utils.getAPI(COLLECTIONS.parkingtickets, $"userId={_user._id}&" + @"$sort={timeIn: -1}");
                if (_parkingTickets.Count > 0)
                {
                    parkingTickets _parkingTicket = (parkingTickets)_parkingTickets[0];
                    if (_parkingTicket.timeOut == null)
                    {
                        setInfomation(_user, _parkingTicket, false);
                        return;
                    }
                }
            }
            MessageBox.Show("Something error!");
        }

        private void btnSaveIn_Click(object sender, EventArgs e)
        {
            bool verify = false;
            if (customerGo.cardIds.Contains(txtCardIdScan.Text))
                verify = true;
            if (txtQRCode.Text.Contains(customerGo.companyId) && txtQRCode.Text.Contains(customerGo.account) && txtQRCode.Text.Contains(customerGo.numberPlate))
                verify = true;

            string _urlImage = ParkingTicketAPI.UploadFile(Utils.ImageToByteArray(pic_vehicle_in.Image), true);
            if (!verify)
            {
                if (_urlImage != null)
                {
                    var _post = ParkingTicketAPI.post(new parkingTickets()
                    {
                        port = cbPortsCompany.Text,
                        companyId = Parameter_Special.USER_PRESENT.companyId,
                        timeIn = DateTime.Now.ToString(),
                        author = Parameter_Special.USER_PRESENT._id,
                        userId = customerGo._id,
                        imageIn = _urlImage,
                        description = txtCardIdScan.Text
                    });
                    clearInformation(true);
                }
                return;
            }

            if (_urlImage != null)
            {
                var _post = ParkingTicketAPI.post(new parkingTickets()
                {
                    port = cbPortsCompany.Text,
                    companyId = Parameter_Special.USER_PRESENT.companyId,
                    timeIn = DateTime.Now.ToString(),
                    author = Parameter_Special.USER_PRESENT._id,
                    userId = customerGo._id,
                    imageIn = _urlImage
                });
                clearInformation(true);
            }
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(parkingTicketGo.description) && parkingTicketGo.description == txtCardIdScan.Text)
            {
                MessageBox.Show("OK");
                return;
            }
            if (lbNotEnoughOut.Visible)
            {
                MessageBox.Show("Balance not enough to pay ticket");
                return;
            }
            string _urlImage = ParkingTicketAPI.UploadFile(Utils.ImageToByteArray(pic_vehicle_out.Image), false);
            if (_urlImage == null)
            {
                MessageBox.Show("Something error!");
                return;
            }
            var _put = Utils.putAPI(COLLECTIONS.parkingtickets, new parkingTickets()
            {
                _id = parkingTicketGo._id,
                timeOut = DateTime.Now.ToString(),
                description = txtDescriptionOut.Text,
                imageOut = _urlImage
            });
            if (_put)
            {
                int _money = 0;
                int.TryParse(lbTotalOut.Text, out _money);
                var _putBalace = Utils.putAPI(COLLECTIONS.users, new users()
                {
                    _id = customerGo._id,
                    balance = customerGo.balance - _money
                });
                clearInformation(false);
                //if (_putBalace)
                //{
                //    var _uploadFile = ParkingTicketAPI.upload(parkingTicketGo, Utils.ImageToByteArray(pic_vehicle_out.Image), false);
                //    if (_uploadFile)
                //    {
                //        clearInformation(false);
                //        return;
                //    }
                //}
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string _query = null;
            if (txtNumberPlateFilter.Text.Length > 0)
                _query = $"numberPlate={txtNumberPlateFilter.Text}";

            if (txtPhoneFilter.Text.Length > 0)
            {
                if (_query != null)
                    _query += $"&phone={txtPhoneFilter.Text}";
                else
                    _query = $"phone={txtPhoneFilter.Text}";
            }

            List<object> _users = Utils.getAPI(COLLECTIONS.users, _query);
            if (_users.Count == 1)
            {
                customerFilter = (users)_users[0];
                grCustomerInfo.Visible = true;
                grTransaction.Visible = true;
                lbCustomerAccount.Text = customerFilter.account;
                lbCustomerName.Text = customerFilter.name;
                lbCustomerCMT.Text = customerFilter.cmt;
                lbCustomerPhone.Text = customerFilter.phone;
                lbCustomerEmail.Text = customerFilter.email;
                lbCustomerNumber.Text = customerFilter.numberPlate;
                lbCustomerColor.Text = customerFilter.vehicleColor;
                lbCustomerBranch.Text = customerFilter.vehicleBranch;
                lbCustomerType.Text = customerFilter.vehicleType;
                lbCustomerBalance.Text = customerFilter.balance.ToString();
                lbCustomerDescription.Text = customerFilter.description;
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtPhoneFilter.Text = null;
            txtNumberPlateFilter.Text = null;
            grCustomerInfo.Visible = false;
            grTransaction.Visible = false;
            customerFilter = new users();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            int _money = 0;
            if (int.TryParse(txtCustomerMoney.Text, out _money))
            {
                var _post = Utils.postAPI(COLLECTIONS.transactions, new transactions()
                {
                    author = Parameter_Special.USER_PRESENT._id,
                    userId = customerFilter._id,
                    money = _money,
                    description = txtCustomerTransaction.Text
                });
                if (!_post)
                {
                    MessageBox.Show("Error");
                    return;
                }
                var _update = Utils.putAPI(COLLECTIONS.users, new users()
                {
                    _id = customerFilter._id,
                    balance = customerFilter.balance + _money
                });
                if (!_update)
                {
                    MessageBox.Show("Error");
                    return;
                }
                customerFilter.balance = customerFilter.balance + _money;
                lbCustomerBalance.Text = customerFilter.balance.ToString();
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show("Error Money!");
            }
        }

        private void btnRegisterCustomer_Click(object sender, EventArgs e)
        {
            int _balance = 0;
            int.TryParse(txtBalanceRegister.Text, out _balance);
            var _user = Utils.postAPI(COLLECTIONS.users, new users()
            {
                account = txtAccountResgister.Text,
                password = txtPasswordRegister.Text,
                name = txtNameRegister.Text,
                cmt = txtCMTRegister.Text,
                phone = txtPhoneRegister.Text,
                email = txtEmailRegister.Text,
                numberPlate = txtNumberRegister.Text,
                balance = _balance,
                vehicleColor = txtColorRegister.Text,
                vehicleBranch = txtBalanceRegister.Text,
                vehicleType = cbTypeRegister.Text,
                role = COLLECTIONS.users.ToString(),
                companyId = Parameter_Special.USER_PRESENT.companyId
            });
            if (_user)
                MessageBox.Show("OK");
            else
                MessageBox.Show("Error");
        }

        private void btnImageQR_Click(object sender, EventArgs e)
        {
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            //dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            //{
            //    return;
            //}
            //Bitmap bm = new Bitmap(dlg.FileName);
            //picQRCode.Image = bm;

            //string qr_code = Utils.ScanQRCodeByBitMap(bm);
            //if (qr_code != null)
            //    txtQRCode.Text = qr_code;

            //SPHScameraSwitch = (int)CAMERASWITCH.CameraQRCode;
            //videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            //videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            //videoCaptureDevice.Start();

            SetupCameraCapture((int)CAMERASWITCH.CameraQRCode, 0);
        }

        private void btnPassQR_Click(object sender, EventArgs e)
        {
            SPHSqrCode = "";
            picQRCode.Image = null;
            txtQRCode.Text = "";
            videoCaptureDevice.Stop();
        }

        private void SetupCameraCapture(int _switchCamera, int _index)
        {
            SPHScameraSwitch = _switchCamera;
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[_index].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (SPHScameraSwitch == (int)CAMERASWITCH.CameraQRCode)
            {
                picQRCode.Image = (Bitmap)eventArgs.Frame.Clone();
                string qr_code = Utils.ScanQRCodeByBitMap((Bitmap)eventArgs.Frame.Clone());
                if (qr_code != null && string.IsNullOrEmpty(SPHSqrCode))
                {
                    SPHSqrCode = qr_code;
                    //var video = sender as VideoCaptureDevice;
                    //video.Stop();
                }
            }
            if (SPHScameraSwitch == (int)CAMERASWITCH.CameraVehicleIn)
            {
                try
                {

                    pic_vehicle_in.Image = (Bitmap)eventArgs.Frame.Clone();
                    Image temp1;
                    string temp2, temp3;
                    Reconize(true, (Bitmap)pic_vehicle_in.Image, out temp1, out temp2, out temp3);
                    pic_vehicle_in.Image = temp1;

                    string _numberPlate = Utils.convertNumberPlate(temp3);
                    //List<object> _users = Utils.getAPI(COLLECTIONS.users, $"numberPlate={_numberPlate}");
                    //if (_users.Count == 1)
                    //{
                    //    users _user = (users)_users[0];
                    //    setInfomation(_user, null, true);
                    //}
                    if (!string.IsNullOrEmpty(_numberPlate))
                    {
                        SPHSnumberPlateIn = temp3;
                    }
                }
                catch { }
            }
        }

        private void MainApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }
    }
}
