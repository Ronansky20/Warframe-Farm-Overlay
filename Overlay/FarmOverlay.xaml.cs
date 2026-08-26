using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Overlay
{
    /// <summary>
    /// Interaction logic for FarmOverlay.xaml
    /// </summary>
    public partial class FarmOverlay : Window
    {
        public FarmOverlay()
        {
            InitializeComponent();
        }

        public void SetPlan(FarmPlan plan)
        {
            TitleText.Text = plan.Target;
            ItemsPanel.Children.Clear();

            foreach (var line in plan.Lines)
            {
                string location = line.BestLocation?.Location ?? "No Drop Location";

                var text = new TextBlock
                {
                    Text = $"{line.Name}   {line.Owned}/{line.Needed}   {location}",
                    Foreground = System.Windows.Media.Brushes.White,
                    Margin = new Thickness(8, 2, 8, 2)
                };

                ItemsPanel.Children.Add(text);
            }
        }
    }
}
