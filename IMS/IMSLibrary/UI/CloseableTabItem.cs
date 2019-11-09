using System.Windows.Controls;

namespace IMSLibrary.UI
{
    public class CloseableTabItem : TabItem
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Header == null) this.Header = string.Empty;
            string HeaderName = this.Header.ToString();
            if (ToolTip == null && !(string.IsNullOrEmpty(HeaderName))) this.ToolTip = HeaderName;
            StackPanel stck = new StackPanel();
            stck.Width = 80.00;
            stck.Height = 22.00;
            TextBlock tb = new TextBlock();
            tb.Width = 60.00;
            tb.Height = 20.00;
            tb.Text = HeaderName;
            Button btn = new Button();
            btn.Width = 20.00;
            btn.Height = 20.00;
            btn.Content = "X";
            btn.ToolTip = "Close";
            stck.Orientation = Orientation.Horizontal;
            stck.Children.Add(tb);
            stck.Children.Add(btn);
            btn.Click += btn_Click;
            this.Header = stck;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "<Pending>")]
        private void btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this != null)
            {
                TabControl tabCtrl = this.Parent as TabControl;
                if (tabCtrl != null) tabCtrl.Items.Remove(this);
            }
        }
    }
}