using System;
using UIKit;
using TaskyShared;

namespace Tasky
{
    partial class TaskDetailController : UIViewController
    {
        readonly TaskManager taskMgr;

        public Task CurrentTask { get; set; }

        public TaskDetailController (IntPtr handle) : base (handle)
        {
            taskMgr = new TaskManager ();
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            Title = "Task Details";

            if (CurrentTask != null) {
                taskNameField.Text = CurrentTask.Name;
                taskNotesField.Text = CurrentTask.Notes;
                taskDoneSwitch.On = CurrentTask.Done;
            }
        }

        partial void SaveButton_TouchUpInside (UIButton sender)
        {
            CurrentTask.Name = taskNameField.Text;
            CurrentTask.Notes = taskNotesField.Text;
            CurrentTask.Done = taskDoneSwitch.On;

            taskMgr.SaveTask (CurrentTask);
            NavigationController.PopViewController (true);
        }

        partial void DeleteButton_TouchUpInside (UIButton sender)
        {
            if (CurrentTask.ID != null) {
                taskMgr.DeleteTask (CurrentTask);
            }

            NavigationController.PopViewController (true);
        }
    }
}
