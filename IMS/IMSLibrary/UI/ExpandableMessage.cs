using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace IMSLibrary.UI
{
    public class ExpandableMessage
    {
        #region FIELDS
        private bool _isExpandable;
        private Grid _outerGrid;
        //private Grid _innerGrid;
        public Grid _innerGrid;
        private System.Windows.Controls.Button _resizeBtn;
        private System.Windows.Controls.Button _closeBtn;
        private TextBlock _headerTextBlock;
        private TextBlock _messageTextBlock;
        private Brush _headerForegroundBrush;
        //private Window _popUpMessage;
        public Window _popUpMessage;
        private double _windowHeight;
        private double _windowMaxHeight;
        private double _windowHeightDifference;
        private double _windowWidth;
        private bool _isExpanded;
        private string _imageContractName;
        private string _imageExpandName;
        private string _imageCloseName;
        private string _mesageText;
        private string _headerText;
        private Thickness _messagePadding;
        private int _messageMinLength;
        private int _messageMaxLength;
        private Brush _backgroundBrush;
        private Brush _borderBrush;
        private Thickness _borderThickness;
        private CornerRadius _borderCornerRadius;
        private static readonly object _lock = new object();
        private static double _messagePlacementHeight;
        private RepositionWindowsService _stack;
        private RepositionWindowsService _resize;
        private DateTime _creationDatetime;
        private double _currentWindowTop;
        private double? _animationStartValue;
        private double? _animationEndValue;
        private TimeSpan _animationDuration;
        private bool _messageWindowRepeatBehaviorForever;
        private bool _headerFlash;
        #endregion FIELDS

        #region EXPANDABLE VERSION
        public ExpandableMessage(string imageCloseName, string imageExpandName, string imageContractName,
           string headerText, Brush headerForegroundBrush, string mesageText, int messageMinLength,
           int messageMaxLength, Brush borderBrush)
            : this(75, 115, 350, imageCloseName, imageExpandName, imageContractName,
            headerText, headerForegroundBrush, mesageText, messageMinLength, messageMaxLength,
            new Thickness(0), Brushes.White, borderBrush, new Thickness(1), new CornerRadius(2),
            0, 50, TimeSpan.FromSeconds(5), false, false)
        { }

        public ExpandableMessage(string imageCloseName, string imageExpandName, string imageContractName,
            string headerText, Brush headerForegroundBrush, string mesageText, int messageMinLength,
            int messageMaxLength, Brush borderBrush, bool messageWindowRepeatBehaviorForever, bool headerFlash)
            : this(75, 115, 350, imageCloseName, imageExpandName, imageContractName,
            headerText, headerForegroundBrush, mesageText, messageMinLength, messageMaxLength,
            new Thickness(0), Brushes.White, borderBrush, new Thickness(1), new CornerRadius(2),
            0, 50, TimeSpan.FromSeconds(5), messageWindowRepeatBehaviorForever, headerFlash)
        { }

        public ExpandableMessage(string imageCloseName, string imageExpandName, string imageContractName,
            string headerText, Brush headerForegroundBrush, string mesageText, int messageMinLength,
            int messageMaxLength, Brush borderBrush, Thickness borderThickness, CornerRadius borderCornerRadius,
            bool messageWindowRepeatBehaviorForever, bool headerFlash)

            : this(75, 115, 350, imageCloseName, imageExpandName, imageContractName,
            headerText, headerForegroundBrush, mesageText, messageMinLength, messageMaxLength,
            new Thickness(0), Brushes.White, borderBrush, borderThickness, borderCornerRadius,
            0, 50, TimeSpan.FromSeconds(5), messageWindowRepeatBehaviorForever, headerFlash)
        { }

        public ExpandableMessage(double windowHeight, double windowMaxHeight, double windowWidth, string imageCloseName, string imageExpandName, string imageContractName, string headerText, Brush headerForegroundBrush, string mesageText, int messageMinLength, int messageMaxLength,
                           Thickness messagePadding, Brush backgroundColour, Brush borderBrush, Thickness borderThickness,
            CornerRadius borderCornerRadius, double? animationStartValue, double? animationEndValue,
            TimeSpan animationDuration, bool messageWindowRepeatBehaviorForever, bool headerFlash)
        {

            //ONLY ONE MESSAGE AT A TIME CAN BE CREATED
            lock (_lock)
            {
                //SET FIELD VALUES

                _isExpandable = !string.IsNullOrEmpty(imageExpandName);

                _creationDatetime = DateTime.Now;
                _windowHeight = windowHeight;
                _windowMaxHeight = windowMaxHeight;
                _windowHeightDifference = windowMaxHeight - _windowHeight;
                _windowWidth = windowWidth;
                _imageCloseName = imageCloseName;
                _imageContractName = imageContractName;
                _imageExpandName = imageExpandName;
                _mesageText = mesageText;
                _headerText = headerText;
                _headerForegroundBrush = headerForegroundBrush;
                _messagePadding = messagePadding;

                if (messageMinLength > mesageText.Length)
                {
                    _messageMinLength = mesageText.Length;
                }
                else
                {
                    _messageMinLength = messageMinLength;
                }

                if (messageMaxLength > mesageText.Length)
                {
                    _messageMaxLength = mesageText.Length;
                }
                else
                {
                    _messageMaxLength = messageMaxLength;
                }
                _backgroundBrush = backgroundColour;
                _borderBrush = borderBrush;
                _borderThickness = borderThickness;
                _borderCornerRadius = borderCornerRadius;
                _animationStartValue = animationStartValue;
                _animationEndValue = animationEndValue;
                _animationDuration = animationDuration;
                _messageWindowRepeatBehaviorForever = messageWindowRepeatBehaviorForever;
                _headerFlash = headerFlash;

                //CONFIGURE THE MESSAGE AND SHOW IT
                PopUpHelper();
                GridHelper();
                ConfigureButtons();
                TextControlHelper();
                Configure(windowWidth);

                //SUBSCRIBE TO THE EVENTS
                _stack = new RepositionWindowsService();
                RepositionWindowsService.WindowLocationChanged += StackWindow;

                if (_isExpandable)
                {
                    _resize = new RepositionWindowsService();
                    RepositionWindowsService.WindowSizeChanged += ResizeWindow;
                }

                //SHOW MESSAGE WINDOW
                _popUpMessage.Show();

            }
        }

        #region METHODS

        public void StackWindow(double currentWindowHeight, DateTime sizeChangedTime)
        {
            if (sizeChangedTime <= _creationDatetime)
            {
                //SUBTRACT THE CURRENT WINDOW HEIGHT             
                _popUpMessage.Top = _popUpMessage.Top + currentWindowHeight;
            }
        }

        public void ResizeWindow(double currentWindowHeight, DateTime sizeChangedTime)
        {
            if (sizeChangedTime <= _creationDatetime)
            {
                //ADD THE CURRENT WINDOW HEIGHT                
                _popUpMessage.Top = _popUpMessage.Top - currentWindowHeight;
            }
        }

        private void PopUpHelper()
        {
            //THIS WILL BE THE MESSAGE WHICH IS DISPLAYED
            _popUpMessage = new Window();
            _popUpMessage.Name = "PopUpMessage";
            _popUpMessage.AllowsTransparency = true;
            _popUpMessage.Background = Brushes.Transparent;
            _popUpMessage.WindowStyle = WindowStyle.None;
            _popUpMessage.ShowInTaskbar = false;
            _popUpMessage.Topmost = true;
            _popUpMessage.Height = _windowHeight;
            _popUpMessage.Width = _windowWidth;
            _popUpMessage.Top = Screen.PrimaryScreen.WorkingArea.Height - (_windowHeight + _messagePlacementHeight);
            _popUpMessage.Left = Screen.PrimaryScreen.WorkingArea.Width - _windowWidth;

            _currentWindowTop = _popUpMessage.Top;

            _popUpMessage.Closed += ((sender, e) =>
            {
                lock (_lock)
                {
                    _messagePlacementHeight -= _popUpMessage.Height;

                    //UNSUBSCRIBE FROM EVENTS,WE ARE DONE WITH THEM.
                    RepositionWindowsService.WindowLocationChanged -= StackWindow;

                    if (_isExpandable)
                    {
                        RepositionWindowsService.WindowSizeChanged -= ResizeWindow;
                    }
                }
                _stack.OnWindowLocationChanged(_popUpMessage.Height, _creationDatetime);
                Console.WriteLine("_popUpMessage.Closed _messagePlacementHeight = " + _messagePlacementHeight.ToString());

            });

            lock (_lock)
            {
                _messagePlacementHeight = _messagePlacementHeight + _windowHeight;
            }

        }

        private System.Windows.Controls.Button ImageButtonHelper(double imageHeight, double imageWidth, string imageName,
           System.Windows.HorizontalAlignment horizontalAlignment, System.Windows.VerticalAlignment verticalAlignment)
        {
            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            button.Height = imageHeight;
            button.Width = imageWidth;
            button.HorizontalAlignment = horizontalAlignment;
            button.VerticalAlignment = verticalAlignment;

            try
            {
                button.Content = ImageHelper(imageName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return button;
        }

        private void TextControlHelper()
        {
            _messageTextBlock = new TextBlock();
            _messageTextBlock.Padding = _messagePadding;
            _messageTextBlock.VerticalAlignment = VerticalAlignment.Center;
            _messageTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _messageTextBlock.Text = _mesageText;
            _messageTextBlock.TextWrapping = TextWrapping.Wrap;
            TextHelper(_messageMinLength);

            _headerTextBlock = new TextBlock();
            _headerTextBlock.Name = "HeaderTextBlock";
            //_headerTextBlock.Width = 150;
            _headerTextBlock.Height = 20;

            //BOLD HEADING
            Bold bold = new Bold();
            bold.Inlines.Add(_headerText);
            _headerTextBlock.Inlines.Add(bold);

            _headerTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            _headerTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _headerTextBlock.SetValue(Grid.ColumnSpanProperty, 3);
            _headerTextBlock.Foreground = _headerForegroundBrush;
            _headerTextBlock.TextEffects.Freeze();
        }

        private void TextHelper(int messageLength)
        {
            //CALLED AS PART OF RESIZING
            if (messageLength > _mesageText.Length)
            {
                _messageTextBlock.Text = _mesageText.Substring(0, _mesageText.Length);
            }
            else
            {
                _messageTextBlock.Text = _mesageText.Substring(0, messageLength);
            }
        }

        private void ConfigureButtons()
        {
            #region CLOSE

            //CLOSE THE MESSAGE
            _closeBtn = ImageButtonHelper(20, 20, _imageCloseName, System.Windows.HorizontalAlignment.Left, VerticalAlignment.Top);
            _closeBtn.Click += ((s, e) =>
            {
                _popUpMessage.Close();
            });

            #endregion CLOSE

            #region RESIZE

            if (_isExpandable)
            {

                //RESIZE IMAGE BUTTON  
                _resizeBtn = ImageButtonHelper(20, 20, _imageExpandName, System.Windows.HorizontalAlignment.Right, VerticalAlignment.Top);

                _resizeBtn.Click += ((s, e) =>
                {
                    var windowHeightDifference = _windowMaxHeight - _windowHeight;

                    //REFRESH WITH NEW HEIGHT
                    if (_isExpanded)
                    {
                        TextHelper(_messageMinLength);
                        _resizeBtn.Content = ImageHelper(_imageExpandName);
                        _windowHeight = _windowHeight - _windowHeightDifference;
                        _popUpMessage.Height = _windowHeight;

                        lock (_lock)
                        {
                            _messagePlacementHeight += -_windowHeightDifference;
                        }
                        _resize.OnWindowSizeChanged(-_windowHeightDifference, _creationDatetime);
                    }
                    else
                    {
                        TextHelper(_messageMaxLength);
                        _resizeBtn.Content = ImageHelper(_imageContractName);
                        _windowHeight = _windowHeight + _windowHeightDifference;
                        _popUpMessage.Height = _windowHeight;
                        lock (_lock)
                        {
                            _messagePlacementHeight += _windowHeightDifference;
                        }
                        _resize.OnWindowSizeChanged(windowHeightDifference, _creationDatetime);
                    }

                    _popUpMessage.Left = Screen.PrimaryScreen.WorkingArea.Width - _windowWidth;
                    _isExpanded = _isExpanded == true ? false : true;
                });
            }

            #endregion RESIZE
        }

        private Image ImageHelper(string imageName)
        {
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {
                bitmapImage.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + imageName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            bitmapImage.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bitmapImage;

            return image;
        }

        private void GridHelper()
        {
            //CREATE OUTER GRID
            _outerGrid = new Grid();
            _outerGrid.Background = _backgroundBrush;

            //ADD BORDER TO GRID
            _outerGrid.Children.Add(BorderHelper());

            //CREATE A INNER GRID WHICH WILL CONTAIN THE ACTUAL MESSAGE
            _innerGrid = new Grid();

        }

        private Border BorderHelper()
        {
            Border border = new Border();
            border.CornerRadius = _borderCornerRadius;
            border.BorderBrush = _borderBrush;
            border.BorderThickness = _borderThickness;
            return border;
        }

        private void Configure(double windowWidth)
        {
            #region ROW AND COLUMN DEFINITIONS

            //CREATE ROW AND TABLE DEFINITIONS
            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();
            RowDefinition rd3 = new RowDefinition();

            rd1.Height = new GridLength(3);     //ACTS AS PADDING
            rd2.Height = GridLength.Auto;       //CLOSE BUTTON IN THIS ROW
            rd3.Height = GridLength.Auto;

            _innerGrid.RowDefinitions.Add(rd1);
            _innerGrid.RowDefinitions.Add(rd2);
            _innerGrid.RowDefinitions.Add(rd3);

            ColumnDefinition cd1 = new ColumnDefinition();
            ColumnDefinition cd2 = new ColumnDefinition();
            ColumnDefinition cd3 = new ColumnDefinition();
            ColumnDefinition cd4 = new ColumnDefinition();
            ColumnDefinition cd5 = new ColumnDefinition();
            ColumnDefinition cd6 = new ColumnDefinition();
            ColumnDefinition cd7 = new ColumnDefinition();

            cd1.Width = new GridLength(3);      //ACTS AS PADDING
            cd2.Width = GridLength.Auto;        //CLOSE BUTTON IN THIS COLUMN   
            cd3.Width = GridLength.Auto;
            cd4.Width = new GridLength(windowWidth - 60);
            cd5.Width = new GridLength(10);
            cd6.Width = GridLength.Auto;        //RESIZE BUTTON IN THE COLUMN
            cd7.Width = new GridLength(10);

            _innerGrid.ColumnDefinitions.Add(cd1);
            _innerGrid.ColumnDefinitions.Add(cd2);
            _innerGrid.ColumnDefinitions.Add(cd3);
            _innerGrid.ColumnDefinitions.Add(cd4);
            _innerGrid.ColumnDefinitions.Add(cd5);
            _innerGrid.ColumnDefinitions.Add(cd6);

            #endregion ROW AND COLUMN DEFINITIONS

            #region ADD CONTROLS

            Grid.SetColumn(_headerTextBlock, 2);
            Grid.SetRow(_headerTextBlock, 1);

            if (_isExpandable)
            {
                Grid.SetColumn(_resizeBtn, 5);
                Grid.SetRow(_resizeBtn, 1);
            }

            Grid.SetColumn(_closeBtn, 1);
            Grid.SetRow(_closeBtn, 1);

            Grid.SetColumn(_messageTextBlock, 3);
            Grid.SetRow(_messageTextBlock, 2);

            _innerGrid.Children.Add(_headerTextBlock);

            _innerGrid.Children.Add(_closeBtn);

            if (_isExpandable)
            {
                _innerGrid.Children.Add(_resizeBtn);
            }
            _innerGrid.Children.Add(_messageTextBlock);

            //NOW ADD THE INNER GRID TO THE OUTER GRID (WHICH CONTAINS THE BORDER)
            _outerGrid.Children.Add(_innerGrid);

            #endregion ADD CONTROLS

            #region POPUP MESSAGE ANIMATION

            //SET THE WINDOWS CONTENT
            _popUpMessage.Content = _outerGrid;

            //REGISTER THE WINDOWS NAME WHICH IS NECESSARY FOR CREATING THE STORYBOARD INSTEAD OF XAML
            NameScope.SetNameScope(_popUpMessage, new NameScope());
            _popUpMessage.RegisterName(_popUpMessage.Name, _popUpMessage);

            //CREATE THE IN AND OUT ANIMATION
            DoubleAnimation windowFadeAnimation = new DoubleAnimation();
            windowFadeAnimation.From = _animationStartValue;
            windowFadeAnimation.To = _animationEndValue;
            windowFadeAnimation.Duration = new Duration(_animationDuration);

            windowFadeAnimation.AutoReverse = true;

            if (_messageWindowRepeatBehaviorForever)
            {
                windowFadeAnimation.RepeatBehavior = RepeatBehavior.Forever;
            }
            else
            {
                windowFadeAnimation.Completed += ((s, e) =>
                {
                    _popUpMessage.Close();
                });
            }

            //CONFIGURE THE ANIMATION TO TARGET THE WINDOWS OPACITY
            Storyboard.SetTargetName(windowFadeAnimation, _popUpMessage.Name);
            Storyboard.SetTargetProperty(windowFadeAnimation, new PropertyPath(Window.OpacityProperty));

            //ADD THE FADE IN AND OUT ANIMATION TO THE STORYBORD
            Storyboard windowFadeStoryBoard = new Storyboard();
            windowFadeStoryBoard.Children.Add(windowFadeAnimation);

            _popUpMessage.Loaded += ((s, e) =>
            {
                windowFadeStoryBoard.Begin(_popUpMessage);
            });

            #endregion POPUP MESSAGE ANIMATION

            #region HEADER ANIMATION

            if (_headerFlash)
            {
                //REGISTER THE WINDOWS NAME WHICH IS NECESSARY FOR CREATING THE STORYBOARD INSTEAD OF XAML
                NameScope.SetNameScope(_headerTextBlock, new NameScope());
                _headerTextBlock.RegisterName(_headerTextBlock.Name, _headerTextBlock);

                //CREATE THE IN AND OUT ANIMATION
                DoubleAnimation headerFlashAnimation = new DoubleAnimation();
                headerFlashAnimation.From = 1;
                headerFlashAnimation.To = 0;
                headerFlashAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.75));

                headerFlashAnimation.AutoReverse = true;
                headerFlashAnimation.RepeatBehavior = RepeatBehavior.Forever;

                //ACTUALLY DO NOTHING FOR NOW
                //headerFlashAnimation.Completed += ((s, e) => {});

                Storyboard.SetTargetName(headerFlashAnimation, _headerTextBlock.Name);
                Storyboard.SetTargetProperty(headerFlashAnimation, new PropertyPath(TextBlock.OpacityProperty));

                //ADD THE FADE IN AND OUT ANIMATION TO THE STORYBORD
                Storyboard headerFlashStoryboard = new Storyboard();
                headerFlashStoryboard.Children.Add(headerFlashAnimation);

                _popUpMessage.Loaded += ((s, e) =>
                {
                    headerFlashStoryboard.Begin(_headerTextBlock);
                });
            }

            #endregion HEADER ANIMATION
        }

        #endregion METHODS

        #region SUPPORTING EVENT CLASS

        //TO SUPPORT THE WINDOW CLOSED AND RESIZE EVENT
        public class RepositionWindowsService
        {
            //Two events (same signature) to keep is clear what they are used for.

            public static event Action<double, DateTime> WindowSizeChanged;
            public void OnWindowSizeChanged(double windowHeight, DateTime sizeChangedTime)
            {
                if (WindowSizeChanged != null)
                {
                    WindowSizeChanged(windowHeight, sizeChangedTime);
                }
            }

            public static event Action<double, DateTime> WindowLocationChanged;
            public void OnWindowLocationChanged(double shiftAmount, DateTime sizeChangedTime)
            {
                if (WindowLocationChanged != null)
                {
                    WindowLocationChanged(shiftAmount, sizeChangedTime);
                }
            }
        }

        #endregion SUPPORTING EVENT CLASS 
        #endregion
    }

    #region NON EXPANDABLE VERSION

    ///// <summary>
    ///// This provides the simplest way to configure a NONE expandable message. Several defaults are used here.
    ///// </summary>
    ///// <param name=”imageCloseName”>Relative Path to image from root. For e.g. @”images\x.gif”</param>
    ///// <param name=”headerText”> The header text</param>
    ///// <param name=”headerForegroundBrush”>The header foreground colour</param>
    ///// <param name=”mesageText”>The main text for the message</param>
    ///// <param name=”messageLength”>The length of the message to dispaly while message is displyed in its standard size, 168 is recommended when window height is 75 and width is 350 </param>
    ///// <param name=”borderBrush”>The border colour</param>
    //public ExpandableMessage(string imageCloseName, string headerText, Brush headerForegroundBrush,
    //    string mesageText, int messageLength, Brush borderBrush)
    //    : this(75, 115, 350, imageCloseName, string.Empty, string.Empty,
    //    headerText, headerForegroundBrush, mesageText, messageLength, 0,
    //    new Thickness(0), Brushes.White, borderBrush, new Thickness(1), new CornerRadius(2),
    //    0, 50, TimeSpan.FromSeconds(5), false, false)
    //{ }

    ///// <summary>
    ///// This provides a simple way to configure a NONE expandable message while allowing further configuration. Fewer defaults are used.
    ///// </summary>
    ///// <param name=”imageCloseName”>Relative Path to image from root. For e.g. @”images\x.gif”</param>
    ///// <param name=”headerText”> The header text</param>
    ///// <param name=”headerForegroundBrush”>The header foreground colour</param>
    ///// <param name=”mesageText”>The main text for the message</param>     
    ///// <param name=”messageLength”>The length of the message to dispaly while message is displyed in its standard size, 168 is recommended when window height is 75 and width is 350 </param>
    ///// <param name=”borderBrush”>The border colour</param>
    ///// <param name=”messageWindowRepeatBehaviorForever”>Set this to true if the messages RepeatBehavior should be RepeatBehavior.Forever otherwise set to false</param>
    ///// <param name=”headerFlash”>Set the to true if the header should flash</param>
    //public ExpandableMessage(string imageCloseName,
    //    string headerText, Brush headerForegroundBrush, string mesageText, int messageLength,
    //    Brush borderBrush, bool messageWindowRepeatBehaviorForever, bool headerFlash)
    //    : this(75, 115, 350, imageCloseName, string.Empty, string.Empty,
    //    headerText, headerForegroundBrush, mesageText, messageLength, 0,
    //    new Thickness(0), Brushes.White, borderBrush, new Thickness(1), new CornerRadius(2),
    //    0, 50, TimeSpan.FromSeconds(5), messageWindowRepeatBehaviorForever, headerFlash)
    //{ }

    ///// <summary>
    ///// This provides a way to configure a NONE expandable message while allowing greater controls over configuration. Fewer defaults are used.
    ///// </summary>      
    ///// <param name=”imageCloseName”>Relative Path to image from root. For e.g. @”images\x.gif”</param>
    ///// <param name=”headerText”> The header text</param>
    ///// <param name=”headerForegroundBrush”>The header foreground colour</param>
    ///// <param name=”mesageText”>The main text for the message</param>     
    ///// <param name=”messageLength”>The length of the message to dispaly while message is displyed in its standard size, 168 is recommended when window height is 75 and width is 350 </param>
    ///// <param name=”borderBrush”>The border colour</param>    
    ///// <param name=”borderThickness”>The thinkness of the border, a thickness of 2 is recommended</param>
    ///// <param name=”borderCornerRadius”>The corner radius for the border, a corner radius of 2 is recommended</param>
    ///// <param name=”messageWindowRepeatBehaviorForever”>Set this to true if the messages RepeatBehavior should be RepeatBehavior.Forever otherwise set to false</param>
    ///// <param name=”headerFlash”>Set the to true if the header should flash</param>
    //public ExpandableMessage(string imageCloseName,
    //    string headerText, Brush headerForegroundBrush, string mesageText, int messageLength,
    //    Brush borderBrush, Thickness borderThickness, CornerRadius borderCornerRadius,
    //    bool messageWindowRepeatBehaviorForever, bool headerFlash)
    //    : this(75, 115, 350, imageCloseName, string.Empty, string.Empty,
    //    headerText, headerForegroundBrush, mesageText, messageLength, 0,
    //    new Thickness(0), Brushes.White, borderBrush, borderThickness, borderCornerRadius,
    //    0, 50, TimeSpan.FromSeconds(5), messageWindowRepeatBehaviorForever, headerFlash)
    //{ }

    ///// <summary>
    ///// This provides a way to configure a NONE expandable message while allowing the greatest control over configuration.
    ///// </summary>
    ///// <param name=”windowHeight”>The height of the message window, 75 is recommended</param>
    ///// <param name=”windowWidth”>The width of the message window, 450 is recommended</param>
    ///// <param name=”imageCloseName”>Relative Path to image from root. For e.g. @”images\x.gif”</param>
    ///// <param name=”headerText”> The header text</param>
    ///// <param name=”headerForegroundBrush”>The header foreground colour</param>
    ///// <param name=”mesageText”>The main text for the message</param> 
    ///// <param name=”messageLength”>The length of the message to dispaly while message is displyed in its standard size, 168 is recommended when window height is 75 and width is 350 </param>
    ///// <param name=”messagePadding”>Padding for the message window, 2 is recommended</param>
    ///// <param name=”backgroundColour”> Message window background colour, white is recommended</param>
    ///// <param name=”borderBrush”>The border colour</param>   
    ///// <param name=”borderThickness”>The thinkness of the border, a thickness of 2 is recommended</param>
    ///// <param name=”borderCornerRadius”>The corner radius for the border, a corner radius of 2 is recommended</param>
    ///// <param name=”animationStartValue”>Set the animation’s start value, 0 is recommended</param>
    ///// <param name=”animationEndValue”>Set the animation’s end value</param>
    ///// <param name=”animationDuration”>Set the animation’s duration</param>
    ///// <param name=”messageWindowRepeatBehaviorForever”>Sets the value to indicate if the animation’s RepeatBehavior should be RepeatBehavior.Forever. In most cases this will be set to false.</param>
    ///// <param name=”headerFlash”>Set the to true if the header should flash</param>
    //public ExpandableMessage(double windowHeight, double windowWidth, string imageCloseName, string headerText,
    //    Brush headerForegroundBrush, string mesageText, int messageLength,
    //    Thickness messagePadding, Brush backgroundColour, Brush borderBrush, Thickness borderThickness,
    //    CornerRadius borderCornerRadius, double? animationStartValue, double? animationEndValue,
    //    TimeSpan animationDuration, bool messageWindowRepeatBehaviorForever, bool headerFlash) :
    //    this(windowHeight, 0, windowWidth, imageCloseName, string.Empty, string.Empty, headerText, headerForegroundBrush,
    //        mesageText, messageLength, 0, messagePadding, backgroundColour, borderBrush, borderThickness, borderCornerRadius,
    //        animationStartValue, animationEndValue, animationDuration, messageWindowRepeatBehaviorForever, headerFlash)
    //{
    //}
    #endregion

}
