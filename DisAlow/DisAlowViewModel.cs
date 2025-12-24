using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using System.Windows;
using System.Windows.Input;

namespace CHD
{
    [AddINotifyPropertyChangedInterface]
    public class DisAlowViewModel
    {
        public enum ActionTypeEnum
        {
            None,
            RunWall,
            RunBeam,
            Cancel
        }
        public Document doc { get; set; }
        public UIDocument uidoc { get; set; }
        public string ToolName { get; set; }
        public bool IsDisallowSelected { get; set; }
        public bool IsAllowSelected { get; set; }
        public bool IsTwoSelected { get; set; }
        public bool IsOneSelected { get; set; }

        public ActionTypeEnum ActionType { get; set; } = ActionTypeEnum.None;
        public DisAlowView MainWindow { get; set; }
        public ICommand WallCmd { get; }
        public ICommand BeamCmd { get; }
        public ICommand CancelCmd { get; }
        public DisAlowViewModel(UIDocument uiDoc)
        {
             this.uidoc = uiDoc;
            this.doc = uiDoc.Document;
            string toolName = "Dis-Alow Join";       
            ToolName = toolName;
            IsTwoSelected = true;
            IsDisallowSelected = true;
            IsAllowSelected = false;
            IsOneSelected = false;

            WallCmd = new RelayCommand<Window>((p) =>
            {
                ActionType = ActionTypeEnum.RunWall;
                MainWindow.DialogResult = true;
            });

            BeamCmd = new RelayCommand<Window>((p) =>
            {
                ActionType = ActionTypeEnum.RunBeam;
                MainWindow.DialogResult = true;
            });
            CancelCmd = new RelayCommand<Window>((p) =>
            {
                ActionType = ActionTypeEnum.Cancel;
                MainWindow.DialogResult = false;
                p.Close();
            });
        }

    }
}
