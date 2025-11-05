using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Drawing
{
    public partial class MainWindow : Window
    {
        private string currentShape = "Line";
        private Shape tempShape;
        private Point startPoint;
        private Polyline currentPolyline;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ======== 按鈕事件 ========

        private void Line_Click(object sender, RoutedEventArgs e) => currentShape = "Line";
        private void Rectangle_Click(object sender, RoutedEventArgs e) => currentShape = "Rectangle";
        private void Ellipse_Click(object sender, RoutedEventArgs e) => currentShape = "Ellipse";
        private void Polyline_Click(object sender, RoutedEventArgs e) => currentShape = "Polyline";

        // ======== Canvas 繪圖事件 ========

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(drawCanvas);

            switch (currentShape)
            {
                case "Line":
                    tempShape = new Line
                    {
                        Stroke = new SolidColorBrush(StrokePicker.SelectedColor ?? Colors.Black),
                        StrokeThickness = 2,
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y
                    };
                    break;

                case "Rectangle":
                    tempShape = new Rectangle
                    {
                        Stroke = new SolidColorBrush(StrokePicker.SelectedColor ?? Colors.Black),
                        Fill = new SolidColorBrush(FillPicker.SelectedColor ?? Colors.Transparent),
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(tempShape, startPoint.X);
                    Canvas.SetTop(tempShape, startPoint.Y);
                    break;

                case "Ellipse":
                    tempShape = new Ellipse
                    {
                        Stroke = new SolidColorBrush(StrokePicker.SelectedColor ?? Colors.Black),
                        Fill = new SolidColorBrush(FillPicker.SelectedColor ?? Colors.Transparent),
                        StrokeThickness = 2
                    };
                    Canvas.SetLeft(tempShape, startPoint.X);
                    Canvas.SetTop(tempShape, startPoint.Y);
                    break;

                case "Polyline":
                    if (currentPolyline == null)
                    {
                        currentPolyline = new Polyline
                        {
                            Stroke = new SolidColorBrush(StrokePicker.SelectedColor ?? Colors.Black),
                            StrokeThickness = 2
                        };
                        drawCanvas.Children.Add(currentPolyline);
                    }
                    currentPolyline.Points.Add(startPoint);
                    return;
            }

            drawCanvas.Children.Add(tempShape);
            drawCanvas.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (tempShape == null || e.LeftButton != MouseButtonState.Pressed) return;

            Point pos = e.GetPosition(drawCanvas);

            switch (tempShape)
            {
                case Line line:
                    line.X2 = pos.X;
                    line.Y2 = pos.Y;
                    break;

                case Rectangle rect:
                    double w = Math.Abs(pos.X - startPoint.X);
                    double h = Math.Abs(pos.Y - startPoint.Y);
                    Canvas.SetLeft(rect, Math.Min(pos.X, startPoint.X));
                    Canvas.SetTop(rect, Math.Min(pos.Y, startPoint.Y));
                    rect.Width = w;
                    rect.Height = h;
                    break;

                case Ellipse ell:
                    double ew = Math.Abs(pos.X - startPoint.X);
                    double eh = Math.Abs(pos.Y - startPoint.Y);
                    Canvas.SetLeft(ell, Math.Min(pos.X, startPoint.X));
                    Canvas.SetTop(ell, Math.Min(pos.Y, startPoint.Y));
                    ell.Width = ew;
                    ell.Height = eh;
                    break;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            tempShape = null;
            drawCanvas.ReleaseMouseCapture();
            currentPolyline = null;
        }

        // ======== 工具按鈕 ========

        // 橡皮擦：刪除最後一個物件
        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            if (drawCanvas.Children.Count > 0)
                drawCanvas.Children.RemoveAt(drawCanvas.Children.Count - 1);
        }

        // 清除畫布
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            drawCanvas.Children.Clear();
        }

        // ======== 儲存功能 ========

        // 儲存 PNG 圖檔
        private void SavePng_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "PNG Image|*.png" };
            if (dlg.ShowDialog() == true)
            {
                var rect = new Rect(drawCanvas.RenderSize);
                var rtb = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
                rtb.Render(drawCanvas);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using (var fs = File.OpenWrite(dlg.FileName))  // ✅ 改舊語法
                {
                    encoder.Save(fs);
                }
            }
        }

        // 儲存 XAML
        private void SaveXaml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Canvas XAML|*.xaml" };
            if (dlg.ShowDialog() == true)
            {
                string xaml = XamlWriter.Save(drawCanvas);
                File.WriteAllText(dlg.FileName, xaml);
            }
        }

        // 讀取 XAML
        private void LoadXaml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Canvas XAML|*.xaml" };
            if (dlg.ShowDialog() == true)
            {
                string xaml = File.ReadAllText(dlg.FileName);
                var loadedCanvas = (Canvas)XamlReader.Parse(xaml);
                drawCanvas.Children.Clear();

                foreach (var child in loadedCanvas.Children)
                    drawCanvas.Children.Add((UIElement)child);
            }
        }
    }
}
