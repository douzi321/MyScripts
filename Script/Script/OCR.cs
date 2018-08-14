using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.CvEnum;
using System.IO;
namespace EmguOCR
{
    class OCR
    {
        private static Tesseract _ocr;
        public static Rectangle[] ocr_rect;
        public static string ReadOCR( Image<Bgr, Byte> srcImage,Rectangle rect)
        {
            string str_ocr = "";
            _ocr = new Tesseract("", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
           // _ocr.SetVariable("tessedit_char_whitelist", "0123456789");
            //_ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            //_ocr.SetVariable("tessedit_char_whitelist", "abcdefghigklmnopqrstuvwxyz");
            _ocr.SetVariable("tessedit_char_whitelist", "0123456789abcdefghigklmnopqrstuvwxyz");
            //_ocr.SetVariable("tessedit_char_whitelist", "0123456789abcdefghigklmnopqrstuvwxyz");                   
               
                try
                {                   
                    Image<Gray, Byte> Image_Bin = new Image<Gray, Byte>(new Size(rect.Width, rect.Height));
                   // Image<Gray, Byte> Image_Bin1 = new Image<Gray, Byte>(new Size(rect.Width, rect.Height));
                    CvInvoke.cvSetImageROI(srcImage, rect);
                    using (Image<Gray, byte> gray = srcImage.Convert<Gray, Byte>())
                    {
                        CvInvoke.cvThreshold(gray, Image_Bin, 0, 255, THRESH.CV_THRESH_OTSU);
                        
                       // CvInvoke.cvThreshold(Image_Bin, Image_Bin1, 50, 255, THRESH.CV_THRESH_BINARY_INV);

                        _ocr.Recognize(Image_Bin);
                        Tesseract.Charactor[] charactors = _ocr.GetCharactors();

                        ocr_rect = new Rectangle[charactors.Length];

                        for (int i = 0; i < charactors.Length; i++)
                        {
                            ocr_rect[i] = new Rectangle(charactors[i].Region.X + rect.X, charactors[i].Region.Y + rect.Y, charactors[i].Region.Width, charactors[i].Region.Height);
                        }
                            str_ocr = _ocr.GetText();           
                       
                    }
                    Image_Bin.Dispose();
                   // Image_Bin1.Dispose();
                }
                catch (Exception exception)
                {
                    //MessageBox.Show(exception.Message);
                }
                CvInvoke.cvResetImageROI(srcImage);   
            
                                            
            return str_ocr;
        }

    }
}
