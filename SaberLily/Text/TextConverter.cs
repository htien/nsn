using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SaberLily.Text
{
    public class TextConverter
    {
        public static string ConvertVietnameseToUnsign(string strIn)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = strIn.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
