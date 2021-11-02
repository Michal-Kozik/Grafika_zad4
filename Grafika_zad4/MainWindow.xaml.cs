using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Grafika_zad4
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

        private void OpenFileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imgSource.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private bool ValidateInputs()
        {
            if (String.IsNullOrEmpty(inputR.Text) || String.IsNullOrEmpty(inputG.Text) || String.IsNullOrEmpty(inputB.Text))
                return false;
            if (!Int32.TryParse(inputR.Text, out int resultR))
                return false;
            if (!Int32.TryParse(inputG.Text, out int resultG))
                return false;
            if (!Int32.TryParse(inputB.Text, out int resultB))
                return false;
            if (resultR < 0 || resultG < 0 || resultB < 0 || resultR > 255 || resultG > 255 || resultB > 255)
                return false;
            return true;
        }

        private bool ValidateInputsDouble()
        {
            if (String.IsNullOrEmpty(inputR.Text) || String.IsNullOrEmpty(inputG.Text) || String.IsNullOrEmpty(inputB.Text))
                return false;
            if (!Double.TryParse(inputR.Text, out double resultR))
                return false;
            if (!Double.TryParse(inputR.Text, out double resultG))
                return false;
            if (!Double.TryParse(inputR.Text, out double resultB))
                return false;
            if (resultR < 0 || resultG < 0 || resultB < 0 || resultR > 255 || resultG > 255 || resultB > 255)
                return false;
            if (inputR.Text.Length > 4 || inputG.Text.Length > 4 || inputB.Text.Length > 4)
                return false;
            return true;
        }

        private bool DivideByZero()
        {
            double resultR = Double.Parse(inputR.Text);
            double resultG = Double.Parse(inputG.Text);
            double resultB = Double.Parse(inputB.Text);
            if (resultR == 0 || resultG == 0 || resultB == 0)
                return true;
            return false;
        }

        private bool ImageExist()
        {
            return imgSource.Source == null ? false : true;
        }

        private void Addition(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidateInputs())
            {
                MessageBox.Show("Niepoprawne dane! Zwróć uwagę, czy podano liczby naturalne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height), 
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            byte inputRed = Convert.ToByte(inputR.Text);
            byte inputGreen = Convert.ToByte(inputG.Text);
            byte inputBlue = Convert.ToByte(inputB.Text);
            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                if (pixelBuffer[i] + inputRed > 255)
                {
                    pixelBuffer[i] = 255;
                } 
                else
                {
                    pixelBuffer[i] += inputRed;
                }
                // G
                if (pixelBuffer[i+1] + inputGreen > 255)
                {
                    pixelBuffer[i+1] = 255;
                }
                else
                {
                    pixelBuffer[i+1] += inputGreen;
                }
                // B
                if (pixelBuffer[i+2] + inputBlue > 255)
                {
                    pixelBuffer[i+2] = 255;
                }
                else
                {
                    pixelBuffer[i+2] += inputBlue;
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void Subtraction(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidateInputs())
            {
                MessageBox.Show("Niepoprawne dane! Zwróć uwagę, czy podano liczby naturalne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            byte inputRed = Convert.ToByte(inputR.Text);
            byte inputGreen = Convert.ToByte(inputG.Text);
            byte inputBlue = Convert.ToByte(inputB.Text);
            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                if (pixelBuffer[i] - inputRed < 0)
                {
                    pixelBuffer[i] = 0;
                }
                else
                {
                    pixelBuffer[i] -= inputRed;
                }
                // G
                if (pixelBuffer[i + 1] - inputGreen > 255)
                {
                    pixelBuffer[i + 1] = 255;
                }
                else
                {
                    pixelBuffer[i + 1] -= inputGreen;
                }
                // B
                if (pixelBuffer[i + 2] - inputBlue > 255)
                {
                    pixelBuffer[i + 2] = 255;
                }
                else
                {
                    pixelBuffer[i + 2] -= inputBlue;
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void Multiplication(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidateInputsDouble())
            {
                MessageBox.Show("Niepoprawne dane! Zwróć uwagę, czy nie podano więcej niż 2 cyfry po przecinku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            double inputRed = Double.Parse(inputR.Text);
            double inputGreen = Double.Parse(inputG.Text);
            double inputBlue = Double.Parse(inputB.Text);
            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                if (pixelBuffer[i] * inputRed > 255)
                {
                    pixelBuffer[i] = 255;
                }
                else
                {
                    pixelBuffer[i] = Convert.ToByte(Math.Round(pixelBuffer[i] * inputRed, 0));
                }
                // G
                if (pixelBuffer[i + 1] * inputGreen > 255)
                {
                    pixelBuffer[i + 1] = 255;
                }
                else
                {
                    pixelBuffer[i + 1] = Convert.ToByte(Math.Round(pixelBuffer[i+1] * inputGreen, 0));
                }
                // B
                if (pixelBuffer[i + 2] * inputBlue > 255)
                {
                    pixelBuffer[i + 2] = 255;
                }
                else
                {
                    pixelBuffer[i + 2] = Convert.ToByte(Math.Round(pixelBuffer[i+2] * inputBlue, 0));
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void Division(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidateInputsDouble())
            {
                MessageBox.Show("Niepoprawne dane! Zwróć uwagę, czy nie podano więcej niż 2 cyfry po przecinku.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DivideByZero())
            {
                MessageBox.Show("Nie dziel przez 0!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            double inputRed = Double.Parse(inputR.Text);
            double inputGreen = Double.Parse(inputG.Text);
            double inputBlue = Double.Parse(inputB.Text);
            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                if (pixelBuffer[i] / inputRed > 255)
                {
                    pixelBuffer[i] = 255;
                }
                else
                {
                    pixelBuffer[i] = Convert.ToByte(Math.Round(pixelBuffer[i] / inputRed, 0));
                }
                // G
                if (pixelBuffer[i + 1] / inputGreen > 255)
                {
                    pixelBuffer[i + 1] = 255;
                }
                else
                {
                    pixelBuffer[i+1] = Convert.ToByte(Math.Round(pixelBuffer[i+1] / inputGreen, 0));
                }
                // B
                if (pixelBuffer[i + 2] / inputBlue > 255)
                {
                    pixelBuffer[i + 2] = 255;
                }
                else
                {
                    pixelBuffer[i+2] = Convert.ToByte(Math.Round(pixelBuffer[i+2] / inputBlue, 0));
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void ChangeBrightness(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                if (sliderBrightness.Value < 0)
                {
                    int absoluteValue = Math.Abs((int)sliderBrightness.Value);
                    if (pixelBuffer[i] - absoluteValue < 0)
                    {
                        pixelBuffer[i] = 0;
                    }
                    else
                    {
                        pixelBuffer[i] -= Convert.ToByte(absoluteValue);
                    }
                }
                else
                {
                    if (pixelBuffer[i] + sliderBrightness.Value > 255)
                    {
                        pixelBuffer[i] = 255;
                    }
                    else
                    {
                        pixelBuffer[i] += Convert.ToByte(sliderBrightness.Value);
                    }
                }
                // G
                if (sliderBrightness.Value < 0)
                {
                    int absoluteValue = Math.Abs((int)sliderBrightness.Value);
                    if (pixelBuffer[i+1] - absoluteValue < 0)
                    {
                        pixelBuffer[i+1] = 0;
                    }
                    else
                    {
                        pixelBuffer[i+1] -= Convert.ToByte(absoluteValue);
                    }
                }
                else
                {
                    if (pixelBuffer[i+1] + sliderBrightness.Value > 255)
                    {
                        pixelBuffer[i+1] = 255;
                    }
                    else
                    {
                        pixelBuffer[i+1] += Convert.ToByte(sliderBrightness.Value);
                    }
                }
                // B
                if (sliderBrightness.Value < 0)
                {
                    int absoluteValue = Math.Abs((int)sliderBrightness.Value);
                    if (pixelBuffer[i+2] - absoluteValue < 0)
                    {
                        pixelBuffer[i+2] = 0;
                    }
                    else
                    {
                        pixelBuffer[i+2] -= Convert.ToByte(absoluteValue);
                    }
                }
                else
                {
                    if (pixelBuffer[i+2] + sliderBrightness.Value > 255)
                    {
                        pixelBuffer[i+2] = 255;
                    }
                    else
                    {
                        pixelBuffer[i+2] += Convert.ToByte(sliderBrightness.Value);
                    }
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void ClearColorsAverage(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                int targetValue = (pixelBuffer[i] + pixelBuffer[i+1] + pixelBuffer[i+2]) / 3;
                // R
                pixelBuffer[i] = Convert.ToByte(targetValue);
                // G
                pixelBuffer[i+1] = Convert.ToByte(targetValue);
                // B
                pixelBuffer[i+2] = Convert.ToByte(targetValue);
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void ClearColorsYUV(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // R
                pixelBuffer[i] = Convert.ToByte(0.299 * pixelBuffer[i] + 0.587 * pixelBuffer[i+1] + 0.114 * pixelBuffer[i+2]);
                // G
                pixelBuffer[i + 1] = Convert.ToByte(0.299 * pixelBuffer[i] + 0.587 * pixelBuffer[i+1] + 0.114 * pixelBuffer[i+2]);
                // B
                pixelBuffer[i + 2] = Convert.ToByte(0.299 * pixelBuffer[i] + 0.587 * pixelBuffer[i+1] + 0.114 * pixelBuffer[i+2]);
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void SmoothFilter(object sender, RoutedEventArgs e)
        {
            if (!ImageExist())
            {
                MessageBox.Show("Nie załadowano obrazka!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zrodlo.
            Bitmap imgSourceBitmap = ConvertImgToBitmap(imgSource);
            BitmapData sourceBitmapData = imgSourceBitmap.LockBits(new Rectangle(0, 0, imgSourceBitmap.Width, imgSourceBitmap.Height),
                                                             ImageLockMode.ReadOnly,
                                                             System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            //byte[,] bufferR = new byte[imgSourceBitmap.Height, imgSourceBitmap.Width];
            //byte[,] bufferG = new byte[imgSourceBitmap.Height, imgSourceBitmap.Width];
            //byte[,] bufferB = new byte[imgSourceBitmap.Height, imgSourceBitmap.Width];
            //int i = 0;
            //for (int y = 0; y < imgSourceBitmap.Height; y++)
            //{
            //    for (int x = 0; x < imgSourceBitmap.Width; x++)
            //    {
            //        bufferR[y, x] = pixelBuffer[i];
            //        bufferG[y, x] = pixelBuffer[i+1];
            //        bufferB[y, x] = pixelBuffer[i+2];
            //        i += 4;
            //    }
            //}

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        pixelBuffer[i] = 255;
                        pixelBuffer[i + 1] = 0;
                        pixelBuffer[i + 2] = 0;
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = 255;
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = 0;
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = 0;
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        pixelBuffer[i] = 0;
                        pixelBuffer[i + 1] = 255;
                        pixelBuffer[i + 2] = 0;
                    }
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        pixelBuffer[i] = 255;
                        pixelBuffer[i + 1] = 0;
                        pixelBuffer[i + 2] = 0;
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = 255;
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = 0;
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = 0;
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        pixelBuffer[i] = 0;
                        pixelBuffer[i + 1] = 255;
                        pixelBuffer[i + 2] = 0;
                    }
                }
                // Pierwsza kolumna i ostatnia.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    pixelBuffer[i] = 0;
                    pixelBuffer[i + 1] = 0;
                    pixelBuffer[i + 2] = 255;
                    pixelBuffer[i + sourceBitmapData.Stride - 4] = 0;
                    pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = 0;
                    pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = 255;
                }

                //// B
                //pixelBuffer[i] = 0;
                //// G
                //pixelBuffer[i + 1] = 0;
                //// R
                //pixelBuffer[i + 2] = 255;
            }


            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultBitmapData.Scan0, pixelBuffer.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private Bitmap ConvertImgToBitmap(System.Windows.Controls.Image source)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)source.ActualWidth, (int)source.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            source.Measure(new System.Windows.Size((int)source.ActualWidth, (int)source.ActualHeight));
            source.Arrange(new Rect(new System.Windows.Size((int)source.ActualWidth, (int)source.ActualHeight)));
            renderTargetBitmap.Render(source);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            encoder.Save(stream);
            Bitmap bitmap = new Bitmap(stream);
            stream.Close();
            renderTargetBitmap.Clear();
            return bitmap;
        }

        private BitmapImage ConvertBitmapToImageSource(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        
    }
}
