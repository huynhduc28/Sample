using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHD.Utils
{
    public class BeamJoinController
    {
        private readonly FamilyInstance _beam;
        private readonly Curve _curve;

        public BeamJoinController(Element elem)
        {
            _beam = elem as FamilyInstance
                ?? throw new ArgumentException("Element is not beam");

            LocationCurve lc = _beam.Location as LocationCurve;
            _curve = lc.Curve;
        }

        public int GetNearestEnd(XYZ A)
        {
            XYZ p0 = _curve.GetEndPoint(0);
            XYZ p1 = _curve.GetEndPoint(1);

            double d0 = p0.DistanceTo(A);
            double d1 = p1.DistanceTo(A);

            return d0 <= d1 ? 0 : 1;
        }

        public void DisallowNearestEnd(XYZ A)
        {
            int nearest = GetNearestEnd(A);
            StructuralFramingUtils.DisallowJoinAtEnd(_beam, nearest);
        }

        public void AllowNearestEnd(XYZ A)
        {
            int nearest = GetNearestEnd(A);
            StructuralFramingUtils.AllowJoinAtEnd(_beam, nearest);
        }
}
    public class WallJoinController
    {
        private readonly Wall _wall;
        private readonly Curve _curve;

        public WallJoinController(Element elem)
        {
            _wall = elem as Wall
                ?? throw new ArgumentException("Element is not a wall");

            LocationCurve lc = _wall.Location as LocationCurve;
            _curve = lc.Curve;
        }

        public int GetNearestEnd(XYZ A)
        {
            XYZ p0 = _curve.GetEndPoint(0);
            XYZ p1 = _curve.GetEndPoint(1);

            double d0 = p0.DistanceTo(A);
            double d1 = p1.DistanceTo(A);

            return d0 <= d1 ? 0 : 1;
        }

        public void DisallowNearestEnd(XYZ A)
        {
            int nearest = GetNearestEnd(A);
            WallUtils.DisallowWallJoinAtEnd(_wall, nearest);
        }

        public void AllowNearestEnd(XYZ A)
        {
            int nearest = GetNearestEnd(A);
            WallUtils.AllowWallJoinAtEnd(_wall, nearest);
        }
    }
}
