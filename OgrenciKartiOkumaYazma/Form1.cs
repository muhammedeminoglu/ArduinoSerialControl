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
                txtMesaj.AppendText(text);
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
                txtUid.AppendText(text);
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
                txtTc.AppendText(text);
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
                txtIsim.AppendText(text);
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
                txtSoyisim.AppendText(text);
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
                txtOgrenciNo.AppendText(text);
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
                txtBirimler.AppendText(text);
            }
        }

        int ilkMi = 0;

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
                imgDurum.BackgroundImage = System.Drawing.Image.FromFile("C:\\Users\\Muhammed\\source\\repos\\MFRC522-Controller\\MFRC522-Controller\\Resimler\\olumlu.png");
            }
            catch
            {
                imgDurum.BackgroundImage = System.Drawing.Image.FromFile("C:\\Users\\Muhammed\\source\\repos\\MFRC522-Controller\\MFRC522-Controller\\Resimler\\olumsuz.png");
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

            SetUidText("");
            SetTc("");
            SetIsim("");
            SetSoyisim("");
            SetOgrNo("");
            SetBirimler("");

            byte[] GelenByte = new byte[86];
            serialPort1.Read(GelenByte, 0, 86);
            

            String Uid = "";
            String Ad = "";
            String Soyad = "";
            String OgrenciNo = "";
            String TcKimlik = "";
            String Birimler = "";

            /*try
            {
                if (gelen[0] == '1' && gelen.Length > 80)
                {
                    for (int i = 5; i < 21; i++)
                    {
                        if (gelen[i] != 32)
                            TcKimlik = TcKimlik + "" + gelen[i];
                    }
                    SetTc(TcKimlik);

                    for (int i = 21; i < 37; i++)
                    {
                        if (gelen[i] != ' ')
                            Ad = Ad + "" + gelen[i];
                    }
                    SetIsim(Ad);

                    for (int i = 37; i < 53; i++)
                    {
                        if (gelen[i] != ' ')
                            Soyad = Soyad + "" + gelen[i];
                    }
                    SetSoyisim(Soyad);

                    for (int i = 53; i < 69; i++)
                    {
                        if (gelen[i] != ' ')
                            OgrenciNo = OgrenciNo + "" + gelen[i];
                    }
                    SetOgrNo(OgrenciNo);

                    for (int i = 69; i < 85; i++)
                    {
                        if (gelen[i] != ' ')
                            Birimler = Birimler + "" + gelen[i];
                    }

                    byte[] Birimimm = Encoding.ASCII.GetBytes(Birimler);
                    String tBirimler = "";
                    for (int i = 0; i < Birimler.Length; i++)
                    {
                        if (i == 0)
                            tBirimler = tBirimler + Convert.ToString(Birimimm[i]);
                        else
                            tBirimler = tBirimler + "," + Convert.ToString(Birimimm[i]);
                    }
                    SetBirimler(tBirimler);


                }
            }
            catch
            {

            }*/

            if (Convert.ToChar(Convert.ToInt32(GelenByte[85])) == '#' && Convert.ToChar(Convert.ToInt32(GelenByte[0])) == '0')
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
            if (rdrHepsi.Checked)
            {
                String[] birimler = new String[16];
                byte[] Yazilacak = new byte[86];

                Yazilacak[0] = 49;
                Yazilacak[1] = 32;
                Yazilacak[2] = 32;
                Yazilacak[3] = 32;
                Yazilacak[4] = 32;
                Yazilacak[85] = 35;

                //*********TC KİMLİK OLUŞTUR****************
                String YeniTC = txtTc.Text;
                for (int i = 0; i < 16; i++)
                {
                    if (i >= txtTc.Text.Length)
                        YeniTC = YeniTC + " ";;
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

                /*
                List<string> BirimlerString = txtBirimler.Text.Split(',').ToList<string>();
                int[] BirimlerSayi = new int[BirimlerString.Count];
                byte[] BirimBuffer = new byte[16];
                String GonderilecekVeri = "1";

                for (int i = 1; i < 17; i++)
                {
                    if (i < txtTc.Text.Length + 1)
                        GonderilecekVeri = GonderilecekVeri + "" + txtTc.Text[i - 1];
                    else
                        GonderilecekVeri = GonderilecekVeri + " ";
                }

                for (int i = 17; i < 33; i++)
                {
                    if (i < txtIsim.Text.Length + 17)
                        GonderilecekVeri = GonderilecekVeri + "" + txtIsim.Text[i - 17];
                    else
                        GonderilecekVeri = GonderilecekVeri + " ";
                }

                for (int i = 33; i < 49; i++)
                {
                    if (i < txtSoyisim.Text.Length + 33)
                        GonderilecekVeri = GonderilecekVeri + "" + txtSoyisim.Text[i - 33];
                    else
                        GonderilecekVeri = GonderilecekVeri + " ";
                }

                for (int i = 49; i < 65; i++)
                {
                    if (i < txtOgrenciNo.Text.Length + 49)
                        GonderilecekVeri = GonderilecekVeri + "" + txtOgrenciNo.Text[i - 49];
                    else
                        GonderilecekVeri = GonderilecekVeri + " ";
                }

                for (int i = 0; i < BirimlerString.Count; i++)
                {
                    BirimlerSayi[i] = Convert.ToInt32(BirimlerString[i]);
                    BirimBuffer[i] = Convert.ToByte(BirimlerSayi[i]);
                }

                for (int i = BirimlerString.Count; i < 16; i++)
                {
                    BirimBuffer[i] = 32;
                }
                GonderilecekVeri = GonderilecekVeri + Encoding.ASCII.GetString(BirimBuffer);
                txtMesaj.AppendText(GonderilecekVeri);
                
                GonderilecekVeri = GonderilecekVeri + '#';
                serialPort1.Write(GonderilecekVeri);*/
            }
            else if(rdrTcKimlik.Checked)
            {
                
                String GonderilecekVeri = "20";

                for (int i = 0; i < 16; i++)
                {
                    if (i < txtTc.Text.Length)
                        GonderilecekVeri = GonderilecekVeri + "" + txtTc.Text[i];
                    else
                        GonderilecekVeri = GonderilecekVeri + ' ';
                }
                GonderilecekVeri = GonderilecekVeri + '#';
                serialPort1.Write(GonderilecekVeri);
                
            }

            else if (rdrAd.Checked)
            {
                String GonderilecekVeri = "21";

                for (int i = 0; i < 16; i++)
                {
                    if (i < txtIsim.Text.Length)
                        GonderilecekVeri = GonderilecekVeri + "" + txtIsim.Text[i];
                    else
                        GonderilecekVeri = GonderilecekVeri + ' ';
                }
                GonderilecekVeri = GonderilecekVeri + '#';
                serialPort1.Write(GonderilecekVeri);
            }

            else if (rdrSoyad.Checked)
            {
                String GonderilecekVeri = "22";

                for (int i = 0; i < 16; i++)
                {
                    if (i < txtSoyisim.Text.Length)
                        GonderilecekVeri = GonderilecekVeri + "" + txtSoyisim.Text[i];
                    else
                        GonderilecekVeri = GonderilecekVeri + " ";
                }
                GonderilecekVeri = GonderilecekVeri + '#';
                serialPort1.Write(GonderilecekVeri);
            }

            else if (rdrNo.Checked)
            {
                String GonderilecekVeri = "23";

                for (int i = 0; i < 16; i++)
                {
                    if (i < txtOgrenciNo.Text.Length)
                        GonderilecekVeri = GonderilecekVeri + "" + txtOgrenciNo.Text[i];
                    else
                        GonderilecekVeri = GonderilecekVeri + ' ';
                }
                GonderilecekVeri = GonderilecekVeri + '#';
                txtMesaj.AppendText(GonderilecekVeri);
                serialPort1.Write(GonderilecekVeri);
            }

            else if(rdrBirim.Checked)
            {
                String GonderilecekVeri = "24";
                String[] birimler = new String[16];
                List<string> BirimlerString = txtBirimler.Text.Split(',').ToList<string>();
                int[] BirimlerSayi = new int[BirimlerString.Count];
                byte[] BirimBuffer = new byte[16];

                for (int i = 0; i < BirimlerString.Count; i++)
                {
                    BirimlerSayi[i] = Convert.ToInt32(BirimlerString[i]);
                    BirimBuffer[i] = Convert.ToByte(BirimlerSayi[i]);
                }

                for (int i = BirimlerString.Count; i < 16; i++)
                {
                    BirimBuffer[i] = 32;
                }
                GonderilecekVeri = GonderilecekVeri + Encoding.ASCII.GetString(BirimBuffer);
                GonderilecekVeri = GonderilecekVeri + '#';
                serialPort1.WriteLine(GonderilecekVeri);
                //serialPort1.Write("#");
            }

        }
    }
}
