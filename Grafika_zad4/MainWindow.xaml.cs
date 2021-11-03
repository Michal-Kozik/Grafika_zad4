using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        // B
                        int sum = 0;
                        sum += pixelBuffer[i];
                        sum += pixelBuffer[i + 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + sourceBitmapData.Stride + 4];
                        pixelBuffer[i] = Convert.ToByte(sum / 4);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1];
                        sum += pixelBuffer[i + 1 + 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 1] = Convert.ToByte(sum / 4);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2];
                        sum += pixelBuffer[i + 2 + 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 2] = Convert.ToByte(sum / 4);
                        // B
                        sum = 0;
                        sum += pixelBuffer[i - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        // B
                        int sum = 0;
                        sum += pixelBuffer[i - 4];
                        sum += pixelBuffer[i];
                        sum += pixelBuffer[i + 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + sourceBitmapData.Stride + 4];
                        pixelBuffer[i] = Convert.ToByte(sum / 6);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - 4];
                        sum += pixelBuffer[i + 1];
                        sum += pixelBuffer[i + 1 + 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 1] = Convert.ToByte(sum / 6);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - 4];
                        sum += pixelBuffer[i + 2];
                        sum += pixelBuffer[i + 2 + 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 2] = Convert.ToByte(sum / 6);
                    }
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        // B
                        int sum = 0;
                        sum += pixelBuffer[i - sourceBitmapData.Stride];
                        sum += pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i];
                        sum += pixelBuffer[i + 4];
                        pixelBuffer[i] = Convert.ToByte(sum / 4);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 1];
                        sum += pixelBuffer[i + 1 + 4];
                        pixelBuffer[i + 1] = Convert.ToByte(sum / 4);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 2];
                        sum += pixelBuffer[i + 2 + 4];
                        pixelBuffer[i + 2] = Convert.ToByte(sum / 4);
                        // B
                        sum = 0;
                        sum += pixelBuffer[i - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i - 4];
                        sum += pixelBuffer[i];
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4];
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4];
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 4);
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        // B
                        int sum = 0;
                        sum += pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i - sourceBitmapData.Stride];
                        sum += pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i - 4];
                        sum += pixelBuffer[i];
                        sum += pixelBuffer[i + 4];
                        pixelBuffer[i] = Convert.ToByte(sum / 6);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 1 - 4];
                        sum += pixelBuffer[i + 1];
                        sum += pixelBuffer[i + 1 + 4];
                        pixelBuffer[i + 1] = Convert.ToByte(sum / 6);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 2 - 4];
                        sum += pixelBuffer[i + 2];
                        sum += pixelBuffer[i + 2 + 4];
                        pixelBuffer[i + 2] = Convert.ToByte(sum / 6);
                    }
                }
                // Pierwsza kolumna.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    // B
                    int sum = 0;
                    sum += pixelBuffer[i - sourceBitmapData.Stride];
                    sum += pixelBuffer[i - sourceBitmapData.Stride + 4];
                    sum += pixelBuffer[i];
                    sum += pixelBuffer[i + 4];
                    sum += pixelBuffer[i + sourceBitmapData.Stride];
                    sum += pixelBuffer[i + sourceBitmapData.Stride + 4];
                    pixelBuffer[i] = Convert.ToByte(sum / 6);
                    // G
                    sum = 0;
                    sum += pixelBuffer[i + 1 - sourceBitmapData.Stride];
                    sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + 4];
                    sum += pixelBuffer[i + 1];
                    sum += pixelBuffer[i + 1 + 4];
                    sum += pixelBuffer[i + 1 + sourceBitmapData.Stride];
                    sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + 4];
                    pixelBuffer[i + 1] = Convert.ToByte(sum / 6);
                    // R
                    sum = 0;
                    sum += pixelBuffer[i + 2 - sourceBitmapData.Stride];
                    sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + 4];
                    sum += pixelBuffer[i + 2];
                    sum += pixelBuffer[i + 2 + 4];
                    sum += pixelBuffer[i + 2 + sourceBitmapData.Stride];
                    sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + 4];
                    pixelBuffer[i + 2] = Convert.ToByte(sum / 6);
                    // B
                    sum = 0;
                    sum += pixelBuffer[i - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 6);
                    // G
                    sum = 0;
                    sum += pixelBuffer[i + 1 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 6);
                    // R
                    sum = 0;
                    sum += pixelBuffer[i + 2 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4];
                    sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4];
                    pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(sum / 6);
                }
                // Ostatnia kolumna.
                else if ((i - 4) % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        // B
                        int sum = 0;
                        sum += pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i - sourceBitmapData.Stride];
                        sum += pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i - 4];
                        sum += pixelBuffer[i];
                        sum += pixelBuffer[i + 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + sourceBitmapData.Stride + 4];
                        pixelBuffer[i] = Convert.ToByte(sum / 9);
                        // G
                        sum = 0;
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 1 - 4];
                        sum += pixelBuffer[i + 1];
                        sum += pixelBuffer[i + 1 + 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 1 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 1] = Convert.ToByte(sum / 9);
                        // R
                        sum = 0;
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 - sourceBitmapData.Stride + 4];
                        sum += pixelBuffer[i + 2 - 4];
                        sum += pixelBuffer[i + 2];
                        sum += pixelBuffer[i + 2 + 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride - 4];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride];
                        sum += pixelBuffer[i + 2 + sourceBitmapData.Stride + 4];
                        pixelBuffer[i + 2] = Convert.ToByte(sum / 9);
                    }
                    catch {}
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

        private void MedianFilter(object sender, RoutedEventArgs e)
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
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        // B
                        List<byte> list = new List<byte>();
                        list.Add(pixelBuffer[i]);
                        list.Add(pixelBuffer[i + 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i] = Convert.ToByte(list.ToArray()[2]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1]);
                        list.Add(pixelBuffer[i + 1 + 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[2]);
                        // R
                        list = new List<byte>(); ;
                        list.Add(pixelBuffer[i + 2]);
                        list.Add(pixelBuffer[i + 2 + 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[2]);
                        // B
                        list = new List<byte>();
                        list.Add(pixelBuffer[i - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Sort();
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Sort();
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Sort();
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        // B
                        List<byte> list = new List<byte>();
                        list.Add(pixelBuffer[i - 4]);
                        list.Add(pixelBuffer[i]);
                        list.Add(pixelBuffer[i + 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i] = Convert.ToByte(list.ToArray()[3]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - 4]);
                        list.Add(pixelBuffer[i + 1]);
                        list.Add(pixelBuffer[i + 1 + 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[3]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - 4]);
                        list.Add(pixelBuffer[i + 2]);
                        list.Add(pixelBuffer[i + 2 + 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[3]);
                    }
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    // Pierwsza kolumna i ostatnia.
                    if (i % sourceBitmapData.Stride == 0)
                    {
                        // B
                        List<byte> list = new List<byte>();
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i]);
                        list.Add(pixelBuffer[i + 4]);
                        list.Sort();
                        pixelBuffer[i] = Convert.ToByte(list.ToArray()[2]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 1]);
                        list.Add(pixelBuffer[i + 1 + 4]);
                        list.Sort();
                        pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[2]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 2]);
                        list.Add(pixelBuffer[i + 2 + 4]);
                        list.Sort();
                        pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[2]);
                        // B
                        list = new List<byte>();
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i - 4]);
                        list.Add(pixelBuffer[i]);
                        list.Sort();
                        pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4]);
                        list.Sort();
                        pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4]);
                        list.Sort();
                        pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[2]);
                    }
                    // Kazda inna kolumna.
                    else
                    {
                        // B
                        List<byte> list = new List<byte>();
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i - 4]);
                        list.Add(pixelBuffer[i]);
                        list.Add(pixelBuffer[i + 4]);
                        list.Sort();
                        pixelBuffer[i] = Convert.ToByte(list.ToArray()[3]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 1 - 4]);
                        list.Add(pixelBuffer[i + 1]);
                        list.Add(pixelBuffer[i + 1 + 4]);
                        list.Sort();
                        pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[3]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 2 - 4]);
                        list.Add(pixelBuffer[i + 2]);
                        list.Add(pixelBuffer[i + 2 + 4]);
                        list.Sort();
                        pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[3]);
                    }
                }
                // Pierwsza kolumna.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    // B
                    List<byte> list = new List<byte>();
                    list.Add(pixelBuffer[i - sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i - sourceBitmapData.Stride + 4]);
                    list.Add(pixelBuffer[i]);
                    list.Add(pixelBuffer[i + 4]);
                    list.Add(pixelBuffer[i + sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i + sourceBitmapData.Stride + 4]);
                    list.Sort();
                    pixelBuffer[i] = Convert.ToByte(list.ToArray()[3]);
                    // G
                    list = new List<byte>();
                    list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + 4]);
                    list.Add(pixelBuffer[i + 1]);
                    list.Add(pixelBuffer[i + 1 + 4]);
                    list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + 4]);
                    list.Sort();
                    pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[3]);
                    // R
                    list = new List<byte>();
                    list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + 4]);
                    list.Add(pixelBuffer[i + 2]);
                    list.Add(pixelBuffer[i + 2 + 4]);
                    list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride]);
                    list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + 4]);
                    list.Sort();
                    pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[3]);
                    // B
                    list = new List<byte>();
                    list.Add(pixelBuffer[i - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Sort();
                    pixelBuffer[i + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[3]);
                    // G
                    list = new List<byte>();
                    list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 1 - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Sort();
                    pixelBuffer[i + 1 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[3]);
                    // R
                    list = new List<byte>();
                    list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 2 - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4 + sourceBitmapData.Stride - 4]);
                    list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + sourceBitmapData.Stride - 4]);
                    list.Sort();
                    pixelBuffer[i + 2 + sourceBitmapData.Stride - 4] = Convert.ToByte(list.ToArray()[3]);
                }
                // Ostatnia kolumna.
                else if ((i - 4) % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        // B
                        List<byte> list = new List<byte>();
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i - 4]);
                        list.Add(pixelBuffer[i]);
                        list.Add(pixelBuffer[i + 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i] = Convert.ToByte(list.ToArray()[4]);
                        // G
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 1 - 4]);
                        list.Add(pixelBuffer[i + 1]);
                        list.Add(pixelBuffer[i + 1 + 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 1 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 1] = Convert.ToByte(list.ToArray()[4]);
                        // R
                        list = new List<byte>();
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 - sourceBitmapData.Stride + 4]);
                        list.Add(pixelBuffer[i + 2 - 4]);
                        list.Add(pixelBuffer[i + 2]);
                        list.Add(pixelBuffer[i + 2 + 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride - 4]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride]);
                        list.Add(pixelBuffer[i + 2 + sourceBitmapData.Stride + 4]);
                        list.Sort();
                        pixelBuffer[i + 2] = Convert.ToByte(list.ToArray()[4]);
                    }
                    catch { }
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

        private void SobelFilter(object sender, RoutedEventArgs e)
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
            
            byte[] pixelBufferResult = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                int targetValue = (pixelBuffer[i] + pixelBuffer[i + 1] + pixelBuffer[i + 2]) / 3;
                pixelBuffer[i] = Convert.ToByte(targetValue);
                pixelBuffer[i + 1] = Convert.ToByte(targetValue);
                pixelBuffer[i + 2] = Convert.ToByte(targetValue);
            }

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    continue;
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    continue;
                }
                // Pierwsza kolumna.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                // Ostatnia kolumna.
                else if ((i - 4) % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        // Pozioma maska.
                        int sumX = 0;
                        sumX -= 1 * pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sumX -= 2 * pixelBuffer[i - sourceBitmapData.Stride];
                        sumX -= 1 * pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sumX += 1 * pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sumX += 2 * pixelBuffer[i + sourceBitmapData.Stride];
                        sumX += 1 * pixelBuffer[i + sourceBitmapData.Stride + 4];
                        // Pionowa maska.
                        int sumY = 0;
                        sumY -= 1 * pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sumY += 1 * pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sumY -= 2 * pixelBuffer[i - 4];
                        sumY += 2 * pixelBuffer[i + 4];
                        sumY -= 1 * pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sumY += 1 * pixelBuffer[i + sourceBitmapData.Stride + 4];
                        byte result;
                        if (Math.Abs(sumX) + Math.Abs(sumY) > 255)
                        {
                            result = 255;
                        }
                        else
                        {
                            result = Convert.ToByte(Math.Abs(sumX) + Math.Abs(sumY));
                        }
                        pixelBufferResult[i] = result;
                        pixelBufferResult[i + 1] = result;
                        pixelBufferResult[i + 2] = result;
                        pixelBufferResult[i + 3] = 255;
                    }
                    catch { }
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBufferResult, 0, resultBitmapData.Scan0, pixelBufferResult.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void SharpenFilter(object sender, RoutedEventArgs e)
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

            byte[] pixelBufferResult = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                int targetValue = (pixelBuffer[i] + pixelBuffer[i + 1] + pixelBuffer[i + 2]) / 3;
                pixelBuffer[i] = Convert.ToByte(targetValue);
                pixelBuffer[i + 1] = Convert.ToByte(targetValue);
                pixelBuffer[i + 2] = Convert.ToByte(targetValue);
            }

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    continue;
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    continue;
                }
                // Pierwsza kolumna.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                // Ostatnia kolumna.
                else if ((i - 4) % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        // Maska
                        int sum = 0;
                        sum -= pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sum -= pixelBuffer[i - sourceBitmapData.Stride];
                        sum -= pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sum -= pixelBuffer[i - 4];
                        sum += 9 * pixelBuffer[i];
                        sum -= pixelBuffer[i + 4];
                        sum -= pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sum -= pixelBuffer[i + sourceBitmapData.Stride];
                        sum -= pixelBuffer[i + sourceBitmapData.Stride + 4];
                        if (Math.Abs(sum) > 255)
                        {
                            sum = 255;
                        }
                        pixelBufferResult[i] = Convert.ToByte(Math.Abs(sum));
                        pixelBufferResult[i + 1] = Convert.ToByte(Math.Abs(sum));
                        pixelBufferResult[i + 2] = Convert.ToByte(Math.Abs(sum));
                        pixelBufferResult[i + 3] = 255;
                    }
                    catch { }
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBufferResult, 0, resultBitmapData.Scan0, pixelBufferResult.Length);
            imgResultBitmap.UnlockBits(resultBitmapData);
            imgResult.Source = ConvertBitmapToImageSource(imgResultBitmap);
        }

        private void GaussFilter(object sender, RoutedEventArgs e)
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

            byte[] pixelBufferResult = new byte[sourceBitmapData.Stride * sourceBitmapData.Height];
            Marshal.Copy(sourceBitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            imgSourceBitmap.UnlockBits(sourceBitmapData);

            for (int i = 0; i + 4 < pixelBuffer.Length; i += 4)
            {
                // Pierwszy wiersz.
                if (i <= sourceBitmapData.Stride)
                {
                    continue;
                }
                // Ostatni wiersz.
                else if (i >= pixelBuffer.Length - sourceBitmapData.Stride)
                {
                    continue;
                }
                // Pierwsza kolumna.
                else if (i % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                // Ostatnia kolumna.
                else if ((i - 4) % sourceBitmapData.Stride == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        // Maska
                        // B
                        int sumB = 0;
                        sumB += pixelBuffer[i - sourceBitmapData.Stride - 4];
                        sumB += 2 * pixelBuffer[i - sourceBitmapData.Stride];
                        sumB += pixelBuffer[i - sourceBitmapData.Stride + 4];
                        sumB += 2 * pixelBuffer[i - 4];
                        sumB += 4 * pixelBuffer[i];
                        sumB += 2 * pixelBuffer[i + 4];
                        sumB += pixelBuffer[i + sourceBitmapData.Stride - 4];
                        sumB += 2 * pixelBuffer[i + sourceBitmapData.Stride];
                        sumB += pixelBuffer[i + sourceBitmapData.Stride + 4];
                        // G
                        int sumG = 0;
                        sumG += pixelBuffer[i+1 - sourceBitmapData.Stride - 4];
                        sumG += 2 * pixelBuffer[i+1 - sourceBitmapData.Stride];
                        sumG += pixelBuffer[i+1 - sourceBitmapData.Stride + 4];
                        sumG += 2 * pixelBuffer[i+1 - 4];
                        sumG += 4 * pixelBuffer[i+1];
                        sumG += 2 * pixelBuffer[i+1 + 4];
                        sumG += pixelBuffer[i+1 + sourceBitmapData.Stride - 4];
                        sumG += 2 * pixelBuffer[i+1 + sourceBitmapData.Stride];
                        sumG += pixelBuffer[i+1 + sourceBitmapData.Stride + 4];
                        // R
                        int sumR = 0;
                        sumR += pixelBuffer[i+2 - sourceBitmapData.Stride - 4];
                        sumR += 2 * pixelBuffer[i+2 - sourceBitmapData.Stride];
                        sumR += pixelBuffer[i+2 - sourceBitmapData.Stride + 4];
                        sumR += 2 * pixelBuffer[i+2 - 4];
                        sumR += 4 * pixelBuffer[i+2];
                        sumR += 2 * pixelBuffer[i+2 + 4];
                        sumR += pixelBuffer[i+2 + sourceBitmapData.Stride - 4];
                        sumR += 2 * pixelBuffer[i+2 + sourceBitmapData.Stride];
                        sumR += pixelBuffer[i+2 + sourceBitmapData.Stride + 4];

                        pixelBufferResult[i] = Convert.ToByte(Math.Floor(sumB / 16.0));
                        pixelBufferResult[i + 1] = Convert.ToByte(Math.Floor(sumG / 16.0));
                        pixelBufferResult[i + 2] = Convert.ToByte(Math.Floor(sumR / 16.0));
                        pixelBufferResult[i + 3] = 255;
                    }
                    catch { }
                }
            }

            // Rezultat.
            Bitmap imgResultBitmap = new Bitmap(imgSourceBitmap.Width, imgSourceBitmap.Height);
            BitmapData resultBitmapData = imgResultBitmap.LockBits(new Rectangle(0, 0, imgResultBitmap.Width, imgResultBitmap.Height),
                                                                   ImageLockMode.WriteOnly,
                                                                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBufferResult, 0, resultBitmapData.Scan0, pixelBufferResult.Length);
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
