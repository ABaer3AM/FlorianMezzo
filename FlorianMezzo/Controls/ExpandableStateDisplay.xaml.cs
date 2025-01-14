using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Microsoft.Maui.Controls;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Controls;

public partial class ExpandableStateDisplay : ContentView
{
    public static readonly BindableProperty MainStateDisplayProperty = BindableProperty.Create(
        nameof(MainStateDisplay),
        typeof(StateDisplay),
        typeof(ExpandableStateDisplay),
        propertyChanged: OnMainStateDisplayChanged);

    public static readonly BindableProperty StateDisplaysProperty = BindableProperty.Create(
        nameof(StateDisplays),
        typeof(List<StateDisplay>),
        typeof(ExpandableStateDisplay),
        defaultValue: new List<StateDisplay>(),
        propertyChanged: OnStateDisplaysChanged);


    public StateDisplay MainStateDisplay
    {
        get => (StateDisplay)GetValue(MainStateDisplayProperty);
        set => SetValue(MainStateDisplayProperty, value);
    }

    public List<StateDisplay> StateDisplays
    {
        get => (List<StateDisplay>)GetValue(StateDisplaysProperty);
        set => SetValue(StateDisplaysProperty, value);
    }



    public ExpandableStateDisplay()
    {
        InitializeComponent();
    }



    private static void OnMainStateDisplayChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpandableStateDisplay expandableStateDisplay && newValue is StateDisplay newStateDisplay)
        {
            expandableStateDisplay.UpdateMainStateDisplay(newStateDisplay);
        }
    }

    private static void OnStateDisplaysChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExpandableStateDisplay expandableStateDisplay && newValue is List<StateDisplay> newList)
        {
            Debug.WriteLine("Changing state displays");
            expandableStateDisplay.UpdateDropdownContent(newList);
        }
    }



    private void UpdateMainStateDisplay(StateDisplay newStateDisplay)
    {
        MainStateDisplayContainer.Content = newStateDisplay;
    }

    public void UpdateDropdownContent(List<StateDisplay> newList)
    {
        DropdownContent.Children.Clear();

        foreach (var stateDisplay in newList)
        {
            // Wrap each StateDisplay in a Frame or directly add to the dropdown
            var frame = new Frame
            {
                Content = stateDisplay,
                CornerRadius = 0,
                Padding = 0,
                Margin = 0,
                ZIndex = 2,
                BackgroundColor = Colors.Transparent, // Transparent background
                HasShadow = false                     // No border or shadow
            };

            DropdownContent.Children.Add(frame);
        }
    }
    public void UpdateDropdownContent(List<SoftDependencyData> softDataList)
    {
        DropdownContent.Children.Clear();

        foreach (var softData in softDataList)
        {
            // Wrap each StateDisplay in a Frame or directly add to the dropdown
            var frame = new Frame
            {
                Content = new StateDisplay(softData),
                CornerRadius = 0,
                Padding = 0,
                Margin = 0,
                ZIndex = 2,
                BackgroundColor = Colors.Transparent, // Transparent background
                HasShadow = false                     // No border or shadow
            };

            DropdownContent.Children.Add(frame);

            // maybe update main deisplay
            if(softData.Status != 1) {
                UpdateMainStateDisplay(new StateDisplay("Software Dependencies", $"Issue with {softData.Title}", softData.Status));
            };
        }
    }

    public void UpdateFull(List<SoftDependencyData> softDataList)
    {
        
    }

    public async void ExpandStateDisplays(object sender, EventArgs e)
    {
        MainStateDisplay.RotateArrow();
        if (!DropdownMenu.IsVisible)
        {
            DropdownMenu.IsVisible = true;
            DropdownMenu.TranslationY = -(DropdownMenu.Height - 50); // Ensure it starts off-screen
            await DropdownMenu.TranslateTo(0, 0, 200);  // Slide Down
        }
        else
        {
            await DropdownMenu.TranslateTo(0, -(DropdownMenu.Height - 50), 200);  // Slide Down
            DropdownMenu.IsVisible = false;
        }
    }
}