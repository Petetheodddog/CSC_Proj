using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataFormats = System.Windows.DataFormats;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RichTextBox = System.Windows.Forms.RichTextBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace CSC_Proj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //**Adding menu item functionality**

        //To add menu item functionality go to xaml and add 
        //<MenuItem Header="Font" HorizontalAlignment="Left" Width="140" Click="MenuItem_Click_Font"/>

        //THIS PART: Click="MenuItem_Click_Font"

        //With a descriptive name of what it is like 'Font'
        //plz always stick to the same naming conventions, 
        //this is a big project.

        #region Program wide usefull methods

        //duh it clears the text box
        private void clear()
        {
            MainTextBox.SelectAll();

            MainTextBox.Selection.Text = "";
        }


        #endregion

        #region File Menu Items

        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save");
        }

        private void MenuItem_Click_SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text Files (.txt)|*.txt|Rich Text Files(.rtf)|*.rtf|All Files (*.*)|*.*";
            saveFile.Title = "Save file...";

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = saveFile.ShowDialog();

            if (userClickedOK == true)
            {
                //because rtf.Text isn't a thing
                //copies the enitre richtextbox and then saves it as rtfBox because you can't do mainTexBox.Text
                MainTextBox.SelectAll();
                string rtfBox = MainTextBox.Selection.Text;

                System.IO.StreamWriter saveWrite = new StreamWriter(saveFile.FileName);
                //saveWrite.Write(MainTextBox.Document.ContentStart);       Doesn't really work should though


                //copied code

                //System.IO.FileStream fStream;
                //TextRange range;


                //range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                //fStream = new System.IO.FileStream(saveFile.FileName, System.IO.FileMode.OpenOrCreate);
                //string format = "";

                //if (saveFile.Filter == ".txt")
                //{
                ////   format = DataFormats.Rtf;
                //     MainTextBox.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                //}

                //range.Load(fStream, format);

                //end coppied code

                saveWrite.Write(rtfBox);
                saveWrite.Close();
            }
        }

        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|Rich Text Files(.rtf)|*.rtf";
            //openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Open A file...";

            //if you want to select multiple files, but why?
            //openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                ////first clear the text box
                clear();

                System.IO.FileStream fStream;
                TextRange range;


                if (System.IO.File.Exists(openFileDialog1.FileName))
                {
                    range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                    fStream = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.OpenOrCreate);
                    string format = "";
                    
                    if (openFileDialog1.FileName.EndsWith(".txt"))
                    {
                        format = DataFormats.Text;
                    }

                    else
                    {
                        format = DataFormats.Rtf;
                    }

                    range.Load(fStream, format);

                    fStream.Close();
                }
            }        
        }

        #endregion

        #region Edit Menu Items

        private void MenuItem_Click_Undo(object sender, RoutedEventArgs e)
        {
            MainTextBox.Undo();
        }

        private void MenuItem_Click_Redo(object sender, RoutedEventArgs e)
        {
            MainTextBox.Redo();
        }

        #endregion

        #region View Menu Items


        private void MenuItem_Click_Font(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Font");
            
        }

        private void MenuItem_Click_Size(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Size");
        }

        private void MenuItem_Click_Colour(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Colour");
        }

        #endregion

        
    }
}
