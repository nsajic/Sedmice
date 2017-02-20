using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Sedmice
{
    public partial class ServerSide : Form
    {
        TcpListener listener = null;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        byte[] data;
        //Socket client;
        TcpClient client;

        SedmiceRacunanje sedmiceRacunanje;
        List<Karta> talonKarte;
        List<Karta> rukaKarte;

        public ServerSide()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox1.Text = address.ToString();
                }
            }
            textBox2.Text = "8000";

            sedmiceRacunanje = new SedmiceRacunanje();
            talonKarte = new List<Karta>();
            rukaKarte = new List<Karta>();
            /*
            talonKarte.Add(new Karta("K", "8"));
            //talonKarte.Add(new Karta("P", "8"));
            rukaKarte.Add(new Karta("T", "J"));
            rukaKarte.Add(new Karta("T", "A"));
            rukaKarte.Add(new Karta("T", "7"));
            rukaKarte.Add(new Karta("T", "K"));

            ispisiTalon();
            Dictionary<Karta, float> procentiKarata = sedmiceRacunanje.procenaPoteza(talonKarte, rukaKarte);
            ispisProcenta(procentiKarata);
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*IPEndPoint ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ip);
            socket.Listen(10);
            Console.WriteLine("Waiting for a client..." + ip);
            client = socket.Accept();
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);
     
            data = new byte[200];
            client.Receive(data, data.Length, SocketFlags.None);
            //int receiveddata = client.Receive(data);

            //STR = new StreamReader(socket.Rec.Receive(data);
            //STW = new StreamWriter(client.GetStream());
            //STW.AutoFlush = true;
            //Console.WriteLine("Received data from CLIENT1: {0}", System.Text.ASCIIEncoding.ASCII.GetString(data).Trim());
            //textBox3.Text += Encoding.ASCII.GetString(data).Trim() + "\n";
            backgroundWorker1.RunWorkerAsync();*/

            //
            //Console.WriteLine("Received data from CLIENT1: {0}", System.Text.ASCIIEncoding.ASCII.GetString(data).Trim());



            listener = new TcpListener(IPAddress.Any, int.Parse(textBox2.Text));

            listener.Start();

            client = listener.AcceptTcpClient();

            //STR = new StreamReader(client.GetStream());

            //STW = new StreamWriter(client.GetStream());

            // STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();             // Startovano citanje podataka

            //ThreadHelperClass.SetText(this, textBox16, "");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)     // Citanje podataka
        {
            //Console.WriteLine("Received data from CLIENT1: {0}", System.Text.ASCIIEncoding.ASCII.GetString(data).Trim());
            Console.WriteLine("P -1");
            while (client.Connected)
            {

                try
                {

                    using (NetworkStream stream = client.GetStream())
                    {

                        /* using (var ms = new MemoryStream())
                         {
                             stream.CopyTo(ms);

                             System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                             Image img = (Image)converter.ConvertFrom(ms.ToArray());

                             pictureBox1.Image = img;*/
                        /* Bitmap newBitmap;
                         using (MemoryStream memoryStream = new MemoryStream(ms.ToArray()))
                         using (Image newImage = Image.FromStream(memoryStream))
                         {
                             newBitmap = new Bitmap(newImage);
                         }*/
                        // }

                        /* byte[] ignore = new byte[4];
                         int bytesRead = 0;
                         do
                         {
                             // This should complete in a single call, but the API requires you
                             // to do it in a loop.
                             bytesRead += stream.Read(ignore, bytesRead, 4 - bytesRead);
                         } while (bytesRead != 4);
                         // Copy the rest of the stream to a file
                         using (var fs = new FileStream("testCard.jpg", FileMode.Create))
                         {
                             stream.CopyTo(fs);
                         }
                         stream.Close();*/





                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            using (BinaryWriter writer = new BinaryWriter(File.Open("../../../python/testCard.jpg", FileMode.Create)))
                            {
                                int byteRead = 0;
                                //Image img;
                                do
                                {
                                    byte[] buffer = reader.ReadBytes(1024);
                                    byteRead = buffer.Length;
                                    writer.Write(buffer);

                                    //byteTransfered += byteRead;
                                } while (byteRead == 1024);

                            }
                        }

                        // Podesiti da prikaze celu sliku (trenutno prikazuje samo delic slike)
                        try
                        {
                            Image img = Image.FromFile("testCard.jpg");
                            //TODO: Vratiti ako lepo prikazuje
                            //pictureBox1.Image = img;
                        }
                        catch (OutOfMemoryException oome)
                        {
                            MessageBox.Show(oome.Message.ToString());
                        }
                        catch (FileNotFoundException fnfe)
                        {
                            MessageBox.Show(fnfe.Message.ToString());
                        }
                        catch (ArgumentException ae)
                        {
                            MessageBox.Show(ae.Message.ToString());
                        }


                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8081/lang");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            // streamWriter.Write();
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        var streamReader = new StreamReader(httpResponse.GetResponseStream());

                        var result = streamReader.ReadToEnd();
                        Console.WriteLine(result);
                        var str = result.Split('(');
                        
                        List<Karta> parsiraneKarte = new List<Karta>();

                        for(int i = 1; i < str.Length; i++)
                        {
                            var str2 = str[i].Split('\'');
                            Karta karta = new Karta(str2[3], str2[1]);
                            parsiraneKarte.Add(karta);
                        }
                        // Isparsirati result

                        if (parsiraneKarte.Count == 1)
                        {
                            talonKarte.Add(parsiraneKarte[0]);
                            ispisiTalon();
                            //karteNaTalonu.Items.Add(parsiraneKarte[0]);
                        } else if (parsiraneKarte.Count > 1)
                        {
                            rukaKarte.AddRange(parsiraneKarte);
                            Dictionary<Karta, float> procentiKarata = sedmiceRacunanje.procenaPoteza(talonKarte, rukaKarte);
                            ispisProcenta(procentiKarata);

                            try
                            {
                                foreach(Karta k in rukaKarte)
                                    ThreadHelperClass.SetText(this, textBox16, textBox16.Text + Environment.NewLine + k.broj + "  -  " + k.znak);
                                foreach (Karta k in talonKarte)
                                    ThreadHelperClass.SetText(this, textBox16, textBox16.Text + Environment.NewLine + k.broj + "  -  " + k.znak);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Ne postoji");
                            }

                            rukaKarte.Clear();
                            talonKarte.Clear();
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                client = listener.AcceptTcpClient();
            }

        }


        public void ispisProcenta(Dictionary<Karta, float> procentiKarata)
        {
            try
            {
                ThreadHelperClass.SetText(this, textBox5, procentiKarata.ElementAt(0).Key.broj);
                //textBox5.Text = procentiKarata.ElementAt(0).Key.broj;
                ThreadHelperClass.SetText(this, textBox7, procentiKarata.ElementAt(0).Key.znak);
                //textBox7.Text = procentiKarata.ElementAt(0).Key.znak;
                ThreadHelperClass.SetText(this, textBox11, procentiKarata.ElementAt(0).Value.ToString() + "%");
                //textBox11.Text = procentiKarata.ElementAt(0).Value.ToString() + "%";
            }
            catch (Exception)
            {
                Console.WriteLine("Ne postoji 0. element");
            }
            try
            {
                //textBox6.Text = procentiKarata.ElementAt(1).Key.broj;
                ThreadHelperClass.SetText(this, textBox6, procentiKarata.ElementAt(1).Key.broj);
                //textBox10.Text = procentiKarata.ElementAt(1).Key.znak;
                ThreadHelperClass.SetText(this, textBox10, procentiKarata.ElementAt(1).Key.znak);
                //textBox12.Text = procentiKarata.ElementAt(1).Value.ToString() + "%";
                ThreadHelperClass.SetText(this, textBox12, procentiKarata.ElementAt(1).Value.ToString() + "%");
            }
            catch (Exception)
            {
                Console.WriteLine("Ne postoji 1. element");
            }
            try
            {
                //textBox4.Text = procentiKarata.ElementAt(2).Key.broj;
                ThreadHelperClass.SetText(this, textBox4, procentiKarata.ElementAt(2).Key.broj);
                //textBox9.Text = procentiKarata.ElementAt(2).Key.znak;
                ThreadHelperClass.SetText(this, textBox9, procentiKarata.ElementAt(2).Key.znak);
                //textBox13.Text = procentiKarata.ElementAt(2).Value.ToString() + "%";
                ThreadHelperClass.SetText(this, textBox13, procentiKarata.ElementAt(2).Value.ToString() + "%");
            }
            catch (Exception)
            {
                Console.WriteLine("Ne postoji 2. element");
            }
            try
            {
                //textBox3.Text = procentiKarata.ElementAt(3).Key.broj;
                ThreadHelperClass.SetText(this, textBox3, procentiKarata.ElementAt(3).Key.broj);
                //textBox8.Text = procentiKarata.ElementAt(3).Key.znak;
                ThreadHelperClass.SetText(this, textBox8, procentiKarata.ElementAt(3).Key.znak);
                //textBox14.Text = procentiKarata.ElementAt(3).Value.ToString() + "%";
                ThreadHelperClass.SetText(this, textBox14, procentiKarata.ElementAt(3).Value.ToString() + "%");
            }
            catch (Exception)
            {
                Console.WriteLine("Ne postoji 3. element");
            }
        }

        public void ispisiTalon()
        {
            try
            {
                //ThreadHelperClass.SetText(this, textBox5, procentiKarata.ElementAt(0).Key.broj);
                //karteNaTalonu.Items.Clear();
                ThreadHelperClass.SetText(this, textBox15, "");
                //ThreadHelperClass.SetText(this, textBox15, "Karte na talonu");
                //karteNaTalonu.Items.Add("Karte na talonu:");
                //string talonKarteIspis = "";
                foreach (Karta k in talonKarte)
                {
                    //ThreadHelperClass.SetText(this, textBox15, k.broj);
                    //talonKarteIspis += k.broj + " ~ " + k.znak;
                    //karteNaTalonu.Items.Add(k.broj + "  -  " + k.znak);
                    ThreadHelperClass.SetText(this, textBox15, textBox15.Text + Environment.NewLine + k.broj + "  -  " + k.znak);

                }
                //ThreadHelperClass.SetText(this, textBox15, talonKarteIspis);
            }
            catch (Exception ex)
            {
                Console.WriteLine("pukla greska..");
            }
        }
    }

    public static class ThreadHelperClass
    {
        delegate void SetTextCallback(Form f, Control ctrl, string text);
        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void SetText(Form form, Control ctrl, string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (ctrl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }
    }
}
