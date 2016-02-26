using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Microsoft.Win32;



namespace Passguard_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String file;
        public static string[] files;
        public static string file_name;
        public static string file_extension;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
        

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // the user can drag in more than one file so wqe only need to use the first selected
                //file
                files = (string[])e.Data.GetData(DataFormats.FileDrop);

                //array files holds the paths to the files
                //first declare file types that are not allowed
                string[] unallowed_files = {
                                            ".exe",
                                            ".lnk",
                                            ".bat",
                                            ".ini"
                                        };

                //we then check for the file extension
                foreach (string unallowed_file in unallowed_files)
                {
                    file_extension = Path.GetExtension(files[0]);
                    if (file_extension == unallowed_file)
                    {
                        //file type is forbidden
                        MessageBox.Show("File type "+unallowed_file+" is forbidden.");

                        //empty file holder
                        files[0] = null;
                    }

                }
                if (files[0]!= null){

                    //update UI showing file has been recieved
                    noticeString.Content = "File: "+Path.GetFileName(files[0]) + " has been received.";

                    //close uploading
                    ImagePanel.IsEnabled = false;
                    noticeString.FontSize = 9;

                    //we now read the file to base64 encoding
                    Byte[] bytes = File.ReadAllBytes(files[0]);
                    file = Convert.ToBase64String(bytes);

                    
                }
            }
        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (file == null)
            {
                MessageBox.Show("Nothing to encrypt");
            }
            else
            {

                //start encryption process
                Passguard_Windows.Cryptlib.CryptLib _crypt = new Passguard_Windows.Cryptlib.CryptLib();

                String iv = Passguard_Windows.Cryptlib.CryptLib.GenerateRandomIV(16); //16 bytes = 128 bits
                string key = Passguard_Windows.Cryptlib.CryptLib.getHashSha256("password", 31); //32 bytes = 256 bits
                String cypherText = _crypt.encrypt(file, key, iv);
                
                try
                {
                    file_name = Path.GetFileNameWithoutExtension(files[0]); 
                    MessageBox.Show(file_name + " has been successfully encrypted!");
                    
                    
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        Filter = "All(*.*)|*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                         File.WriteAllText(file_name+file_extension, cypherText);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }
        

        private void reset_uploading(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Massive error here. Stay away ");
            /*
             * CAUTION: An unhandled exception of type 'System.StackOverflowException' ERROR
            //empyt out files array
            files[0] = null;

            //update UI showing file has been recieved
            noticeString.Content = this.Content;

            //close uploading
            ImagePanel.IsEnabled = true;
            noticeString.FontSize = this.FontSize;*/
        }

    }
}