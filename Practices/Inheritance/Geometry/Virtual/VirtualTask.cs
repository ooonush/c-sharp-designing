using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            Bounds bounds = GetBounds();

            double sizeX = bounds.Max.X - bounds.Min.X;
            double sizeY = bounds.Max.Y - bounds.Min.Y;
            double sizeZ = bounds.Max.Z - bounds.Min.Z;

            return new RectangularCuboid(bounds.GetCenter(), sizeX, sizeY, sizeZ);
        }

        private Bounds GetBounds()
        {
            var minBound = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
            var maxBound = new Vector3(double.MinValue, double.MinValue, double.MinValue);

            foreach (Body part in Parts)
            {
                RectangularCuboid boundingBox = part.GetBoundingBox();
                Vector3 currentMax = boundingBox.Bounds.Max;
                maxBound = new Vector3(
                    Math.Max(currentMax.X, maxBound.X),
                    Math.Max(currentMax.Y, maxBound.Y),
                    Math.Max(currentMax.Z, maxBound.Z));

                Vector3 currentMin = boundingBox.Bounds.Min;
                minBound = new Vector3(
                    Math.Min(currentMin.X, minBound.X),
                    Math.Min(currentMin.Y, minBound.Y),
                    Math.Min(currentMin.Z, minBound.Z));
            }

            return new Bounds(minBound, maxBound);
        }
    }

    public readonly struct Bounds
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public Bounds(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 GetCenter()
        {
            Vector3 difference = Max - Min;
            difference = new Vector3(difference.X / 2, difference.Y / 2, difference.Z / 2);
            return Max - difference;
        }
    }

    public class RectangularCuboid : Body
    {
        public readonly Bounds Bounds;

        public readonly double SizeX;
        public readonly double SizeY;
        public readonly double SizeZ;
        
        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            
            var minBound = new Vector3(position.X - sizeX / 2, position.Y - sizeY / 2, position.Z - sizeZ / 2);
            var maxBound = new Vector3(position.X + sizeX / 2, position.Y + sizeY / 2, position.Z + sizeZ / 2);
            
            Bounds = new Bounds(minBound, maxBound);
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                Position.X - SizeX / 2,
                Position.Y - SizeY / 2,
                Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return this;
        }
    }

    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            Vector3 vector = point - Position;
            double length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            double diameter = Radius * 2;
            return new RectangularCuboid(Position, diameter, diameter, diameter);
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            double vectorX = point.X - Position.X;
            double vectorY = point.Y - Position.Y;
            double length2 = vectorX * vectorX + vectorY * vectorY;
            double minZ = Position.Z - SizeZ / 2;
            double maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            double diameter = Radius * 2;
            return new RectangularCuboid(Position, diameter, diameter, SizeZ);
        }
    }
}