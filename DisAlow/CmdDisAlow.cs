using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static CHD.DisAlowViewModel;

namespace CHD
{
    [Transaction(TransactionMode.Manual)]
    internal class CmdDisAlow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            try
            {
                DisAlowViewModel viewModel = new DisAlowViewModel(uidoc);
                DisAlowView window = new DisAlowView() { DataContext = viewModel };
                viewModel.MainWindow = window;


                window.ShowDialog();

                DisAlowUtils utils = new DisAlowUtils();

                switch (viewModel.ActionType)
                {
                    case ActionTypeEnum.RunWall:
                        utils.MainWall(viewModel);
                        break;
                    case ActionTypeEnum.RunBeam:
                        utils.MainBeam(viewModel);
                        break;
                    case ActionTypeEnum.Cancel:
                    case ActionTypeEnum.None:
                    default:
                        return Result.Cancelled;
                }
            }
            catch
            {
                return Result.Failed;
            }                           
            return Result.Succeeded;

        }

    }
}
