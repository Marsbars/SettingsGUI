using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using Dropdown = System.String;

namespace MarsSettingsGUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        //public Settings settings;
        public SettingsWindow(Settings settings, string theme = "Blue")
        {
            InitializeComponent();

            ThemeManager.AddAccent("Warrior", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Warrior.xaml"));
            ThemeManager.AddAccent("DeathKnight", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/DeathKnight.xaml"));
            ThemeManager.AddAccent("Druid", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Druid.xaml"));
            ThemeManager.AddAccent("Hunter", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Hunter.xaml"));
            ThemeManager.AddAccent("Mage", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Mage.xaml"));
            ThemeManager.AddAccent("Paladin", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Paladin.xaml"));
            ThemeManager.AddAccent("Priest", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Priest.xaml"));
            ThemeManager.AddAccent("Rogue", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Rogue.xaml"));
            ThemeManager.AddAccent("Shaman", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Shaman.xaml"));
            ThemeManager.AddAccent("Warlock", new Uri("pack://application:,,,/MarsSettingsGUI;component/Themes/Warlock.xaml"));

            //var x = ThemeManager.GetAccent("Warlock");
            //x.Resources["AccentColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e542f4"));
            //x.Name = "a";
            //var z = ThemeManager.GetAccent("a");

            GenerateGUI(settings);


            ThemeManager.ChangeAppStyle(this,
                ThemeManager.GetAccent(theme),
                ThemeManager.GetAppTheme("BaseDark"));
            //ThemeManager.ChangeAppTheme(this, "BaseLight");
        }

        private void GenerateGUI(Settings settings)
        {
            this.Title = settings.ToString();
            List<string> list = new List<string>();

            var properties = settings.GetType().GetProperties();

            var categories = new List<string>();
            foreach (var item in properties.Where(x => x.CustomAttributes.Count() > 0))
            {
                categories.Add(item.GetCustomAttribute<CategoryAttribute>().Category);
            }

            foreach (var category in categories.Distinct())
            {
                TabItem tab = new TabItem
                {
                    Name = category,
                    Header = category
                };
                mainTabControl.Items.Add(tab);

                ScrollViewer scrollViewer = new ScrollViewer
                {
                    MaxHeight = 550,
                    //Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444")),
                    //Margin = new Thickness(4)
                };

                StackPanel stackPanel = new StackPanel
                {
                    Name = category + "stack",
                    Margin = new Thickness(20)
                };

                scrollViewer.Content = stackPanel;
                tab.Content = scrollViewer;
                foreach (var property in properties.Where(x => x.CustomAttributes.Count() > 0 && x.GetCustomAttribute<CategoryAttribute>().Category == category).OrderBy(x => x.GetCustomAttribute<OrderAttribute>() != null ? x.GetCustomAttribute<OrderAttribute>().Order : 99))
                {
                    var type = property.PropertyType;
                    var propertyName = property.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                    var isPercent = property.GetCustomAttribute<PercentageAttribute>() != null ? property.GetCustomAttribute<PercentageAttribute>().IsPercentage : false;
                    var isDropdown = property.GetCustomAttribute<DropdownListAttribute>() != null;
                    Border border = new Border
                    {
                        BorderBrush = Brushes.SlateGray,
                        BorderThickness = new Thickness(0, 0, 0, 1)
                    };
                    stackPanel.Children.Add(border);
                    //var valueaa = property.GetValue(MarsMageTBCSetting.CurrentSetting);
                    if (type == typeof(bool))
                    {
                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5),
                            //Orientation = Orientation.Horizontal,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        Label labelname = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            //Margin = new Thickness(0, 10, 5, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 150,
                        };

                        Binding binding = new Binding(property.Name)
                        {
                            Source = settings,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };

                        ToggleSwitchButton tglSwitch = new ToggleSwitchButton
                        {
                            Name = property.Name + "tgl",
                            //Header = propertyName,
                            //Margin = new Thickness(10),
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : "",
                            FontFamily = this.FontFamily,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            //HeaderFontFamily = this.FontFamily,

                        };
                        tglSwitch.SetBinding(ToggleSwitchButton.IsCheckedProperty, binding);

                        dock.Children.Add(labelname);
                        dock.Children.Add(tglSwitch);
                        border.Child = dock;
                        //stackPanel.Children.Add(tglSwitch);
                    }
                    if (type == typeof(int) && isPercent)
                    {
                        StackPanel dock = new StackPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5),
                            Orientation = Orientation.Horizontal
                        };

                        Label labelname = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 0, 5, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 154,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        Label labelvalue = new Label
                        {
                            Name = property.Name + "labelvalue",
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Width = 50
                        };

                        Slider slider = new Slider
                        {
                            Name = property.Name + "slider",
                            TickFrequency = 5,
                            Minimum = property.GetCustomAttribute<ValueRangeAttribute>() != null ? property.GetCustomAttribute<ValueRangeAttribute>().MinValue : 0,
                            Maximum = property.GetCustomAttribute<ValueRangeAttribute>() != null ? property.GetCustomAttribute<ValueRangeAttribute>().MaxValue : 100,
                            IsSnapToTickEnabled = true,
                            Width = 230,
                            Margin = new Thickness(0, 8, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Right
                        };
                        //slider.SetBinding(Slider.ActualWidthProperty,)

                        Binding binding = new Binding("Value")
                        {
                            Source = slider,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        labelvalue.SetBinding(Label.ContentProperty, binding);

                        Binding sliderBinding = new Binding(property.Name)
                        {
                            Source = settings,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        slider.SetBinding(Slider.ValueProperty, sliderBinding);

                        dock.Children.Add(labelname);
                        dock.Children.Add(slider);
                        dock.Children.Add(labelvalue);

                        border.Child = dock;
                        //stackPanel.Children.Add(dock);
                    }
                    if (type == typeof(string) && !isDropdown)
                    {
                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5),
                            //Orientation = Orientation.Horizontal,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        Label labelname = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 5, 5, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 150,
                        };

                        TextBox textBox = new TextBox
                        {
                            Name = property.Name + "textBox",
                            Width = 250,
                            Margin = new Thickness(5),
                            FontFamily = this.FontFamily,
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Right,

                        };

                        Binding binding = new Binding(property.Name)
                        {
                            Source = settings,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        textBox.SetBinding(TextBox.TextProperty, binding);

                        dock.Children.Add(labelname);
                        dock.Children.Add(textBox);
                        border.Child = dock;
                    }
                    if (type == typeof(List<string>))
                    {
                        StackPanel stack = new StackPanel
                        {
                            Name = property.Name + "stack",
                            Margin = new Thickness(5)
                        };

                        Label label = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        //string[] stringarray = (string[])property.GetValue(settings);

                        ObservableCollection<string> collection = new ObservableCollection<string>((List<string>)property.GetValue(settings));

                        DataGrid grid = new DataGrid
                        {
                            AutoGenerateColumns = false,
                            Name = property.Name + "grid",
                            MaxHeight = 300,
                            Margin = new Thickness(5),
                            ItemsSource = collection,//(List<string>)property.GetValue(settings);
                            FontSize = 14,
                            // IsHitTestVisible = false
                        };
                        grid.Columns.Add(new DataGridTextColumn() { Header = "Item", Binding = new Binding(), Width = 300 });


                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5)
                        };

                        Button addButton = new Button
                        {
                            Name = property.Name + "addbtn",
                            Content = "Add",
                            Width = 100,
                            Margin = new Thickness(2)
                        };

                        addButton.Click += async (sender, e) =>
                        {
                            var metroDialogSettings = new MetroDialogSettings()
                            {
                                AffirmativeButtonText = "Add",
                                NegativeButtonText = "Cancel",
                                AnimateHide = true,
                                AnimateShow = true,
                                ColorScheme = MetroDialogColorScheme.Theme
                            };
                            var x = await this.ShowInputAsync("Add", "Item", metroDialogSettings);
                            if (x != null)
                            {
                                //var y = property.GetValue(settings);
                                //property.PropertyType.GetMethod("Add").Invoke(y, new[] { x });
                                collection.Add(x);
                                property.SetValue(settings, collection.ToList());
                            }
                        };

                        Button removeButton = new Button
                        {
                            Name = property.Name + "removebtn",
                            Content = "Remove selected",
                            Width = dock.Width / 2,
                            Margin = new Thickness(2)
                        };
                        removeButton.Click += (sender, e) =>
                        {
                            // Yay.
                            if (grid.SelectedIndex >= 0)
                            {
                                collection.Remove(grid.SelectedValue.ToString());
                                property.SetValue(settings, collection.ToList());
                            }
                        };

                        dock.Children.Add(addButton);
                        dock.Children.Add(removeButton);

                        stack.Children.Add(label);
                        stack.Children.Add(grid);
                        stack.Children.Add(dock);
                        border.Child = stack;

                        //this.Closing += (sender,e) => { property.SetValue(};
                    }
                    if (type == typeof(List<int>))
                    {
                        StackPanel stack = new StackPanel
                        {
                            Name = property.Name + "stack"
                        };

                        Label label = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        //string[] stringarray = (string[])property.GetValue(settings);

                        ObservableCollection<int> collection = new ObservableCollection<int>((List<int>)property.GetValue(settings));

                        DataGrid grid = new DataGrid
                        {
                            AutoGenerateColumns = false,
                            Name = property.Name + "grid",
                            MaxHeight = 300,
                            Margin = new Thickness(5),
                            ItemsSource = collection,//(List<string>)property.GetValue(settings);
                            FontSize = 14,
                            //IsHitTestVisible = false
                        };
                        grid.Columns.Add(new DataGridTextColumn() { Header = "Item", Binding = new Binding(), Width = 300 });


                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5)
                        };

                        Button addButton = new Button
                        {
                            Name = property.Name + "addbtn",
                            Content = "Add",
                            Width = 100,
                            Margin = new Thickness(2)
                        };

                        addButton.Click += async (sender, e) =>
                        {
                            var metroDialogSettings = new MetroDialogSettings()
                            {
                                AffirmativeButtonText = "Add",
                                NegativeButtonText = "Cancel",
                                AnimateHide = true,
                                AnimateShow = true,
                                ColorScheme = MetroDialogColorScheme.Theme
                            };
                            var x = await this.ShowInputAsync("Add", "Item", metroDialogSettings);
                            if (Int32.TryParse(x, out int y))
                            {
                                //var y = property.GetValue(settings);
                                //property.PropertyType.GetMethod("Add").Invoke(y, new[] { x });
                                collection.Add(y);
                                property.SetValue(settings, collection.ToList());
                            }
                        };

                        Button removeButton = new Button
                        {
                            Name = property.Name + "removebtn",
                            Content = "Remove selected",
                            Width = dock.Width / 2,
                            Margin = new Thickness(2)
                        };
                        removeButton.Click += (sender, e) =>
                        {
                            // Yay.
                            if (grid.SelectedIndex >= 0)
                            {
                                collection.Remove(Int32.Parse(grid.SelectedValue.ToString()));
                                property.SetValue(settings, collection.ToList());
                            }
                        };

                        dock.Children.Add(addButton);
                        dock.Children.Add(removeButton);

                        stack.Children.Add(label);
                        stack.Children.Add(grid);
                        stack.Children.Add(dock);
                        border.Child = stack;

                        //this.Closing += (sender,e) => { property.SetValue(};
                    }
                    if (type == typeof(int) && !isPercent)
                    {
                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5),
                            //Orientation = Orientation.Horizontal,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        Label labelname = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 5, 5, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 150,
                        };

                        NumericUpDown textBox = new NumericUpDown
                        {
                            Name = property.Name + "textBox",
                            Width = 250,
                            Margin = new Thickness(5),
                            FontFamily = this.FontFamily,
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Right,
                        };

                        Binding binding = new Binding(property.Name)
                        {
                            Source = settings,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        textBox.SetBinding(NumericUpDown.ValueProperty, binding);

                        dock.Children.Add(labelname);
                        dock.Children.Add(textBox);
                        border.Child = dock;
                    }
                    if (type == typeof(string) && isDropdown)
                    {
                        DockPanel dock = new DockPanel
                        {
                            Name = property.Name + "dock",
                            Margin = new Thickness(5),
                            //Orientation = Orientation.Horizontal,
                            ToolTip = property.GetCustomAttribute<DescriptionAttribute>() != null ? property.GetCustomAttribute<DescriptionAttribute>().Description : ""
                        };

                        Label labelname = new Label
                        {
                            Name = property.Name + "labelname",
                            Content = propertyName,
                            FontWeight = FontWeights.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 5, 5, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 150,
                        };

                        ComboBox textBox = new ComboBox
                        {
                            Name = property.Name + "textBox",
                            Width = 250,
                            Margin = new Thickness(5),
                            FontFamily = this.FontFamily,
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            ItemsSource = property.GetCustomAttribute<DropdownListAttribute>().List,
                            
                            //SelectedItem = "Value"
                        };

                        Binding binding = new Binding(property.Name)
                        {
                            Source = settings,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            //Path = new PropertyPath("Value"),
                            
                        };
                        textBox.SetBinding(ComboBox.SelectedValueProperty, binding);

                        dock.Children.Add(labelname);
                        dock.Children.Add(textBox);
                        border.Child = dock;
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValueRangeAttribute : Attribute
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public ValueRangeAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderAttribute : Attribute
    {
        public int Order { get; set; }

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DropdownListAttribute : Attribute
    {
        public string[] List { get; set; }

        public DropdownListAttribute(string[] list)
        {
            List = list;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PercentageAttribute : Attribute
    {
        public bool IsPercentage { get; set; }

        public PercentageAttribute(bool isPercentage)
        {
            IsPercentage = isPercentage;
        }
    }
}
