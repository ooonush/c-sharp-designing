using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
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

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }
        public Vector3 MinBoundXYZ { get; }
        public Vector3 MaxBoundXYZ { get; }
        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            MinBoundXYZ = new Vector3(position.X - SizeX / 2, position.Y - SizeY / 2, position.Z - SizeZ / 2);
            MaxBoundXYZ = new Vector3(position.X + SizeX / 2, position.Y + SizeY / 2, position.Z + SizeZ / 2);
        }

        public RectangularCuboid(Vector3 minBound, Vector3 maxBound) : base(GetCenter(maxBound, maxBound))
        {
            MinBoundXYZ = minBound;
            MaxBoundXYZ = minBound;
            SizeX = maxBound.X - minBound.X;
            SizeY = maxBound.Y - minBound.Y;
            SizeZ = maxBound.Z - minBound.Z;
        }

        private static Vector3 GetCenter(Vector3 minBound, Vector3 maxBound)
        {
            Vector3 middle = maxBound - minBound;
            middle = new Vector3(middle.X / 2, middle.Y / 2, middle.Z / 2);
            return maxBound - middle;
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
            var boundingBoxes = GetBoundingBoxes();
            Vector3 minBound = GetMinBound(boundingBoxes);
            Vector3 maxBound = GetMaxBound(boundingBoxes);

            return new RectangularCuboid(minBound, maxBound);
        }

        private static Vector3 GetMinBound(IEnumerable<RectangularCuboid> boundingBoxes)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;
            var minZ = double.MaxValue;

            foreach (RectangularCuboid box in boundingBoxes)
            {
                if (box.MinBoundXYZ.X < minX)
                {
                    minX = box.MinBoundXYZ.X;
                }

                if (box.MinBoundXYZ.Y < minY)
                {
                    minY = box.MinBoundXYZ.Y;
                }

                if (box.MinBoundXYZ.Z < minZ)
                {
                    minZ = box.MinBoundXYZ.Z;
                }
            }

            return new Vector3(minX, minY, minZ);
        }
        
        private static Vector3 GetMaxBound(IEnumerable<RectangularCuboid> boundingBoxes)
        {
            var maxX = double.MinValue;
            var maxY = double.MinValue;
            var maxZ = double.MinValue;

            foreach (RectangularCuboid box in boundingBoxes)
            {
                if (box.MinBoundXYZ.X > maxX)
                {
                    maxX = box.MinBoundXYZ.X;
                }

                if (box.MinBoundXYZ.Y > maxY)
                {
                    maxY = box.MinBoundXYZ.Y;
                }

                if (box.MinBoundXYZ.Z > maxZ)
                {
                    maxZ = box.MinBoundXYZ.Z;
                }
            }

            return new Vector3(maxX, maxY, maxZ);
        }

        private RectangularCuboid[] GetBoundingBoxes()
        {
            return Parts.Select(p => p.GetBoundingBox()).ToArray();
        }
    }
}