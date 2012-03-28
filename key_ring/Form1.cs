using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace key_ring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            groupBoxSignIn.Show();
            groupBoxMain.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxSignUp.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Hide();
            groupBoxChangePassword.Hide();
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void buttonSignIn_Click_1(object sender, EventArgs e)
        {
            string line;
            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            if (userName.Text.ToString() == "admin" && password.Text.ToString() == "1Cafe2Face")
            {
                //now some steps is going to be taken to generate the ComboBox with new data
                //...........................................................................
                if (File.Exists("c:\\key_ring\\encrypted.key"))
                {
                    // open the file containing the key and IV

                    FileStream fsKeyInCombo = File.OpenRead(@"c:\\key_ring\\encrypted.key");

                    // use a BinaryReader to read formatted data from the file
                    BinaryReader brCombo = new BinaryReader(fsKeyInCombo);

                    // read data from the file and close it
                    tdes.Key = brCombo.ReadBytes(24);
                    tdes.IV = brCombo.ReadBytes(8);

                    // Open the encrypted file
                    FileStream fsInCombo = File.OpenRead(@"c:\\key_ring\\user_info.txt");

                    // Create a cryptostream to decrypt from the filestream
                    CryptoStream csread = new CryptoStream(fsInCombo, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                    // Create a StreamReader to format the input
                    StreamReader srread = new StreamReader(csread);

                    // And decrypt the data
                    comboBoxUserName.Items.Clear();
                    
                    while ((line = srread.ReadLine()) != null && line.Length > 0)
                    {
                        int firstComma = line.IndexOf(";");
                        //string name = line.Substring(0, firstComma);
                        string necessaryInfo = line.Substring(firstComma + 1, line.Length - firstComma - 1);
                        string Uname = necessaryInfo.Substring(0, necessaryInfo.IndexOf(";"));
                        comboBoxUserName.Items.Add(Uname);
                        
                    }

                    srread.Close();
                    brCombo.Close();
                    fsKeyInCombo.Close();

                    groupBoxAdminPanel.Show();
                    groupBoxMain.Hide();
                    groupBoxAddInfo.Hide();
                    groupBoxSignIn.Hide();
                    groupBoxSignUp.Hide();
                    groupBoxHelp.Hide();
                    groupBoxChangePassword.Hide();
                }
                else
                {
                    MessageBox.Show("There are no existing user!");
                }
                button3.Enabled = false;
                button4.Enabled = false;
                
                
            }
            else
            {
                if (codeWord.Text.ToString() == textBoxRandomValue.Text.ToString())
                {
                    uName = userName.Text.ToString();
                    string passwd = password.Text.ToString();
                    //string line;
                    int userNameIndex;
                    int passWordIndex;
                    int success = 0;

                    //create key_ring directory in C drive if doesnot exists...
                    if (!Directory.Exists("c:\\key_ring"))
                    {
                        Directory.CreateDirectory("c:\\key_ring");
                    }
                    //create the user_info file if it doesnt exits............
                    if (!File.Exists("c:\\key_ring\\user_info.txt"))
                    {
                        File.Create("c:\\key_ring\\user_info.txt").Close();
                    }


                    // open the file containing the key and IV
                    FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\encrypted.key");

                    // use a BinaryReader to read formatted data from the file
                    BinaryReader br = new BinaryReader(fsKeyIn);

                    // read data from the file and close it
                    tdes.Key = br.ReadBytes(24);
                    tdes.IV = br.ReadBytes(8);

                    // Open the encrypted file
                    FileStream fsIn = File.OpenRead(@"c:\\key_ring\\user_info.txt");

                    // Create a cryptostream to decrypt from the filestream
                    CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                    // Create a StreamReader to format the input
                    StreamReader sr = new StreamReader(cs);

                    // And decrypt the data

                    while ((line = sr.ReadLine()) != null)
                    {
                        userNameIndex = line.IndexOf(uName);
                        passWordIndex = line.IndexOf(passwd);

                        if (userNameIndex != -1 && passWordIndex != -1)
                        {
                            success = 1;
                        }
                    }

                    sr.Close();


                    if (success == 1)
                    {


                        //now some steps is going to be taken to generate the ComboBox with new data
                        //...........................................................................
                        if (File.Exists("c:\\key_ring\\Account_info\\" + uName + ".key"))
                        {
                            // open the file containing the key and IV

                            FileStream fsKeyInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

                            // use a BinaryReader to read formatted data from the file
                            BinaryReader brCombo = new BinaryReader(fsKeyInCombo);

                            // read data from the file and close it
                            tdes.Key = brCombo.ReadBytes(24);
                            tdes.IV = brCombo.ReadBytes(8);

                            // Open the encrypted file
                            FileStream fsInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

                            // Create a cryptostream to decrypt from the filestream
                            CryptoStream csread = new CryptoStream(fsInCombo, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                            // Create a StreamReader to format the input
                            StreamReader srread = new StreamReader(csread);

                            // And decrypt the data
                            comboBoxAccount.Items.Clear();
                            //int i = 0;
                            while ((line = srread.ReadLine()) != null && line.Length > 0)
                            {
                                int i = line.IndexOf(";");
                                string account = line.Substring(0, i);
                                comboBoxAccount.Items.Add(account);
                                //comboBoxAccount.Items.Add(line);
                            }
                            br.Close();
                            srread.Close();
                            brCombo.Close();
                            fsKeyInCombo.Close();
                        }

                        button3.Enabled = false;
                        button4.Enabled = false;

                        groupBoxAdminPanel.Hide();
                        groupBoxMain.Show();
                        groupBoxAddInfo.Hide();
                        groupBoxSignIn.Hide();
                        groupBoxSignUp.Hide();
                        groupBoxHelp.Hide();
                        groupBoxChangePassword.Hide();

                    }
                    else
                    {
                        MessageBox.Show("User name or Password Invalid!");
                    }


                }
                else
                {
                    MessageBox.Show("Write the code correctly!");

                }
            }
            


        }

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBoxSignIn_Enter(object sender, EventArgs e)
        {
            randomValue = RandomNumber(21335, 83216).ToString();
            textBoxRandomValue.Text = randomValue.ToString();

        }

        private void linkLabelSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxName.Text = "";
            textBoxSignUpEmail.Text = "";
            textBoxSignUpUserName.Text = "";
            textBoxSignUpPassword.Text = "";
            textBoxSignUpRetypePassword.Text = "";

            groupBoxMain.Hide();
            groupBoxSignIn.Hide();
            groupBoxSignUp.Show();
            groupBoxHelp.Hide();
            groupBoxChangePassword.Hide();
            groupBoxAdminPanel.Hide();
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.ToString();
            string uName = textBoxSignUpUserName.Text.ToString();
            string passwd = textBoxSignUpPassword.Text.ToString();
            string retypePasswd = textBoxSignUpRetypePassword.Text.ToString();
            string eMail = textBoxSignUpEmail.Text.ToString();
            string line;

            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();



            if (passwd == retypePasswd)
            {
                if (name != uName)
                {
                    //creating necessary files for this user......
                    if (!Directory.Exists("c:\\key_ring\\Accout_info"))
                    {
                        Directory.CreateDirectory("c:\\key_ring\\Account_info");
                    }

                    if (!File.Exists("c:\\key_ring\\Account_info\\" + textBoxSignUpUserName.Text.ToString() + ".txt"))
                    {
                        File.Create("c:\\key_ring\\Account_info\\" + textBoxSignUpUserName.Text.ToString() + ".txt").Close();
                        string userInfo = name + ";" + uName + ";" + passwd + ";" + eMail + ";";

                        if (File.Exists("c:\\key_ring\\encrypted.key"))
                        {
                            // open the file containing the key and IV
                            FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\encrypted.key");

                            // use a BinaryReader to read formatted data from the file
                            BinaryReader br = new BinaryReader(fsKeyIn);

                            // read data from the file and close it
                            tdes.Key = br.ReadBytes(24);
                            tdes.IV = br.ReadBytes(8);

                            // Open the encrypted file
                            FileStream fsIn = File.OpenRead(@"c:\\key_ring\\user_info.txt");

                            // Create a cryptostream to decrypt from the filestream
                            CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                            // Create a StreamReader to format the input
                            StreamReader sr = new StreamReader(cs);


                            // create a writer and open a temporary file
                            FileStream fs = new FileStream("c:\\key_ring\\user_info_tmp.txt", FileMode.Append, FileAccess.Write);

                            // Create a cryptostream to encrypt to the filestream
                            CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

                            //StreamWriter tw = new StreamWriter(fs);
                            StreamWriter tw = new StreamWriter(csencr);

                            // And decrypt the data and write in a temporary file

                            while ((line = sr.ReadLine()) != null && line.Length > 0)
                            {
                                tw.WriteLine(line);
                            }
                            // write the new user info in the temp file.......
                            tw.WriteLine(";;;;this is new user");
                            tw.WriteLine(userInfo);
                            // close the stream
                            tw.Flush();
                            tw.Close();
                            sr.Close();
                            File.Delete("c:\\key_ring\\user_info.txt");
                            File.Move(@"c:\\key_ring\\user_info_tmp.txt", @"c:\\key_ring\\user_info.txt");
                            br.Close();
                        }
                        else
                        {
                            // create a writer to write the new users info
                            FileStream fs = new FileStream("c:\\key_ring\\user_info.txt", FileMode.Append, FileAccess.Write);

                            // Create a cryptostream to encrypt to the filestream
                            CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

                            //StreamWriter tw = new StreamWriter(fs);
                            StreamWriter tw = new StreamWriter(csencr);

                            // write the new user info in the temp file.......
                            tw.WriteLine(";;;;this is new user");
                            tw.WriteLine(userInfo);
                            // close the stream
                            tw.Flush();
                            tw.Close();
                        }


                        // save the key and IV for future use

                        FileStream fsKeyOut = File.Create(@"c:\\key_ring\\encrypted.key");

                        // use a BinaryWriter to write formatted data to the file
                        BinaryWriter bw = new BinaryWriter(fsKeyOut);

                        // write data to the file
                        bw.Write(tdes.Key);
                        bw.Write(tdes.IV);

                        // flush and close
                        bw.Flush();
                        bw.Close();


                        MessageBox.Show("Account has been created.");

                        userName.Text = "";
                        password.Text = "";
                        codeWord.Text = "";

                        groupBoxSignIn.Show();
                        groupBoxSignUp.Hide();
                        groupBoxMain.Hide();
                        groupBoxAddInfo.Hide();
                        groupBoxHelp.Hide();
                        groupBoxAdminPanel.Hide();
                        groupBoxChangePassword.Hide();

                    }
                    else
                    {
                        MessageBox.Show("User " + textBoxSignUpUserName.Text.ToString() + " already exists!");
                    }


                }
                else
                {
                    MessageBox.Show("Name and User ID should be different !");
                }

            }
            else
            {
                MessageBox.Show("Passwords don't match!");
            }
            //}


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            
            textBoxAddInfoAccount.Text = "";
            textBoxAddInfoUserName.Text = "";
            textBoxAddInfoPassword.Text = "";
            textBoxRetypePassword.Text = "";

            groupBoxAddInfo.Show();
            groupBoxSignIn.Hide();
            groupBoxSignUp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxMain.Hide();
            groupBoxHelp.Hide();
            groupBoxChangePassword.Hide();

        }

        private void buttonAddInfo_Click(object sender, EventArgs e)
        {
            string addAccount = textBoxAddInfoAccount.Text.ToString();
            string addUserName = textBoxAddInfoUserName.Text.ToString();
            string addPassword = textBoxAddInfoPassword.Text.ToString();
            string retypePassword = textBoxRetypePassword.Text.ToString();
            string line;
            int success = 1;
            string accountInfo = addAccount + ";" + addUserName + ";" + addPassword + ";" + DateTime.Now + ";";

            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            if (addPassword == retypePassword)
            {
                if (File.Exists("c:\\key_ring\\Account_info\\" + uName + ".key"))
                {
                    // open the file containing the key and IV
                    FileStream fsKeyInChecking = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

                    // use a BinaryReader to read formatted data from the file
                    BinaryReader brChecking = new BinaryReader(fsKeyInChecking);

                    // read data from the file and close it
                    tdes.Key = brChecking.ReadBytes(24);
                    tdes.IV = brChecking.ReadBytes(8);

                    // Open the encrypted file
                    FileStream fsInChecking = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

                    // Create a cryptostream to decrypt from the filestream
                    CryptoStream csChecking = new CryptoStream(fsInChecking, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                    // Create a StreamReader to format the input
                    StreamReader tw3Checking = new StreamReader(csChecking);

                    while ((line = tw3Checking.ReadLine()) != null && line.Length > 0)
                    {
                        if (line.IndexOf(addAccount.ToString() + ";") != -1)
                        {
                            success = 0;
                        }
                        else
                        {
                            success = 1;

                        }

                    }
                    csChecking.Close();
                    brChecking.Close();
                    tw3Checking.Close();

                    if(success == 0)
                    {
                    MessageBox.Show(addAccount.ToString() + " already exists!");
                    }
                    else
                    {
                        // open the file containing the key and IV
                        FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

                        // use a BinaryReader to read formatted data from the file
                        BinaryReader br = new BinaryReader(fsKeyIn);

                        // read data from the file and close it
                        tdes.Key = br.ReadBytes(24);
                        tdes.IV = br.ReadBytes(8);

                        // Open the encrypted file
                        FileStream fsIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

                        // Create a cryptostream to decrypt from the filestream
                        CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                        // Create a StreamReader to format the input
                        StreamReader sr = new StreamReader(cs);

                        // create a writer and open a temporary file
                        FileStream fs = new FileStream("c:\\key_ring\\Account_info\\" + uName + "_tmp.txt", FileMode.Append, FileAccess.Write);

                        // Create a cryptostream to encrypt to the filestream
                        CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

                        //StreamWriter tw = new StreamWriter(fs);
                        StreamWriter tw = new StreamWriter(csencr);

                        // And decrypt the data and write in a temporary file

                        while ((line = sr.ReadLine()) != null && line.Length > 0)
                        {
                            tw.WriteLine(line);
                        }
                        // write the new account info in the temp file.......
                        //tw.WriteLine(";this is new info");
                        tw.Write(accountInfo);
                        MessageBox.Show("Information added.");
                        // close the stream
                        tw.Flush();
                        tw.Close();
                        sr.Close();
                        File.Delete("c:\\key_ring\\Account_info\\" + uName + ".txt");
                        File.Move(@"c:\\key_ring\\Account_info\\" + uName + "_tmp.txt", @"c:\\key_ring\\Account_info\\" + uName + ".txt");
                        br.Close();
                    }
                }
                else
                {
                    // create a writer to write the new Account info
                    FileStream fs = new FileStream("c:\\key_ring\\Account_info\\" + uName + ".txt", FileMode.Append, FileAccess.Write);

                    // Create a cryptostream to encrypt to the filestream
                    CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

                    //StreamWriter tw = new StreamWriter(fs);
                    StreamWriter tw = new StreamWriter(csencr);

                    // write the new user info in the Account file.......
                    //tw.WriteLine(";this is new info");
                    tw.WriteLine(accountInfo);
                    MessageBox.Show("Information added.");
                    textBoxAddInfoAccount.Text = "";
                    textBoxAddInfoUserName.Text = "";
                    textBoxAddInfoPassword.Text = "";
                    textBoxRetypePassword.Text = "";

                    // close the stream
                    tw.Flush();
                    tw.Close();
                }
                // save the key and IV for future use

                FileStream fsKeyOut = File.Create(@"c:\\key_ring\\Account_info\\" + uName + ".key");

                // use a BinaryWriter to write formatted data to the file
                BinaryWriter bw = new BinaryWriter(fsKeyOut);

                // write data to the file
                bw.Write(tdes.Key);
                bw.Write(tdes.IV);

                // flush and close
                bw.Flush();
                bw.Close();

                //now some steps is going to be taken to generate the ComboBox with new data
                //...........................................................................

                // open the file containing the key and IV
                FileStream fsKeyInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

                // use a BinaryReader to read formatted data from the file
                BinaryReader brCombo = new BinaryReader(fsKeyInCombo);

                // read data from the file and close it
                tdes.Key = brCombo.ReadBytes(24);
                tdes.IV = brCombo.ReadBytes(8);

                // Open the encrypted file
                FileStream fsInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

                // Create a cryptostream to decrypt from the filestream
                CryptoStream csread = new CryptoStream(fsInCombo, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                // Create a StreamReader to format the input
                StreamReader srread = new StreamReader(csread);

                // And decrypt the data
                comboBoxAccount.Items.Clear();
                while ((line = srread.ReadLine()) != null && line.Length > 0)
                {
                    int i = line.IndexOf(";");
                    string account = line.Substring(0, i);
                    comboBoxAccount.Items.Add(account);

                }

                brCombo.Close();
                
                srread.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match!");
            }
                
        }

        private void buttonBackToLogin_Click(object sender, EventArgs e)
        {
            userName.Text = "";
            password.Text = "";
            codeWord.Text = "";

            groupBoxSignIn.Show();
            groupBoxMain.Hide();
            groupBoxAddInfo.Hide();
            groupBoxSignUp.Hide();
            groupBoxHelp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxChangePassword.Hide();
        }

        private void name_Popup(object sender, PopupEventArgs e)
        {

        }

        private void buttonAddInformationBack_Click(object sender, EventArgs e)
        {
            
            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxLastUpdate.Text = "";

            
            groupBoxMain.Show();
            groupBoxSignIn.Hide();
            groupBoxSignUp.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxChangePassword.Hide();
        }

        private void buttonAddInformationExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line;
            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            // open the file containing the key and IV
            FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader br = new BinaryReader(fsKeyIn);

            // read data from the file and close it
            tdes.Key = br.ReadBytes(24);
            tdes.IV = br.ReadBytes(8);

            // Open the encrypted file
            FileStream fsIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader tw3 = new StreamReader(cs);

            while ((line = tw3.ReadLine()) != null && line.Length > 0)
            {
                if (line.IndexOf(comboBoxAccount.SelectedItem.ToString() + ";") != -1)
                {
                    int firstComma = line.IndexOf(";");
                    string necessaryInfo = line.Substring(firstComma + 1, line.Length - firstComma - 1);
                    textBoxUserName.Text = necessaryInfo.Substring(0, necessaryInfo.IndexOf(";"));

                    string passwordDateInfo = necessaryInfo.Substring(necessaryInfo.IndexOf(";") + 1, necessaryInfo.Length - necessaryInfo.IndexOf(";") - 1);
                    textBoxPassword.Text = passwordDateInfo.Substring(0, passwordDateInfo.IndexOf(";"));

                    string dateInfo = passwordDateInfo.Substring(passwordDateInfo.IndexOf(";") + 1, passwordDateInfo.Length - passwordDateInfo.IndexOf(";") - 1);
                    textBoxLastUpdate.Text = dateInfo.Substring(0, dateInfo.IndexOf(";"));
                }
            }
            tw3.Close();
            fsKeyIn.Close();
            br.Close();

            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string line;

            //create the temporaray file if it doesnt exits............
            if (!File.Exists("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt"))
            {
                File.Create("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt").Close();
            }
            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            // open the file containing the key and IV
            FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader br = new BinaryReader(fsKeyIn);

            // read data from the file and close it
            tdes.Key = br.ReadBytes(24);
            tdes.IV = br.ReadBytes(8);

            // Open the encrypted file
            FileStream fsIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader sr = new StreamReader(cs);

            //now take some steps to write in an encrypted way in the temp file..........
            //...........................................................................
            // create a writer and open a temporary file
            FileStream fs = new FileStream("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt", FileMode.Append, FileAccess.Write);

            // Create a cryptostream to encrypt to the filestream
            CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

            //StreamWriter tw = new StreamWriter(fs);
            StreamWriter tw = new StreamWriter(csencr);

            // And decrypt the data and write in a temporary file

            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                if (line.IndexOf(comboBoxAccount.SelectedItem.ToString()) == -1)
                {
                    tw.WriteLine(line);
                }

            }
            // write the new user info in the temp file.......
            //tw.WriteLine("this is new user");
            //tw.WriteLine(userInfo);
            // close the stream
            tw.Flush();
            tw.Close();
            sr.Close();
            br.Close();
            //fsKeyIn.Flush();
            fsKeyIn.Close();
            File.Delete("c:\\key_ring\\Account_info\\" + uName + ".txt");
            File.Move(@"c:\\key_ring\\Account_info\\" + uName + ".tmp.txt", @"c:\\key_ring\\Account_info\\" + uName + ".txt");


            // save the key and IV for future use

            FileStream fsKeyOut = File.Create(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryWriter to write formatted data to the file
            BinaryWriter bw = new BinaryWriter(fsKeyOut);

            // write data to the file
            bw.Write(tdes.Key);
            bw.Write(tdes.IV);

            // flush and close
            bw.Flush();
            bw.Close();

            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxLastUpdate.Text = "";

            //now some steps is going to be taken to generate the ComboBox with new data
            //...........................................................................

            // open the file containing the key and IV
            FileStream fsKeyInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader brCombo = new BinaryReader(fsKeyInCombo);

            // read data from the file and close it
            tdes.Key = brCombo.ReadBytes(24);
            tdes.IV = brCombo.ReadBytes(8);

            // Open the encrypted file
            FileStream fsInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream csread = new CryptoStream(fsInCombo, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader srread = new StreamReader(csread);

            // And decrypt the data
            comboBoxAccount.Items.Clear();
            while ((line = srread.ReadLine()) != null && line.Length > 0)
            {
                int i = line.IndexOf(";");
                string account = line.Substring(0, i);
                comboBoxAccount.Items.Add(account);
                //comboBoxAccount.Items.Add(line);
            }

            brCombo.Close();
            srread.Close();
            fsInCombo.Close();
            button3.Enabled = false;
            MessageBox.Show("Information removed.");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //creating the updated information..........
            string newLine = comboBoxAccount.SelectedItem.ToString() + ";" + textBoxUserName.Text.ToString() + ";" + textBoxPassword.Text.ToString() + ";" + textBoxLastUpdate.Text.ToString() + ";";
            string line;

            //create the temporaray file if it doesnt exits............
            if (!File.Exists("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt"))
            {
                File.Create("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt").Close();
            }
            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            // open the file containing the key and IV
            FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader br = new BinaryReader(fsKeyIn);

            // read data from the file and close it
            tdes.Key = br.ReadBytes(24);
            tdes.IV = br.ReadBytes(8);

            // Open the encrypted file
            FileStream fsIn = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader sr = new StreamReader(cs);

            //now take some steps to write in an encrypted way in the temp file..........
            //...........................................................................
            // create a writer and open a temporary file
            FileStream fs = new FileStream("c:\\key_ring\\Account_info\\" + uName + ".tmp.txt", FileMode.Append, FileAccess.Write);

            // Create a cryptostream to encrypt to the filestream
            CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

            //StreamWriter tw = new StreamWriter(fs);
            StreamWriter tw = new StreamWriter(csencr);

            // And decrypt the data and write in a temporary file

            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                if (line.IndexOf(comboBoxAccount.SelectedItem.ToString()) == -1)
                {
                    tw.WriteLine(line);
                }

            }
            //write the updated info in the file.......
            tw.WriteLine(newLine);

            // write the new user info in the temp file.......
            //tw.WriteLine("this is new user");
            //tw.WriteLine(userInfo);
            // close the stream
            tw.Flush();
            tw.Close();
            sr.Close();
            br.Close();
            //fsKeyIn.Flush();
            fsKeyIn.Close();
            File.Delete("c:\\key_ring\\Account_info\\" + uName + ".txt");
            File.Move(@"c:\\key_ring\\Account_info\\" + uName + ".tmp.txt", @"c:\\key_ring\\Account_info\\" + uName + ".txt");


            // save the key and IV for future use

            FileStream fsKeyOut = File.Create(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryWriter to write formatted data to the file
            BinaryWriter bw = new BinaryWriter(fsKeyOut);

            // write data to the file
            bw.Write(tdes.Key);
            bw.Write(tdes.IV);

            // flush and close
            bw.Flush();
            bw.Close();

            //textBoxUserName.Text = "";
            //textBoxPassword.Text = "";
            //textBoxLastUpdate.Text = "";

            //now some steps is going to be taken to generate the ComboBox with new data
            //...........................................................................

            // open the file containing the key and IV
            FileStream fsKeyInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader brCombo = new BinaryReader(fsKeyInCombo);

            // read data from the file and close it
            tdes.Key = brCombo.ReadBytes(24);
            tdes.IV = brCombo.ReadBytes(8);

            // Open the encrypted file
            FileStream fsInCombo = File.OpenRead(@"c:\\key_ring\\Account_info\\" + uName + ".txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream csread = new CryptoStream(fsInCombo, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader srread = new StreamReader(csread);

            // And decrypt the data
            comboBoxAccount.Items.Clear();
            while ((line = srread.ReadLine()) != null && line.Length > 0)
            {
                int i = line.IndexOf(";");
                string account = line.Substring(0, i);
                comboBoxAccount.Items.Add(account);
                //comboBoxAccount.Items.Add(line);
            }

            brCombo.Close();
            MessageBox.Show("Information Updated.");

            button4.Enabled = false;
            button3.Enabled = false;
            textBoxUserName.Text = "";
            textBoxPassword.Text = "";
            textBoxLastUpdate.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBoxSignIn.Hide();
            groupBoxMain.Hide();
            groupBoxSignUp.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Show();
            groupBoxAdminPanel.Hide();
            groupBoxChangePassword.Hide();
        }

        private void buttonHelpBack_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;

            groupBoxSignIn.Hide();
            groupBoxMain.Show();
            groupBoxSignUp.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxChangePassword.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBoxChangePasswordNewPassword.Text = "";
            textBoxChangePasswordRetypeNewPassword.Text = "";
            
            groupBoxSignIn.Hide();
            groupBoxMain.Show();
            groupBoxSignUp.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxChangePassword.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBoxChangePasswordNewPassword.Text.ToString() == textBoxChangePasswordRetypeNewPassword.Text.ToString())
            {
                string line;

                //create the temporaray file if it doesnt exits............
                if (!File.Exists("c:\\key_ring\\user_info_tmp.txt"))
                {
                    File.Create("c:\\key_ring\\user_info_tmp.txt").Close();
                }
                // Create a new crypto provider
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                // open the file containing the key and IV
                FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\encrypted.key");

                // use a BinaryReader to read formatted data from the file
                BinaryReader br = new BinaryReader(fsKeyIn);

                // read data from the file and close it
                tdes.Key = br.ReadBytes(24);
                tdes.IV = br.ReadBytes(8);

                // Open the encrypted file
                FileStream fsIn = File.OpenRead(@"c:\\key_ring\\user_info.txt");

                // Create a cryptostream to decrypt from the filestream
                CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

                // Create a StreamReader to format the input
                StreamReader sr = new StreamReader(cs);

                //now take some steps to write in an encrypted way in the temp file..........
                //...........................................................................
                // create a writer and open a temporary file
                FileStream fs = new FileStream("c:\\key_ring\\user_info_tmp.txt", FileMode.Append, FileAccess.Write);

                // Create a cryptostream to encrypt to the filestream
                CryptoStream csencr = new CryptoStream(fs, tdes.CreateEncryptor(), CryptoStreamMode.Write);

                //StreamWriter tw = new StreamWriter(fs);
                StreamWriter tw = new StreamWriter(csencr);

                // And decrypt the data and write in a temporary file

                while ((line = sr.ReadLine()) != null && line.Length > 0)
                {
                    if (line.IndexOf(uName.ToString()) == -1)
                    {
                        tw.WriteLine(line);
                    }
                    else
                    {
                        int firstComma = line.IndexOf(";");
                        string name = line.Substring(0, firstComma);
                        string necessaryInfo = line.Substring(firstComma + 1, line.Length - firstComma - 1);
                        //textBoxUserName.Text = necessaryInfo.Substring(0, necessaryInfo.IndexOf(";"));
                        string userName = uName;
                        string passwordEmailInfo = necessaryInfo.Substring(necessaryInfo.IndexOf(";") + 1, necessaryInfo.Length - necessaryInfo.IndexOf(";") - 1);
                        //string password = passwordEmailInfo.Substring(0, passwordEmailInfo.IndexOf(";"));
                        string password = textBoxChangePasswordNewPassword.Text.ToString();
                        string emailInfo = passwordEmailInfo.Substring(passwordEmailInfo.IndexOf(";") + 1, passwordEmailInfo.Length - passwordEmailInfo.IndexOf(";") - 1);
                        string email = emailInfo.Substring(0, emailInfo.IndexOf(";"));

                        string newInfo = name + ";" + userName + ";" + password + ";" + email + ";";
                        
                        tw.WriteLine(newInfo);
                    }
                }
                
                // close the stream
                tw.Flush();
                tw.Close();
                sr.Close();
                br.Close();
                fsKeyIn.Close();
                File.Delete("c:\\key_ring\\user_info.txt");
                File.Move(@"c:\\key_ring\\user_info_tmp.txt", @"c:\\key_ring\\user_info.txt");


                // save the key and IV for future use
                FileStream fsKeyOut = File.Create(@"c:\\key_ring\\encrypted.key");

                // use a BinaryWriter to write formatted data to the file
                BinaryWriter bw = new BinaryWriter(fsKeyOut);

                // write data to the file
                bw.Write(tdes.Key);
                bw.Write(tdes.IV);

                // flush and close
                bw.Flush();
                bw.Close();

                groupBoxSignIn.Show();
                groupBoxMain.Hide();
                groupBoxSignUp.Hide();
                groupBoxAddInfo.Hide();
                groupBoxHelp.Hide();
                groupBoxAdminPanel.Hide();
                groupBoxChangePassword.Hide();

                textBoxChangePasswordNewPassword.Text = "";
                textBoxChangePasswordRetypeNewPassword.Text = "";
                //userName.Text = "";
                //password.Text = "";
                codeWord.Text = "";
               
            }
            else
            {
                MessageBox.Show("Passwords do not match!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBoxSignIn.Hide();
            groupBoxMain.Hide();
            groupBoxSignUp.Hide();
            groupBoxAdminPanel.Hide();
            groupBoxAddInfo.Hide();
            groupBoxHelp.Hide();
            groupBoxChangePassword.Show();
        }

        private void buttonAdminPanelExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line;
            // Create a new crypto provider
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            // open the file containing the key and IV
            FileStream fsKeyIn = File.OpenRead(@"c:\\key_ring\\encrypted.key");

            // use a BinaryReader to read formatted data from the file
            BinaryReader br = new BinaryReader(fsKeyIn);

            // read data from the file and close it
            tdes.Key = br.ReadBytes(24);
            tdes.IV = br.ReadBytes(8);

            // Open the encrypted file
            FileStream fsIn = File.OpenRead(@"c:\\key_ring\\user_info.txt");

            // Create a cryptostream to decrypt from the filestream
            CryptoStream cs = new CryptoStream(fsIn, tdes.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader to format the input
            StreamReader tw3 = new StreamReader(cs);

            while ((line = tw3.ReadLine()) != null && line.Length > 0)
            {
                if (line.IndexOf(comboBoxUserName.SelectedItem.ToString()) != -1 )
                {
                    int firstComma = line.IndexOf(";");
                    string necessaryInfo = line.Substring(firstComma + 1, line.Length - firstComma - 1);
                    //textBoxUserName.Text = necessaryInfo.Substring(0, necessaryInfo.IndexOf(";"));

                    string passwordDateInfo = necessaryInfo.Substring(necessaryInfo.IndexOf(";") + 1, necessaryInfo.Length - necessaryInfo.IndexOf(";") - 1);
                    textBoxAdminPanelPassword.Text = passwordDateInfo.Substring(0, passwordDateInfo.IndexOf(";"));

                    string eMailInfo = passwordDateInfo.Substring(passwordDateInfo.IndexOf(";") + 1, passwordDateInfo.Length - passwordDateInfo.IndexOf(";") - 1);
                    textBoxAdminPanelEmail.Text = eMailInfo.Substring(0, eMailInfo.IndexOf(";"));
                    
                }
            }
            tw3.Close();
            fsKeyIn.Close();
            br.Close();
        }

        private void linkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Contact here 'rizvi_iut@yahoo.com'");
        }
    }
}