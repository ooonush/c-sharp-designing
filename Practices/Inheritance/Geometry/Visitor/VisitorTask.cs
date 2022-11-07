using System;
using System.Collections.Generic;

namespace Inheritance.Geometry.Visitor
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class RectangularCuboid : Body
    {
        public readonly double SizeX;
        public readonly double SizeY;
        public readonly double SizeZ;
        
        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
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
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
    
    public interface IVisitor<out T>
    {
        T Visit(Cylinder cylinder);
        T Visit(Ball ball);
        T Visit(RectangularCuboid rectangularCuboid);
        T Visit(CompoundBody compoundBody);
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

    public class BoundingBoxVisitor : IVisitor<RectangularCuboid>
    {
        public RectangularCuboid Visit(Cylinder cylinder)
        {
            double diameter = cylinder.Radius * 2;
            return new RectangularCuboid(cylinder.Position, diameter, diameter, cylinder.SizeZ);
        }

        public RectangularCuboid Visit(Ball ball)
        {
            double diameter = ball.Radius * 2;
            return new RectangularCuboid(ball.Position, diameter, diameter, diameter);
        }

        public RectangularCuboid Visit(CompoundBody compoundBody)
        {
            Bounds bounds = compoundBody.GetBounds();

            double sizeX = bounds.Max.X - bounds.Min.X;
            double sizeY = bounds.Max.Y - bounds.Min.Y;
            double sizeZ = bounds.Max.Z - bounds.Min.Z;

            return new RectangularCuboid(bounds.GetCenter(), sizeX, sizeY, sizeZ);
        }

        public RectangularCuboid Visit(RectangularCuboid rectangularCuboid)
        {
            return rectangularCuboid;
        }
    }

    public class BoxifyVisitor : IVisitor<Body>
    {
        private readonly BoundingBoxVisitor _boundingBoxVisitor = new BoundingBoxVisitor();

        public Body Visit(CompoundBody compoundBody)
        {
            var boxifyParts = new Body[compoundBody.Parts.Count];
            for (int i = 0; i < compoundBody.Parts.Count; i++)
            {
                boxifyParts[i] = compoundBody.Parts[i].Accept(this);
            }
            
            return new CompoundBody(boxifyParts);
        }

        public Body Visit(Cylinder cylinder)
        {
            return GetBoundingBox(cylinder);
        }

        public Body Visit(Ball ball)
        {
            return GetBoundingBox(ball);
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            return GetBoundingBox(rectangularCuboid);
        }

        private RectangularCuboid GetBoundingBox(Body body)
        {
            return body.Accept(_boundingBoxVisitor);
        }
    }

    public static class BodiesExtensions
    {
        public static Bounds GetBounds(this CompoundBody compoundBody)
        {
            var minBound = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
            var maxBound = new Vector3(double.MinValue, double.MinValue, double.MinValue);
            var boundingBoxVisitor = new BoundingBoxVisitor();

            foreach (Body part in compoundBody.Parts)
            {
                RectangularCuboid boundingBox = part.Accept(boundingBoxVisitor);
                Bounds bounds = boundingBox.GetBounds();
                
                maxBound = Math3.GetMaxVector3Values(bounds.Max, maxBound);
                minBound = Math3.GetMinVector3Values(bounds.Min, minBound);
            }

            return new Bounds(minBound, maxBound);
        }

        private static Bounds GetBounds(this RectangularCuboid cuboid)
        {
            var minBound = new Vector3(
                cuboid.Position.X - cuboid.SizeX / 2, 
                cuboid.Position.Y - cuboid.SizeY / 2, 
                cuboid.Position.Z - cuboid.SizeZ / 2);
            var maxBound = new Vector3(
                cuboid.Position.X + cuboid.SizeX / 2, 
                cuboid.Position.Y + cuboid.SizeY / 2, 
                cuboid.Position.Z + cuboid.SizeZ / 2);
            
            return new Bounds(minBound, maxBound);
        }
    }

    public static class Math3
    {
        public static Vector3 GetMaxVector3Values(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                Math.Max(vector1.X, vector2.X),
                Math.Max(vector1.Y, vector2.Y),
                Math.Max(vector1.Z, vector2.Z)
                );
        }
        
        public static Vector3 GetMinVector3Values(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                Math.Min(vector1.X, vector2.X),
                Math.Min(vector1.Y, vector2.Y),
                Math.Min(vector1.Z, vector2.Z)
            );
        }
    }
}