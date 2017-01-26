/*
* FILE : MainWindow.xaml.cs
* PROJECT : PROG2120 - Final Project
* PROGRAMMER : Jacob Huras, Anthony Bastos
* FIRST VERSION : 2016-12-13
* DESCRIPTION :
* The methods in this file are used to handle events used in the Main
* Window of the application. It alos initializes compenents required by the Main Window.
*/


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
using System.Windows.Shapes;
using System.Windows.Forms;

namespace SETPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum ShapeType
        {
            line,
            ellipse,
            rectangle
        }
        public ShapeType activeShapeType;
        public int activeStrokeThickness;
        public bool newShape;
        public Point startPoint;
        public Brush strokeColor;
        public Brush fillColor;



        /*
        FUNCTION: MainWindow
        DESCRIPTION:
	        This initializes the window and global values.
        PARAMETERS:
	        None.
        RETURNS:
	        None.
        */
        public MainWindow()
        {
            InitializeComponent();
            newShape = false;
            strokeColor = Brushes.Black;
            fillColor = Brushes.Black;
            ClrPcker_Stroke.SelectedColor = Colors.Black;
            ClrPcker_Fill.SelectedColor = Colors.Black;
        }


        /*
        FUNCTION: DrawingArea_MouseDown
        DESCRIPTION:
	        This method is used to determine where the mouse is clicked on the canvas.
            It also determines the start point for any shape the user has chosen.
        PARAMETERS:
	        object sender - the sender of the event
            MouseButtonEventArgs - relative mouse event args based on sender
        RETURNS:
	        None.
        */
        private void DrawingArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DrawingArea.CaptureMouse())
            {
                startPoint = e.GetPosition(DrawingArea);
                switch (activeShapeType)
                {
                    case ShapeType.line:
                        newShape = true;

                        var line = new Line
                        {
                            Stroke = strokeColor,
                            StrokeThickness = activeStrokeThickness,
                            X1 = startPoint.X,
                            Y1 = startPoint.Y,
                            X2 = startPoint.X,
                            Y2 = startPoint.Y,
                            StrokeDashArray = new DoubleCollection { 4 },
                        };
                        DrawingArea.Children.Add(line);
                        break;
                    case ShapeType.rectangle:
                        newShape = true;
                        var rectangle = new Rectangle
                        {
                            Stroke = strokeColor,
                            StrokeThickness = activeStrokeThickness,
                            Height = 0,
                            Width = 0,
                            StrokeDashArray = new DoubleCollection { 4 },
                            
                        };
                        DrawingArea.Children.Add(rectangle);

                        break;
                    case ShapeType.ellipse:
                        newShape = true;
                        var ellipse = new Ellipse
                        {
                            Stroke = strokeColor,
                            StrokeThickness = activeStrokeThickness,
                            Height = 0,
                            Width = 0,
                            StrokeDashArray = new DoubleCollection { 4 },
                            

                        };
                        DrawingArea.Children.Add(ellipse);
                        break;

                }

                if (this.Title[0] != '*')
                {
                    this.Title = this.Title.Insert(0, "*");
                }
            }
        }



        /*
        FUNCTION: DrawingArea_MouseMove
        DESCRIPTION:
	        This method is used to determine where the mouse is held down on the canvas.
            It also determines the rubber band for the shape the user has chosen.
        PARAMETERS:
	        object sender - the sender of the event
            System.Windows.Input.MouseEventArgs - relative mouse event args based on sender
        RETURNS:
	        None.
        */
        private void DrawingArea_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (DrawingArea.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed && newShape)
            {
                var endPoint = e.GetPosition(DrawingArea);
                mouseStatus.Content = "X: " + endPoint.X + ", Y: " + endPoint.Y;
                switch(activeShapeType)
                {
                    case ShapeType.line:
                        var line = DrawingArea.Children.OfType<Line>().LastOrDefault();

                        if (line != null)
                        {
                            line.X2 = endPoint.X;
                            line.Y2 = endPoint.Y;
                        }
                        break;
                    case ShapeType.rectangle:
                        var rectangle = DrawingArea.Children.OfType<Rectangle>().LastOrDefault();

                        if(rectangle != null)
                        {

                            var x = Math.Min(endPoint.X, startPoint.X);
                            var y = Math.Min(endPoint.Y, startPoint.Y);

                            var w = Math.Max(endPoint.X, startPoint.X) - x;
                            var h = Math.Max(endPoint.Y, startPoint.Y) - y;

                            rectangle.Width = w;
                            rectangle.Height = h;

                            Canvas.SetLeft(rectangle, x);
                            Canvas.SetTop(rectangle, y);
                        }

                        break;
                    case ShapeType.ellipse:
                        var ellipse = DrawingArea.Children.OfType<Ellipse>().LastOrDefault();

                        if (ellipse != null)
                        {

                            var x = Math.Min(endPoint.X, startPoint.X);
                            var y = Math.Min(endPoint.Y, startPoint.Y);

                            var w = Math.Max(endPoint.X, startPoint.X) - x;
                            var h = Math.Max(endPoint.Y, startPoint.Y) - y;

                            ellipse.Width = w;
                            ellipse.Height = h;

                            Canvas.SetLeft(ellipse, x);
                            Canvas.SetTop(ellipse, y);
                        }
                        break;
                }
               
            }
        }



        /*
        FUNCTION: DrawingArea_MouseUp
        DESCRIPTION:
	        This method is used to determine where the mouse is let go on the canvas.
            It also determines the end point for the shape the user has chosen.
        PARAMETERS:
	        object sender - the sender of the event
            MouseButtonEventArgs - relative mouse event args based on sender
        RETURNS:
	        None.
        */
        private void DrawingArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DrawingArea.IsMouseCaptured)
            {
                mouseStatus.Content = "";
                switch(activeShapeType)
                {
                    case ShapeType.line:
                        newShape = false;
                        var line = DrawingArea.Children.OfType<Line>().LastOrDefault();
                        if (line != null)
                        {
                            var endPoint = e.GetPosition(DrawingArea);
                            line.X2 = endPoint.X;
                            line.Y2 = endPoint.Y;
                            line.StrokeDashArray = null;
                        }
                        break;
                    case ShapeType.rectangle:
                        newShape = false;
                        var rectangle = DrawingArea.Children.OfType<Rectangle>().LastOrDefault();
                        if(rectangle != null)
                        {
                            rectangle.StrokeDashArray = null;
                            rectangle.Fill = fillColor;
                        }

                        break;
                    case ShapeType.ellipse:
                        newShape = false;
                        var ellipse = DrawingArea.Children.OfType<Ellipse>().LastOrDefault();
                        if (ellipse != null)
                        {
                            ellipse.StrokeDashArray = null;
                            ellipse.Fill = fillColor;
                        }
                        break;
                }
                
            }
            ((Canvas)sender).ReleaseMouseCapture();
        }



        /*
        FUNCTION: line_Checked
        DESCRIPTION:
	        This method is used to set the activeShapeType to line.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void line_Checked(object sender, RoutedEventArgs e)
        {
            activeShapeType = ShapeType.line;
        }



        /*
        FUNCTION: rectangle_Checked
        DESCRIPTION:
	        This method is used to set the activeShapeType to rectangle.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void rectangle_Checked(object sender, RoutedEventArgs e)
        {
            activeShapeType = ShapeType.rectangle;
        }



        /*
        FUNCTION: ellipse_Checked
        DESCRIPTION:
	        This method is used to set the activeShapeType to ellipse.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void ellipse_Checked(object sender, RoutedEventArgs e)
        {
            activeShapeType = ShapeType.ellipse;
        }



        /*
        FUNCTION: small_Checked
        DESCRIPTION:
	        This method is used to set the current stroke thickness to small.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void small_Checked(object sender, RoutedEventArgs e)
        {
            activeStrokeThickness = 1;
        }



        /*
        FUNCTION: medium_Checked
        DESCRIPTION:
	        This method is used to set the current stroke thickness to medium.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void medium_Checked(object sender, RoutedEventArgs e)
        {
            activeStrokeThickness = 3;
        }



        /*
        FUNCTION: large_Checked
        DESCRIPTION:
	        This method is used to set the current stroke thickness to large.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void large_Checked(object sender, RoutedEventArgs e)
        {
            activeStrokeThickness = 6;
        }



        /*
        FUNCTION: ClrPcker_Stroke_SelectedColorChanged
        DESCRIPTION:
	        This method is used to set stroke color to whatever the user selected in the dialog.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void ClrPcker_Stroke_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = new SolidColorBrush(ClrPcker_Stroke.SelectedColor.Value);
        }



        /*
        FUNCTION: ClrPcker_Fill_SelectedColorChanged
        DESCRIPTION:
	        This method is used to set fill color to whatever the user selected in the dialog.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void ClrPcker_Fill_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = new SolidColorBrush(ClrPcker_Fill.SelectedColor.Value);
        }



        /*
        FUNCTION: aboutToolStripMenuItem_Click
        DESCRIPTION:
	        This method is used to open the about box dialog.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void aboutToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }



        /*
        FUNCTION: clearToolStripMenuItem_Click
        DESCRIPTION:
	        This method is used to clear the working canvas.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void clearToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //start message box
            string title = "SETPaint";
            string message = "Are you sure you want to clear the canvas?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result = System.Windows.Forms.MessageBox.Show(message, title, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                DrawingArea.Children.Clear();
                DrawingArea.Background = Brushes.White;
            }
        }



        /*
        FUNCTION: closeToolStripMenuItem_Click
        DESCRIPTION:
	        This method is used to close the application.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void closeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        /*
        FUNCTION: saveAsToolStripMenuItem_Click
        DESCRIPTION:
	        This method is used to save the current canvas to a png specified in a save dialog.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void saveAsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            // set defaults like filters and filename
            saveDialog.Filter = "PNG files (*.png)|*.png";
            saveDialog.FilterIndex = 0;
            saveDialog.RestoreDirectory = true;
            saveDialog.DefaultExt = ".png";

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // open save dialog
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)DrawingArea.RenderSize.Width,
                (int)DrawingArea.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(DrawingArea);

                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, Convert.ToInt32(DrawingArea.ActualWidth), Convert.ToInt32(DrawingArea.ActualHeight)));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                using (var fs = System.IO.File.OpenWrite(saveDialog.FileName))
                {
                    pngEncoder.Save(fs);
                }

                this.Title = System.IO.Path.GetFileName(saveDialog.FileName) + " - SETPaint";
                this.Title = this.Title.Trim('*');
            }
        }



        /*
        FUNCTION: openToolStripMenuItem_Click
        DESCRIPTION:
	        This method is used to open an image into the canvas. It also clears the current canvas.
        PARAMETERS:
	        N/A
        RETURNS:
	        None.
        */
        private void openToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DrawingArea.Children.Clear();

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "PNG files (*.png)|*.png";
            openDialog.FilterIndex = 0;
            openDialog.RestoreDirectory = true;
            openDialog.DefaultExt = ".png";

            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(openDialog.FileName, UriKind.Relative));
                brush.Stretch = Stretch.Fill;
                DrawingArea.Background = brush;
            }
        }
    }
}
