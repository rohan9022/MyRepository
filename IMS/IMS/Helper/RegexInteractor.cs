using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IMS.Helper
{
    public class RegexInteractor : DependencyObject
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for RegexEntry.  This enables animation, styling, binding, etc…
        /// </summary>
        public static readonly DependencyProperty RegexEntryProperty =
            DependencyProperty.RegisterAttached("RegexEntry", typeof(bool), typeof(RegexInteractor), new UIPropertyMetadata(false, OnRegexEntryChanged));
        /// <summary>
        /// Runs test for get Regex.
        /// </summary>
        /// <param name=”obj”>
        /// The obj parameter.
        /// </param>
        /// <returns>
        /// The get Regex entry.
        /// </returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [Description("Enable or disable Regex checking for this control.")]
        [DisplayNameAttribute(@"Is Regex Enabled")]
        [CategoryAttribute("Interactor")]
        public static bool GetRegexEntry(DependencyObject obj)
        {
            return (bool)obj.GetValue(RegexEntryProperty);
        }

        /// <summary>
        /// Runs test for set Regex.
        /// </summary>
        /// <param name=”obj”>
        /// The obj parameter.
        /// </param>
        /// <param name=”value”>
        /// If set to <c>true</c> if value.
        /// </param>
        public static void SetRegexEntry(DependencyObject obj, bool value)
        {
            obj.SetValue(RegexEntryProperty, value);
        }

        /// <summary>
        /// Runs test for on Regex entry.
        /// </summary>
        /// <param name=”regexEntry”>
        /// The RegexEntry parameter.
        /// </param>
        /// <param name=”e”>
        /// The <see cref=”System.Windows.DependencyPropertyChangedEventArgs”/> instance containing the event data.
        /// </param>
        private static void OnRegexEntryChanged(DependencyObject regexEntry, DependencyPropertyChangedEventArgs e)
        {
            if (!(regexEntry is TextBox))
            {
                return; // support only Regex entry in a TextBox
            }

            var textBox = regexEntry as TextBox;

            // add the event handles for key press
            if ((bool)e.NewValue)
            {
                textBox.PreviewTextInput += TextBoxPreviewTextInput;
            }
            else
            {
                textBox.PreviewTextInput -= TextBoxPreviewTextInput;
            }
        }

        /// <summary>
        /// The validation Regex.
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty = DependencyProperty
            .RegisterAttached(
                "Expression",
                typeof(string),
                typeof(RegexInteractor),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Sets the expression.
        /// </summary>
        /// <param name=”obj”>The obj parameter.</param>
        /// <param name=”value”>The value parameter.</param>
        public static void SetExpression(DependencyObject obj, string value)
        {
            obj.SetValue(ExpressionProperty, value);
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name=”obj”>The obj parameter.</param>
        /// <returns>
        /// Returns a value of <see cref=”System.String”/>.
        /// </returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [DisplayNameAttribute(@"Regex expression")]
        [Description("The Regex expression to use to validate the textbox entry.")]
        [CategoryAttribute("Interactor")]
        public static string GetExpression(DependencyObject obj)
        {
            return (string)obj.GetValue(ExpressionProperty);
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the textBox control.
        /// </summary>
        /// <param name=”sender”>The source of the event.</param>
        /// <param name=”e”>The <see cref=”System.Windows.Input.TextCompositionEventArgs”/> instance containing the event data.</param>
        private static void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (! (sender is TextBox))
            {
                return;
            }

            var tb = sender as TextBox;
            var text = (sender as TextBox).Text + e.Text;
            var prevInfo = tb.GetValue(ExpressionProperty) as string;
            if (prevInfo == null)
            {
                return;
            }

            var regex = new Regex(prevInfo);
            e.Handled = !regex.IsMatch(text);
        }
    }
}
