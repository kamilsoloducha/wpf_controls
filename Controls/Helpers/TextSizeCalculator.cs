using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls.Helpers {
  public class TextSizeCalculator {

    public static void Calculate(TextBlock pTextBlock, double pLimit) {
      if (string.IsNullOrEmpty(pTextBlock.Text)) {
        return;
      }
      if (pLimit <= 0) {
        return;
      }
      FormattedText formattedText = new FormattedText(
            pTextBlock.Text,
            CultureInfo.CurrentUICulture,
            pTextBlock.FlowDirection,
            new Typeface(pTextBlock.FontFamily, pTextBlock.FontStyle, pTextBlock.FontWeight, pTextBlock.FontStretch),
            pTextBlock.FontSize,
            pTextBlock.Foreground);
      int newFontSize = (int)pTextBlock.FontSize;
      if (formattedText.Width > pLimit) {
        while (formattedText.Width > pLimit) {
          newFontSize--;
          formattedText.SetFontSize(newFontSize);
        }
      } else {
        while (formattedText.Width < pLimit && formattedText.Height < pLimit) {
          newFontSize++;
          formattedText.SetFontSize(newFontSize);
        }
      }
      pTextBlock.FontSize = newFontSize;
    }

    public static void CalculateNewText(TextBlock pTextBlock, double pLimit, string newText) {
      if (string.IsNullOrEmpty(newText)) {
        return;
      }
      if (pLimit <= 0) {
        return;
      }
      FormattedText formattedText = new FormattedText(
            newText,
            CultureInfo.CurrentUICulture,
            pTextBlock.FlowDirection,
            new Typeface(pTextBlock.FontFamily, pTextBlock.FontStyle, pTextBlock.FontWeight, pTextBlock.FontStretch),
            pTextBlock.FontSize,
            pTextBlock.Foreground);
      int newFontSize = (int)pTextBlock.FontSize;
      if (formattedText.Width > pLimit) {
        while (formattedText.Width > pLimit) {
          newFontSize--;
          formattedText.SetFontSize(newFontSize);
        }
      } else {
        while (formattedText.Width < pLimit && formattedText.Height < pLimit) {
          newFontSize++;
          formattedText.SetFontSize(newFontSize);
        }
      }
      pTextBlock.FontSize = newFontSize;
    }

  }
}
