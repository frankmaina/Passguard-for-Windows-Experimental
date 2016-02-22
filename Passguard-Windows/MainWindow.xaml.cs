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

namespace Passguard_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String file;
        public static string[] files;

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
                    string extension = Path.GetExtension(files[0]);
                    if (extension == unallowed_file)
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
                    noticeString.FontSize = 15;

                    //we now read the file to base64 encoding
                    Byte[] bytes = File.ReadAllBytes(files[0]);
                    file = Convert.ToBase64String(bytes);

                    /*
                     * Do encryption here
                     * 
                     */

                }
            }
        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (file == null)
            {
                MessageBox.Show("Nothing to encrypt");
            }
        }
        

        private void reset_uploading(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Massive error here. Stay away ")
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