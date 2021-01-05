using AForge.Video.DirectShow;
using SPHS.AppWindow.actions;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow
{
    public partial class SimulinkDevice : Form
    {
        private List<companies> _companies = new List<companies>();
        private List<devices> _devices = new List<devices>();
        private static int indexDevice = 0;
        private static int indexStatus = 0;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        public SimulinkDevice()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            getToken();
            getCompanies();

            // init load camera
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoCaptureDevice = new VideoCaptureDevice();
            SetupCameraCapture();
            CheckForIllegalCrossThreadCalls = false;
            new Thread(
                () =>
                {
                    while (true)
                    {
                        try
                        {
                            switch (indexStatus)
                            {
                                case 1:
                                    lbStatusAction.ForeColor = Color.Red;
                                    lbStatusAction.Text = Parameter_Special.STATUS_ACCESS_VERIFY[1];
                                    break;
                                case 2:
                                    lbStatusAction.ForeColor = Color.Yellow;
                                    lbStatusAction.Text = Parameter_Special.STATUS_ACCESS_VERIFY[2];
                                    break;
                                case 3:
                                    lbStatusAction.ForeColor = Color.Green;
                                    lbStatusAction.Text = Parameter_Special.STATUS_ACCESS_VERIFY[3];
                                    break;
                                default:
                                    lbStatusAction.ForeColor = Color.Black;
                                    lbStatusAction.Text = Parameter_Special.STATUS_ACCESS_VERIFY[0];
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            // ignore
                        }

                    }
                })
            { IsBackground = true }.Start();
        }

        private void getToken()
        {
            var user = UserAPI.login(Parameter_Special.ACCOUNT_DEFAULT, Parameter_Special.PASSWORD_DEFAULT);
            if (user.accessToken != null)
            {
                Parameter_Special.USER_PRESENT = user;
            }
        }

        private void getCompanies()
        {
            if (!string.IsNullOrEmpty(Parameter_Special.USER_PRESENT.accessToken))
            {

                var companies = Utils.getAPI(COLLECTIONS.companies, null);
                foreach (var company in companies)
                {
                    _companies.Add((companies)company);
                }
                cbCompanies.DataSource = _companies;
                cbCompanies.DisplayMember = "name";
                cbCompanies.SelectedIndex = 0;
                getDevices();
            }
        }

        private void getDevices()
        {
            int index = cbCompanies.SelectedIndex;
            if (index >= 0)
            {
                _devices = new List<devices>();
                var devices = Utils.getAPI(COLLECTIONS.devices, $"companyId={_companies[index]._id}");
                foreach (var device in devices)
                {
                    _devices.Add((devices)device);
                }
                if (_devices.Count > 0)
                {
                    cbDevices.DataSource = _devices;
                    cbDevices.DisplayMember = "name";
                    cbDevices.SelectedIndex = 0;
                    indexDevice = 0;
                }
            }
        }

        private void cbCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDevices();
        }

        private void SetupCameraCapture()
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame; ;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            picCameraDevice.Image = (Bitmap)eventArgs.Frame.Clone();
            string qr_code = Utils.ScanQRCodeByBitMap((Bitmap)eventArgs.Frame.Clone());
            if (qr_code != null && _devices.Count > 0)
            {
                try
                {
                    //int status = Utils.verfyQRCodeInLocal(qr_code, _devices[indexDevice]);
                    int status = Utils.verifyQRCode(qr_code, _devices[indexDevice]._id);
                    indexStatus = status;
                }
                catch
                {
                    indexStatus = 0;
                }
            }
        }

        private void SimulinkDevice_Load(object sender, EventArgs e)
        {

        }

        private void cbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexDevice = cbDevices.SelectedIndex;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            indexStatus = 0;
        }

        private void txtCardId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //int status = Utils.verifyCardInLocal(txtCardId.Text, _devices[indexDevice]);
                int status = Utils.verifyCard(txtCardId.Text, _devices[indexDevice]._id);
                indexStatus = status;
            }
            catch
            {
                indexStatus = 0;
            }
        }

        private void SimulinkDevice_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }
    }
}
