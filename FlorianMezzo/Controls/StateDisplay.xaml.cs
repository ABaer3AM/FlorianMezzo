using System.Collections.ObjectModel;
using System.Diagnostics;
using FlorianMezzo.Controls.db;

namespace FlorianMezzo.Controls;

public partial class StateDisplay : ContentView
{
    public StateDisplay()
    {
        InitializeComponent();
    }

    // Overloaded Constructors to digest data easily-----------------------------------------------------
    public StateDisplay(string title, string feedback, int status)
    {
        InitializeComponent();

        Title = title;
        Feedback = feedback;
        Status = status;
    }
    public StateDisplay(string title, string feedback, int status, string note)
    {
        InitializeComponent();

        Title = title;
        Feedback = feedback;
        Status = status;

        if(note == "")
        {
            sdFrame.GestureRecognizers.Remove(noteGesture);
        }
        else
        {
            Note = note;
        }
    }
    public StateDisplay(DbData dataIn)
    {
        InitializeComponent();

        Title = dataIn.Title;
        Feedback = dataIn.Feedback;
        Status = dataIn.Status;

    }
    // --------------------------------------------------------------------------------------------------

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(StateDisplay),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (StateDisplay)bindable;

            control.titleText.Text = newValue as string;

        });

    public static readonly BindableProperty FeedbackProperty = BindableProperty.Create(
        nameof(Feedback),
        typeof(string),
        typeof(StateDisplay),
        defaultValue: "",
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (StateDisplay)bindable;

            control.UpdateFeedback(newValue as string);

        });


    public static readonly BindableProperty StatusProperty = BindableProperty.Create(
        nameof(Status),
        typeof(int),
        typeof(StateDisplay),
        defaultValue: 0, // Default to "success" state
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (StateDisplay)bindable;
            // Switch images based on status
            control.UpdateStatus((int)newValue);
        });

    public static readonly BindableProperty NoteProperty = BindableProperty.Create(
        nameof(Note),
        typeof(string),
        typeof(StateDisplay),
        defaultValue: "",
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (StateDisplay)bindable;

            control.UpdateNote(newValue as string);

        });


    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }
    public string Feedback
    {
        get => GetValue(FeedbackProperty) as string;
        set => SetValue(FeedbackProperty, value);
    }

    public int Status
    {
        get => (int)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }
    public string Note
    {
        get => GetValue(NoteProperty) as string;
        set => SetValue(NoteProperty, value);
    }



    public async void UpdateFull(int status, string feedback)
    {
        UpdateStatus(status);
        UpdateFeedback(feedback);
        await Task.Yield();
    }
    public void UpdateStatus(int status)
    {
        // Update the image source and background color based on the status value
        if (status == 1)
        {
            statusImage.Source = "check.png";  // Example: change image to a checkmark
            imageFrame.BackgroundColor = Color.FromHex("#66E44C"); // Green
        }
        else if (status == -1)
        {
            statusImage.Source = "exclamation.png";  // Example: change image to !
            imageFrame.BackgroundColor = Color.FromHex("#E9D75F"); // Yellow
        }
        else if (status == -2)
        {
            statusImage.Source = "running.png";  // Example: change image to running icon
            imageFrame.BackgroundColor = Color.FromHex("#83858a"); // Grey
            UpdateFeedback("Fetching...");
        }
        else
        {
            statusImage.Source = "xmark.png";  // Example: change image to X mark
            imageFrame.BackgroundColor = Color.FromHex("#F94620"); // Red
        }
        Status = status;
        /* Statuses
         *  1 -> Operational and Good Standing
         *  0 -> Non-operational
         * -1 -> Operational but not good standing (warning)
         * -2 -> Currently fetching the status (running)   */
    }

    public void UpdateFeedback(string feedback)
    {
        if(feedback.Length > 20)
        {
            feedback = feedback.Substring(0, 20) + "...";
        }
        feedbackText.Text = (feedback);
        Feedback = feedback;
    }
    public void UpdateNote(string note)
    {
        noteText.Text = note;
        Note = note;
    }

    private async void ShowNote(object sender, EventArgs e)
    {
        Debug.WriteLine("Maybe show note");
        if (Note != "" && Note != null)
        {
            RotateArrow();
            if (!DropdownNote.IsVisible)
            {
                DropdownNote.IsVisible = true;
                DropdownNote.TranslationY = -50; // Ensure it starts off-screen
                await DropdownNote.TranslateTo(0, 0, 200);  // Slide Down
            }
            else
            {
                await DropdownNote.TranslateTo(0, -50, 200);  // Slide Up
                DropdownNote.IsVisible = false;
            }
        }
    }

    public async void RotateArrow()
    {
        if (dropdownMarker.Rotation == 0)
        {
            await dropdownMarker.RotateTo(90, 200);
        }
        else
        {
            await dropdownMarker.RotateTo(0, 200);
        }
    }
}