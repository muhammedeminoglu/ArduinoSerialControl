using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace OgrenciKartiOkumaYazma
{
    public partial class Form1 : Form
    {
        //İletişim anında textbox'ları set etmek için delegate değişkenleri
        delegate void SetUid(string text);
        delegate void SetTcKimlik(string text);
        delegate void SetAd(string text);
        delegate void SetSoyad(string text);
        delegate void SetOgrenciNo(string text);
        delegate void SetGelen(string text);
        delegate void SetBirim(string text);
        //*****************************************************************

        int ilkMi = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void SetGelenMesaj(string text)
        {
            if (txtMesaj.InvokeRequired)
            {
                SetGelen d = new SetGelen(SetGelenMesaj);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtMesaj.Text = text;
            }
        }
        private void SetUidText(string text)
        {
            if (txtUid.InvokeRequired)
            {
                SetUid d = new SetUid(SetUidText);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtUid.Text = text;
            }
        }
        private void SetTc(string text)
        {
            if (txtUid.InvokeRequired)
            {
                SetTcKimlik d = new SetTcKimlik(SetTc);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtTc.Text = text;
            }
        }
        private void SetIsim(string text)
        {
            if (txtIsim.InvokeRequired)
            {
                SetAd d = new SetAd(SetIsim);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtIsim.Text = text;
            }
        }
        private void SetSoyisim(string text)
        {
            if (txtSoyisim.InvokeRequired)
            {
                SetSoyad d = new SetSoyad(SetSoyisim);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtSoyisim.Text = text;
            }
        }
        private void SetOgrNo(string text)
        {
            if (txtOgrenciNo.InvokeRequired)
            {
                SetOgrenciNo d = new SetOgrenciNo(SetOgrNo);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtOgrenciNo.Text = text;
            }
        }
        private void SetBirimler(string text)
        {
            if (txtBirimler.InvokeRequired)
            {
                SetBirim d = new SetBirim(SetBirimler);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                txtBirimler.Text = text;
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }

            serialPort1.PortName = cmbPort.Text;
            serialPort1.BaudRate = Convert.ToInt32(cmbBaud.Text);

            try
            {
                serialPort1.Open();
                imgDurum.BackgroundImage = System.Drawing.Image.FromFile(Directory.GetCurrentDirectory() + "\\Resources\\olumlu.PNG");
            }
            catch
            {
                imgDurum.BackgroundImage = System.Drawing.Image.FromFile(Directory.GetCurrentDirectory() + "\\Resources\\olumsuz.PNG"); 
                MessageBox.Show("Seri Port Bağlantısı gerçekleşmedi");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                String[] portlar = SerialPort.GetPortNames();
                cmbPort.Items.AddRange(portlar);
                cmbPort.SelectedIndex = 0;
                cmbBaud.SelectedIndex = 0;

            }
            catch
            {
                imgDurum.BackgroundImage = System.Drawing.Image.FromFile("C:\\Users\\Muhammed\\source\\repos\\MFRC522-Controller\\MFRC522-Controller\\Resimler\\olumsuz.png");
            }

        }

        //Okuma fonksiyonu
        private void button2_Click(object sender, EventArgs e)
        {
            temizle();
            if (ilkMi == 0)
            {
                if (serialPort1.IsOpen)
                    serialPort1.Write("0                                                                                    #");
                else
                {
                    MessageBox.Show("Port Bağlı Değil");
                }
            }
            if (serialPort1.IsOpen)
                serialPort1.Write("0                                                                                    #");
            else
            {
                MessageBox.Show("Port Bağlı Değil");
            }
            ilkMi = 1;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        void temizle()
        {
            rdrHepsi.Checked = true;
            txtIsim.Text = "";
            txtSoyisim.Text = "";
            txtTc.Text = "";
            txtUid.Text = "";
            txtOgrenciNo.Text = "";
            txtBirimler.Text = "";
            txtMesaj.Text = "";
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //String gelen = serialPort1.ReadLine();
            byte[] GelenByte = new byte[86];
            int alim = 0;
            try
            {
                serialPort1.Read(GelenByte, 0, 86);
                alim = 1;
            }
            catch
            {

            }
            


            String Uid = "";
            String Ad = "";
            String Soyad = "";
            String OgrenciNo = "";
            String TcKimlik = "";
            String Birimler = "";

            if (Convert.ToChar(Convert.ToInt32(GelenByte[0])) == '0' && Convert.ToChar(Convert.ToInt32(GelenByte[85])) == '#' && alim == 1)
            {
                
                for (int i = 1; i < 5; i++)
                {
                    if (i == 1)
                        Uid = Uid + "" + Convert.ToString(Convert.ToInt32(GelenByte[i]));
                    else
                        Uid = Uid + " " + Convert.ToString(Convert.ToInt32(GelenByte[i]));
                }
                
                for (int i = 5; i < 21; i++)
                {
                    if (GelenByte[i] != 32)
                        TcKimlik = TcKimlik + Convert.ToChar(Convert.ToInt32(GelenByte[i]));
                }
                

                for (int i = 21; i < 37; i++)
                {
                    if (GelenByte[i] != ' ')
                        Ad = Ad + "" + Convert.ToChar(Convert.ToInt32(GelenByte[i]));
                }
                

                for (int i = 37; i < 53; i++)
                {
                    if (GelenByte[i] != ' ')
                        Soyad = Soyad + "" + Convert.ToChar(Convert.ToInt32(GelenByte[i])); ;
                }
                

                for (int i = 53; i < 69; i++)
                {
                    if (GelenByte[i] != ' ')
                        OgrenciNo = OgrenciNo + "" + Convert.ToChar(Convert.ToInt32(GelenByte[i])); ;
                }
               

                for (int i = 69; i < 85; i++)
                {
                    if (GelenByte[i] != 32)
                        if (i == 69)
                            Birimler = Birimler + "" + Convert.ToString(Convert.ToInt32(GelenByte[i]));
                        else
                            Birimler = Birimler + "," + Convert.ToString(Convert.ToInt32(GelenByte[i]));
                }

                SetUidText(Uid);
                SetTc(TcKimlik);
                SetIsim(Ad);
                SetSoyisim(Soyad);
                SetOgrNo(OgrenciNo);
                SetBirimler(Birimler);  
            }

        }

        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Hata oluşması engellendi");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] Yazilacak = new byte[86];

            
            Yazilacak[85] = 35;
            if (rdrHepsi.Checked)
            {
                String[] birimler = new String[16];
                Yazilacak[0] = 49;
                Yazilacak[1] = 32;
                Yazilacak[2] = 32;
                Yazilacak[3] = 32;
                Yazilacak[4] = 32;

                //*********TC KİMLİK OLUŞTUR****************
                String YeniTC = txtTc.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtTc.Text.Length)
                        YeniTC = YeniTC + " "; ;
                }

                byte[] Tc = Encoding.ASCII.GetBytes(YeniTC);
                for (int i = 5; i < 21; i++)
                {
                    Yazilacak[i] = Tc[i - 5];
                }
                //*******************************************

                //************Ad OLUŞTUR*********************
                String YeniAd = txtIsim.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtIsim.Text.Length)
                        YeniAd = YeniAd + " "; ;
                }

                byte[] Ad = Encoding.ASCII.GetBytes(YeniAd);
                for (int i = 21; i < 37; i++)
                {
                    Yazilacak[i] = Ad[i - 21];
                }
                //**********************************************

                //************Soyad OLUŞTUR*********************
                String YeniSoyad = txtSoyisim.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtSoyisim.Text.Length)
                        YeniSoyad = YeniSoyad + " ";
                }

                byte[] Soyad = Encoding.ASCII.GetBytes(YeniSoyad);
                for (int i = 37; i < 53; i++)
                {
                    Yazilacak[i] = Soyad[i - 37];
                }
                //**********************************************

                //************Öğrenci No OLUŞTUR*********************
                String YeniNo = txtOgrenciNo.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtOgrenciNo.Text.Length)
                        YeniNo = YeniNo + " ";
                }

                byte[] Numara = Encoding.ASCII.GetBytes(YeniNo);
                for (int i = 53; i < 69; i++)
                {
                    Yazilacak[i] = Numara[i - 53];
                }


                List<string> BirimlerString = txtBirimler.Text.Split(',').ToList<string>();
                for (int i = 69; i < 85; i++)
                {
                    if (i - 69 < BirimlerString.Count)
                        Yazilacak[i] = Convert.ToByte(Convert.ToInt32(BirimlerString[i - 69]));
                    else
                        Yazilacak[i] = 32;
                }

                serialPort1.Write(Yazilacak, 0, 86);
            }
            else if (rdrTcKimlik.Checked)
            {
                Yazilacak[0] = 50;
                Yazilacak[1] = 48;
                String YeniTc = txtTc.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtTc.Text.Length)
                        YeniTc = YeniTc + " ";
                }

                byte[] Numara = Encoding.ASCII.GetBytes(YeniTc);
                for (int i = 2; i < 86; i++)
                {
                    if (i < 18)
                        Yazilacak[i] = Numara[i - 2];
                    else
                        Yazilacak[i] = 35;
                }
                serialPort1.Write(Yazilacak, 0, 86);
            }

            else if (rdrAd.Checked)
            {
                Yazilacak[0] = 50;
                Yazilacak[1] = 49;
                String YeniIsim = txtIsim.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtIsim.Text.Length)
                        YeniIsim = YeniIsim + " ";
                }

                byte[] Numara = Encoding.ASCII.GetBytes(YeniIsim);
                for (int i = 2; i < 86; i++)
                {
                    if (i < 18)
                        Yazilacak[i] = Numara[i - 2];
                    else
                        Yazilacak[i] = 35;
                }
                serialPort1.Write(Yazilacak, 0, 86);
            }

            else if (rdrSoyad.Checked)
            {
                Yazilacak[0] = 50;
                Yazilacak[1] = 50;
                String YeniIsim = txtSoyisim.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtSoyisim.Text.Length)
                        YeniIsim = YeniIsim + " ";
                }

                byte[] Numara = Encoding.ASCII.GetBytes(YeniIsim);
                for (int i = 2; i < 86; i++)
                {
                    if (i < 18)
                        Yazilacak[i] = Numara[i - 2];
                    else
                        Yazilacak[i] = 35;
                }
                serialPort1.Write(Yazilacak, 0, 86);
            }

            else if (rdrNo.Checked)
            {
                Yazilacak[0] = 50;
                Yazilacak[1] = 51;
                String YeniIsim = txtOgrenciNo.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtOgrenciNo.Text.Length)
                        YeniIsim = YeniIsim + " ";
                }

                byte[] Numara = Encoding.ASCII.GetBytes(YeniIsim);
                for (int i = 2; i < 86; i++)
                {
                    if (i < 18)
                        Yazilacak[i] = Numara[i - 2];
                    else
                        Yazilacak[i] = 35;
                }
                serialPort1.Write(Yazilacak, 0, 86);
            }

            else if (rdrBirim.Checked)
            {
                Yazilacak[0] = 50;
                Yazilacak[1] = 52;
                List<string> BirimlerString = txtBirimler.Text.Split(',').ToList<string>();
                for (int i = 2; i < 86; i++)
                {
                    if (i < BirimlerString.Count + 2)
                        Yazilacak[i] = Convert.ToByte(Convert.ToInt32(BirimlerString[i - 2]));
                    else
                        if (i < 18)
                            Yazilacak[i] = 0;
                }

                serialPort1.Write(Yazilacak, 0, 86);
            }

        }
    }
}
