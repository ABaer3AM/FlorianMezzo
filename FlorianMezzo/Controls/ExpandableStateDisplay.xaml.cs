using System.Diagnostics;
using System.Runtime.CompilerServices;
using FlorianMezzo.Controls.db;
using Microsoft.Maui.Controls.Shapes;

namespace FlorianMezzo.Controls;

public partial class ExpandableStateDisplay : ContentView
{
    public event EventHandler<(string title, int isOpen)> OnDropdownToggled;

    public HashSet<string> OpenedTitles { get; set; } = new();


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



    public void UpdateMainStateDisplay(StateDisplay newStateDisplay)
    {
        MainStateDisplayContainer.Content = newStateDisplay;
    }

    // Overloaded method to UPDATE ESD WITH A LIST OF STATUSES---------------------------
    public void UpdateDropdownContent(List<StateDisplay> newList)
    {
        DropdownContent.Children.Clear();

        foreach (var stateDisplay in newList)
        {
            // Hook into the dropdown opened event
            stateDisplay.DropdownToggled += (s, info) =>
            {
                OnDropdownToggled?.Invoke(this, info);
            };

            // Restore dropdown state if open
            if (OpenedTitles.Contains(stateDisplay.Title))
            {
                stateDisplay.ForceDropdownOpen();
            }

            Debug.WriteLine($"[ESD] Subscribed close for: {stateDisplay.Title}2");


            var frame = new Frame
            {
                Content = stateDisplay,
                CornerRadius = 0,
                Padding = 0,
                Margin = 0,
                ZIndex = 2,
                BackgroundColor = Colors.Transparent,
                HasShadow = false
            };

            DropdownContent.Children.Add(frame);
        }
    }

    public void UpdateDropdownContent(List<DbData> dataList)
    {
        bool isValid = true;
        DropdownContent.Children.Clear();
        bool hasCritical = false;

        foreach (var dataEntry in dataList)
        {
            var stateDisplay = new StateDisplay(dataEntry);

            // Hook into the dropdown opened event
            stateDisplay.DropdownToggled += (s, title) =>
            {
                OnDropdownToggled?.Invoke(this, title);
            };

            // Restore dropdown state if open
            if (OpenedTitles.Contains(stateDisplay.Title))
            {
                stateDisplay.ForceDropdownOpen();
            }

            var border = new Border
            {
                Content = stateDisplay,
                Padding = 0,
                Margin = 0,
                ZIndex = 2,
                BackgroundColor = Colors.Transparent,
                StrokeThickness = 0,
                Shadow = null
            };

            DropdownContent.Children.Add(border);

            // maybe update main display
            if (dataEntry.Status != 1)
            {
                isValid = false;
                if (!hasCritical)
                {
                    UpdateMainStateDisplay(new StateDisplay(MainStateDisplay.Title, $"Issue with {dataEntry.Title}", dataEntry.Status, ""));
                    if (dataEntry.Status == 0) { hasCritical = true; }
                }
            }
        }

        if (isValid && dataList.Count > 0)
        {
            UpdateMainStateDisplay(new StateDisplay(MainStateDisplay.Title, "Operational", 1, ""));
        }
        else if (dataList.Count <= 0)
        {
            UpdateMainStateDisplay(new StateDisplay(MainStateDisplay.Title, "No Data Found", 0, ""));
        }
    }

    // ----------------------------------------------------------------------------------


    public async void ExpandStateDisplays(object sender, EventArgs e)
    {
        if (!DropdownMenu.IsVisible)
        {
            DropdownMenu.IsVisible = true;
            MainStateDisplay.RotateArrow(1);
            DropdownMenu.TranslationY = -(DropdownMenu.Height - 50); // Ensure it starts off-screen
            await DropdownMenu.TranslateTo(0, 0, 200);  // Slide Down
        }
        else
        {
            MainStateDisplay.RotateArrow(-1);
            await DropdownMenu.TranslateTo(0, -(DropdownMenu.Height - 50), 200);  // Slide Down
            DropdownMenu.IsVisible = false;
        }
    }
}