﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace KompetansetorgetXamarin.Controls
{
    public class CustomSwitchCell : ViewCell
    {
        public event EventHandler<EventArgs> SwitchTapped;
        //public static BindableProperty nameProperty = BindableProperty.Create<CustomSwitchCell, Thickness>(d => d.Padding, default(Thickness));
        //public static BindableProperty toggleProperty = BindableProperty.Create<CustomSwitchCell, Thickness>(d => d.Padding, default(Thickness));

        public CustomSwitchCell()
        {
            var stack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10, 5, 10, 5),
                Spacing = 0
            };

            var NameLabel = new Label();
            NameLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            NameLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            NameLabel.LineBreakMode = LineBreakMode.NoWrap;
            NameLabel.TextColor = Color.White;
            NameLabel.SetBinding(Label.TextProperty, "Name");
            stack.Children.Add(NameLabel);

            var ToggleSwitch = new Switch();
            ToggleSwitch.HorizontalOptions = LayoutOptions.End;
            ToggleSwitch.SetBinding(Switch.IsToggledProperty, "IsSelected");
            stack.Children.Add(ToggleSwitch);

            View = stack;
        }
    }
}