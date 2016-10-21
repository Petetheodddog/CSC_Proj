using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Printing;
using System.Drawing;
using System.Windows.Xps;
using System.Drawing.Printing;
using DataFormats = System.Windows.DataFormats;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RichTextBox = System.Windows.Forms.RichTextBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Diagnostics;


namespace CSC_Proj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch watch = new Stopwatch();
        Stopwatch watchCopy = new Stopwatch();
        bool firstTime;

        public MainWindow()
        {
            
            InitializeComponent();
            watch = System.Diagnostics.Stopwatch.StartNew();
            MainTextBox.SpellCheck.IsEnabled = true;
            firstTime = true;

        }

        //**Adding menu item functionality**

        //To add menu item functionality go to xaml and add 
        //<MenuItem Header="Font" HorizontalAlignment="Left" Width="140" Click="MenuItem_Click_Font"/>

        //THIS PART: Click="MenuItem_Click_Font"

        //With a descriptive name of what it is like 'Font'
        //plz always stick to the same naming conventions, 
        //this is a big project.

        int lineCount = 1;










        private void MainTextBox_keyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textRange = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            var b = 0;
            TextPointer textPointer = MainTextBox.Selection.Start;
            if (e.Key == Key.Return)
            {
                lineCount++;
                b = 1;
            }
            if (e.Key == Key.Back && textPointer.GetTextInRun(LogicalDirection.Backward).LastOrDefault().GetHashCode() == 0)
            {
                lineCount--;
                b = -1;
            }

            // back up code: 
            //var t = textRange.Text.Where(x => x == '\n').Count();
            if (b != 0)
            {
                //lineCount = t;
                var sb = new StringBuilder();
                //for (int i = 1; i <= lineCount; i++) sb.Append(i).Append("\n\n");
                if (b == 1)
                {
                    txt__label.Text += lineCount + "\n\n";
                }
                if (b == -1)
                {
                    try
                    {
                        txt__label.Text = txt__label.Text.Remove(txt__label.Text.Length - 3);
                    }
                    catch (Exception)
                    {

                       //Minor bug :)
                    }
                  
                }
                //txt__label.Text = sb.ToString();

            }

        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            txt__label.ScrollToVerticalOffset(e.VerticalOffset);
        }

        #region Program wide usefull methods & global variables

        //duh it clears the text box
        private void clear()
        {
            MainTextBox.SelectAll();

            MainTextBox.Selection.Text = "";
        }

        private string filePath = "";

        #endregion

        //Done || Can still be inproved if anyone wants to give it a shot, work on shortcuts for save, new etc. alos the RTF save file
        #region File Menu Items

        //      PRINT function
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            TextRange sourceDocument = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);

            MemoryStream stream = new MemoryStream();

            sourceDocument.Save(stream, DataFormats.Xaml);



            // Clone the source document�s content into a new FlowDocument.

            FlowDocument flowDocumentCopy = new FlowDocument();

            TextRange copyDocumentRange = new TextRange(flowDocumentCopy.ContentStart, flowDocumentCopy.ContentEnd);

            copyDocumentRange.Load(stream, DataFormats.Xaml);



            // Create a XpsDocumentWriter object, open a Windows common print dialog.

            // This methods returns a ref parameter that represents information about the dimensions of the printer media.

            PrintDocumentImageableArea ia = null;

            XpsDocumentWriter docWriter = PrintQueue.CreateXpsDocumentWriter(ref ia);



            if (docWriter != null && ia != null)

            {

                DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocumentCopy).DocumentPaginator;



                // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.

                paginator.PageSize = new System.Windows.Size(ia.MediaSizeWidth, ia.MediaSizeHeight);

                Thickness pagePadding = flowDocumentCopy.PagePadding;

                flowDocumentCopy.PagePadding = new Thickness(

                        Math.Max(ia.OriginWidth, pagePadding.Left),

                        Math.Max(ia.OriginHeight, pagePadding.Top),

                        Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), pagePadding.Right),

                        Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), pagePadding.Bottom));

                flowDocumentCopy.ColumnWidth = double.PositiveInfinity;

                // Send DocumentPaginator to the printer.

                docWriter.Write(paginator);

            }

        }

        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            MainTextBox.SelectAll();
            string rtfBox = MainTextBox.Selection.Text;

            System.IO.StreamWriter saveWrite = new StreamWriter(filePath);

            saveWrite.Write(rtfBox);
            saveWrite.Close();

            //if you dont do this everything in the text box remains selected
            MainTextBox.Copy();
            MainTextBox.Paste();
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

                filePath = saveFile.FileName;

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
            openFileDialog1.FilterIndex = 1;    //TBH not too sure what this does, but hey lets leave it
            openFileDialog1.Title = "Open A file...";

            //Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();     //makes the if statement below easier 

            //Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                //first clear the text box
                clear();

                System.IO.FileStream fStream;
                TextRange range;

                if (System.IO.File.Exists(openFileDialog1.FileName))
                {
                    range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                    fStream = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.OpenOrCreate);
                    string format = "";

                    //writes the data in the correct format for txt
                    if (openFileDialog1.FileName.EndsWith(".txt"))
                    {
                        format = DataFormats.Text;
                    }

                    //writes the data in the correct format for rtf 
                    else
                    {
                        format = DataFormats.Rtf;
                    }

                    //used for save/save as and finding the last path of a file to automatically write to the correct place.
                    filePath = openFileDialog1.FileName;

                    range.Load(fStream, format);

                    fStream.Close();
                }
            }
        }

        #endregion
        //Only other text editors just cant open the rtf files that get saved

        //Done
        #region Edit Menu Items

        private void MenuItem_Click_Undo(object sender, RoutedEventArgs e)
        {
            MainTextBox.Undo();
        }

        private void MenuItem_Click_Redo(object sender, RoutedEventArgs e)
        {
            MainTextBox.Redo();
        }

        private void MenuItem_Click_Cut(object sender, RoutedEventArgs e)
        {
            MainTextBox.Cut();
        }

        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            MainTextBox.Copy();
        }

        private void MenuItem_Click_Paste(object sender, RoutedEventArgs e)
        {
            MainTextBox.Paste();
        }

        private void MenuItem_Click_SelectAll(object sender, RoutedEventArgs e)
        {
            MainTextBox.SelectAll();
        }
        #endregion

        //Todo, well there is nothing to do yet
        #region Insert Menu Items


        #endregion

        //Done
        #region Format Menu Items

        System.Windows.Forms.FontDialog openFontDialog1;


        private void MenuItem_Click_Font(object sender, RoutedEventArgs e)
        {

            MainTextBox.Focus();
            if (openFontDialog1 == null) openFontDialog1 = new System.Windows.Forms.FontDialog();    // keeps selected text style in font menu

            if (openFontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fontName;
                float fontSize;
                string fontStyle = Convert.ToString(openFontDialog1.Font.Style);
                bool underline = openFontDialog1.Font.Underline;
                bool strikeout = openFontDialog1.Font.Strikeout;

                fontName = openFontDialog1.Font.Name;
                fontSize = openFontDialog1.Font.Size;

                TextRange text = new TextRange(MainTextBox.Selection.Start, MainTextBox.Selection.End);     // selected text


                //                    Underline and crossthrough
                if (underline)
                {
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                }
                if (strikeout)
                {
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Strikethrough);
                }

                if (!strikeout && !underline)
                { text.ApplyPropertyValue(Run.TextDecorationsProperty, null); }

                if (strikeout && !underline)
                {
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Strikethrough);
                }

                if (!strikeout && underline)
                {
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, null);
                    text.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                }


                //                              Change style

                if (fontStyle == "Italic")
                { text.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic); }

                if (fontStyle == "Bold, Italic")
                {
                    text.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Oblique);
                    text.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                }

                if (fontStyle == "Bold")
                { text.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold); }

                if (fontStyle == "Regular")
                {
                    text.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
                    text.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
                }


                //                          Change font size

                text.ApplyPropertyValue(TextElement.FontSizeProperty, (double)fontSize);


                //                          Change font Family

                text.ApplyPropertyValue(TextElement.FontFamilyProperty, fontName);

            }
        }


        //                           Change Color

        private void MenuItem_Click_Colour(object sender, RoutedEventArgs e)
        {
            TextRange text = new TextRange(MainTextBox.Selection.Start, MainTextBox.Selection.End);     // selected text

            System.Windows.Forms.ColorDialog ShowColorDialog = new System.Windows.Forms.ColorDialog();

            if (ShowColorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color c = ShowColorDialog.Color;

                var drawingcolor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);

                SolidColorBrush brush = new SolidColorBrush(drawingcolor);

                text.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }

        }

        #endregion

        //Todo, well there is nothing to do yet
        #region Review Menu Items

        private void MenuItem_Click_WordCount(object sender, RoutedEventArgs e)
        {
            Stopwatch watchCopy = new Stopwatch();

            MainTextBox.SelectAll();
            string rtfBox = MainTextBox.Selection.Text;

            //unhighlights all the text
            MainTextBox.Copy();
            MainTextBox.Paste();

            string[] eh = null;

            string[] words = rtfBox.Split(eh, StringSplitOptions.RemoveEmptyEntries);

            MessageBox.Show(string.Format("There are {0} words in the file.", words.Length));
        }


        #endregion

        private void MenuItem_Click_Time(object sender, RoutedEventArgs e)
        {
            string t = DateTime.Now.ToLongDateString() + "\n" +  DateTime.Now.ToLongTimeString();

            MainTextBox.CaretPosition.InsertTextInRun( t);

            //MessageBox.Show(t);
        }

        private void MenuItem_Click_Rate(object sender, RoutedEventArgs e)
        {
             
            //populate string [] with no spaces of text
            string[] start = null;
            MainTextBox.SelectAll();
            string p = MainTextBox.Selection.Text;

            string[] temp = p.Split(start, StringSplitOptions.RemoveEmptyEntries);
            List<string> words = new List<string>(temp);



            
            string plural = "";

            string output = "";


            var watchCopy = watch;


          

            watchCopy = watch;
            watchCopy.Stop();
            double elpsdSeconds = watch.Elapsed.TotalSeconds;
            int minutes = watch.Elapsed.Minutes;
            int seconds = Convert.ToInt32(elpsdSeconds);
          

            //deals with wether one word was typed
            if (plural.Length == 1) plural = "word";
            else plural = "words";


            Double time = 0;
            string repeat = "";
            //deals with syntax of first check vs revisit
            if (firstTime == true) repeat = "start";
            if (firstTime == false) repeat = "lastchecked";

            

            if (minutes == 0)
            {
                time = (seconds);
                output = string.Format("You have typed {0} {1} in {2} seconds \n since your {3}.", (words.Count), plural, seconds, repeat);

            }

             if (minutes==1)
            {
                time = (minutes);
                output = string.Format("You have typed {0} {1} per minute {2} \n since your {3}.", (words.Count), plural, time, repeat);
            }

            else if(minutes>1)
            {

                time = (minutes);
                output = string.Format("You have typed {0} {1} per {2} minutes  \n since your {3}.", (words.Count), plural, time, repeat);
            }
            

            watchCopy.Restart();
            firstTime = false;
            MessageBox.Show(output);
        }
    }
}