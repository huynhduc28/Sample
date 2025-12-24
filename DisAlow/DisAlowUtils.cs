using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using CHD.Utils;

namespace CHD
{
    public class DisAlowUtils
    {
        public void MainWall(DisAlowViewModel vm)
        {
            List<Element> selected = RevitAPI_Query.SelectElements(vm.uidoc, new List<BuiltInCategory> { BuiltInCategory.OST_Walls }, "Select Wall. Press ESC to exit");
            if (selected == null || selected.Count == 0) return;

            XYZ pointA = null;
            if (vm.IsOneSelected)
            {
                try
                {
                    pointA = vm.uidoc.Selection.PickPoint("Pick a point to determine the nearest wall end for joining.");
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                if (pointA == null) return;
            }
            using (Transaction tx = new Transaction(vm.doc, vm.ToolName))
            {
                tx.Start();
                foreach (Element elem in selected)
                {
                    if (elem is Wall wall)
                    {
                        ProcessWallJoin(wall, vm, pointA);
                    }
                }
                tx.Commit();
            }
        }

        public void MainBeam(DisAlowViewModel vm)
        {
            List<Element> selected = RevitAPI_Query.SelectElements(vm.uidoc, new List<BuiltInCategory> { BuiltInCategory.OST_StructuralFraming }, "Select StructuralFraming. Press ESC to exit");
            if (selected == null || selected.Count == 0) return;
            XYZ pointA = null;
            if (vm.IsOneSelected)
            {
                try
                {
                    pointA = vm.uidoc.Selection.PickPoint("Pick a point to determine the nearest beam end for joining.");
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                if (pointA == null) return;
            }

            using (Transaction tx = new Transaction(vm.doc, vm.ToolName))
            {
                tx.Start();
                foreach (Element elem in selected)
                {
                    if (elem is FamilyInstance beam)
                    {
                        ProcessBeamJoin(beam, vm, pointA);
                    }
                }
                tx.Commit();
            }
        }

        private void ProcessBeamJoin(FamilyInstance beam, DisAlowViewModel vm, XYZ pointA = null)
        {
            if (vm.IsTwoSelected)
            {
                if (vm.IsDisallowSelected)
                {
                    StructuralFramingUtils.DisallowJoinAtEnd(beam, 0);
                    StructuralFramingUtils.DisallowJoinAtEnd(beam, 1);
                }
                else if (vm.IsAllowSelected)
                {
                    StructuralFramingUtils.AllowJoinAtEnd(beam, 0);
                    StructuralFramingUtils.AllowJoinAtEnd(beam, 1);
                }
            }
            else if (vm.IsOneSelected && pointA != null)
            {
                BeamJoinController ctrl = new BeamJoinController(beam);
                if (vm.IsDisallowSelected)
                {
                    ctrl.DisallowNearestEnd(pointA);
                }
                else if (vm.IsAllowSelected)
                {
                    ctrl.AllowNearestEnd(pointA);
                }
            }
        }

        private void ProcessWallJoin(Wall wall, DisAlowViewModel vm, XYZ pointA = null)
        {
            if (vm.IsTwoSelected)
            {
                if (vm.IsDisallowSelected)
                {
                    WallUtils.DisallowWallJoinAtEnd(wall, 0);
                    WallUtils.DisallowWallJoinAtEnd(wall, 1);
                }
                else if (vm.IsAllowSelected)
                {
                    WallUtils.AllowWallJoinAtEnd(wall, 0);
                    WallUtils.AllowWallJoinAtEnd(wall, 1);
                }
            }
            else if (vm.IsOneSelected && pointA != null)
            {
                WallJoinController ctrl = new WallJoinController(wall);
                if (vm.IsDisallowSelected)
                {
                    ctrl.DisallowNearestEnd(pointA);
                }
                else if (vm.IsAllowSelected)
                {
                    ctrl.AllowNearestEnd(pointA);
                }
            }
        }
    }
}