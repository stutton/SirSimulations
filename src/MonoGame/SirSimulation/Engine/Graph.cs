using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirSimulation.Engine
{
    public class Graph
    {
        private Vector2 _scale = new Vector2(1.0f, 1.0f);
        private BasicEffect _effect;
        private short[] _lineListIndices;
        private short[] _triangleStripeIndices;

        public Graph(GraphicsDevice graphicsDevice, Point size, float maxValue)
        {
            _effect = new BasicEffect(graphicsDevice)
            {
                View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreateOrthographicOffCenter(0, (float)graphicsDevice.Viewport.Width, (float)graphicsDevice.Viewport.Height, 0, 1.0f, 1000.0f),
                World = Matrix.Identity,
                VertexColorEnabled = true
            };

            MaxValue = maxValue;
            Size = size;

            Type = GraphType.Line;
        }

        public GraphType Type { get; set; }
        public Vector2 Position { get; set; }
        public Point Size { get; set; }
        public float MaxValue { get; }
        public BasicEffect Effect => _effect;

        public void Draw(List<(float value, Color color)> values)
        {
            if (values.Count < 2)
            {
                return;
            }

            var xScale = Size.X / (float)values.Count;
            var yScale = Size.Y / MaxValue;

            _scale = new Vector2(xScale, yScale);
            UpdateWorld();

            if(Type == GraphType.Line)
            {
                var pointList = new VertexPositionColor[values.Count];
                var i = 0;
                foreach (var (value, color) in values)
                {
                    pointList[i] = new VertexPositionColor(
                        new Vector3( i, value < MaxValue ? value : MaxValue, 0), color);
                    i++;
                }
                DrawLineList(pointList);
            }
            else if (Type == GraphType.Fill)
            {
                var pointList = new VertexPositionColor[values.Count * 2];
                var i = 0;
                foreach (var (value, color) in values)
                {
                    pointList[i * 2 + 1] = new VertexPositionColor(
                        new Vector3( i, value < MaxValue ? value : MaxValue, 0), color);
                    pointList[i * 2] = new VertexPositionColor(new Vector3(i, 0, 0), color);
                    i++;
                }
                DrawTriangleStrip(pointList);
            }
        }

        public void Draw(List<float> values, Color color)
        {
            if (values.Count < 2)
            {
                return;
            }

            var xScale = Size.X / (float)values.Count;
            var yScale = Size.Y / MaxValue;

            _scale = new Vector2(xScale, yScale);
            UpdateWorld();

            if(Type == GraphType.Line)
            {
                var pointList = new VertexPositionColor[values.Count];
                var i = 0;
                foreach (var value in values)
                {
                    pointList[i] = new VertexPositionColor(
                        new Vector3( i, value < MaxValue ? value : MaxValue, 0), color);
                    i++;
                }
                DrawLineList(pointList);
            }
            else if (Type == GraphType.Fill)
            {
                var pointList = new VertexPositionColor[values.Count * 2];
                var i = 0;
                foreach (var value in values)
                {
                    pointList[i * 2 + 1] = new VertexPositionColor(
                        new Vector3( i, value < MaxValue ? value : MaxValue, 0), color);
                    pointList[i * 2] = new VertexPositionColor(new Vector3(i, 0, 0), color);
                    i++;
                }
                DrawTriangleStrip(pointList);
            }
        }

        private void UpdateWorld()
        {
            _effect.World =
                Matrix.CreateScale(_scale.X, _scale.Y, 1.0f)
                * Matrix.CreateRotationX(MathHelper.Pi) // Flip graph so that higher values are above
                * Matrix.CreateTranslation(new Vector3(this.Position, 0));
        }

        private void DrawLineList(VertexPositionColor[] pointList)
        {
            if (_lineListIndices == null || _lineListIndices.Length != ((pointList.Length * 2) - 2))
            {
                _lineListIndices = new short[(pointList.Length * 2) - 1];
                for (var i = 0; i < pointList.Length - 1; i++)
                {
                    _lineListIndices[i * 2] = (short)(i);
                    _lineListIndices[(i * 2) + 1] = (short)(i + 1);
                }
            }

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    pointList,
                    0,
                    pointList.Length,
                    _lineListIndices,
                    0,
                    pointList.Length - 1);
            }
        }

        private void DrawTriangleStrip(VertexPositionColor[] pointList)
        {
            if (_triangleStripeIndices == null || _triangleStripeIndices.Length != pointList.Length)
            {
                _triangleStripeIndices = new short[pointList.Length];
                for (int i = 0; i < pointList.Length; i++)
                {
                    _triangleStripeIndices[i] = (short)i;
                }
            }

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    pointList,
                    0,
                    pointList.Length,
                    _triangleStripeIndices,
                    0,
                    pointList.Length - 2);
            }
        }

        public enum GraphType
        {
            Line,
            Fill
        }
    }
}
