using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SuperJson;

namespace VectorEditor
{
    public enum ShapeType
    {
        Line,
        Circle,
        Square,
        Triangle,
        None
    }

    public enum EditMode
    {
        Create,
        Move,
        Remove,
        None
    }

    public partial class Form1 : Form
    {
        private EditMode mMode = EditMode.None;
        private ShapeType mShape;
        EditPoint mEditPoint;

        string fileName;

        private List<Shape> mShapes = new List<Shape>();

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (mMode)
            {
                case EditMode.Create:
                    var newShape = Shape.CreateShape(mShape, e.X, e.Y);
                    if (newShape != null)
                        mShapes.Add(newShape);
                    break;
                case EditMode.Move:
                    foreach (var pt in mShapes.SelectMany(s => s.EditPoints()))
                    {
                        if (e.Location.DistanceTo(pt.Point) < 5)
                        {
                            mEditPoint = pt;
                            break;
                        }
                    }
                    break;
                case EditMode.Remove:
                    foreach (var shape in mShapes.ToList())
                    {
                        var rect = AroundRectangle(shape.EditPoints());
                        if (rect.Contains(e.Location))
                            mShapes.Remove(shape);
                    }
                    break;
            }
            panel1.Invalidate();
        }

        private Rectangle AroundRectangle(List<EditPoint> pts)
        {
            var result = new Rectangle(pts[0].X, pts[0].Y, 0, 0);
            foreach (var pt in pts)
            {
                if (pt.X > result.Right)
                    result.Width += pt.X - result.Right;
                if (pt.X < result.Left)
                {
                    result.Width += result.Left - pt.X;
                    result.X = pt.X;
                }
                if (pt.Y > result.Bottom)
                    result.Height += pt.Y - result.Bottom;
                if (pt.Y < result.Top)
                {
                    result.Height += result.Top - pt.Y;
                    result.Y = pt.Y;
                }
            }
            return result;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mEditPoint = null;
            panel1.Invalidate();
        }

        private void toolStripButton5_Click(object sender, System.EventArgs e)
        {
            mMode = EditMode.Create;
        }

        private void toolStripButton6_Click(object sender, System.EventArgs e)
        {
            mMode = EditMode.Move;
        }

        private void toolStripButton7_Click(object sender, System.EventArgs e)
        {
            mMode = EditMode.Remove;
        }

        private void toolStripButton1_Click(object sender, System.EventArgs e)
        {
            mShape = ShapeType.Line;
        }

        private void toolStripButton2_Click(object sender, System.EventArgs e)
        {
            mShape = ShapeType.Circle;
        }

        private void toolStripButton3_Click(object sender, System.EventArgs e)
        {
            mShape = ShapeType.Square;
        }

        private void toolStripButton4_Click(object sender, System.EventArgs e)
        {
            mShape = ShapeType.Triangle;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mMode == EditMode.Move && mEditPoint != null)
            {
                mEditPoint.MoveTo(e.X, e.Y);
            }
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(panel1.BackColor);
            foreach (var shape in mShapes)
            {
                shape.Render(e.Graphics);
            }

            if (mMode == EditMode.Move)
            {
                foreach (var pt in mShapes.SelectMany(s => s.EditPoints()))
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), pt.X - 5, pt.Y - 5, 10, 10);
                }
            }

            if (mMode == EditMode.Remove)
            {
                foreach (var rect in mShapes.Select(s => AroundRectangle(s.EditPoints())))
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red), rect);
                }
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mShapes.Clear();
            fileName = "";
        }

        private void сохранитьКекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseFileName();
            Save();
        }

        private void ChooseFileName()
        {
            using (var dialog = new SaveFileDialog() { Filter = "Vector picture|*.pct" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = dialog.FileName;
            }
        }

        private void Save()
        {
            var ser = new SuperJsonSerializer();
            var result = ser.Serialize(mShapes);
            File.WriteAllText(fileName, result);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileName))
                ChooseFileName();
            Save();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog() { Filter = "Vector picture|*.pct" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = dialog.FileName;

                var ser = new SuperJsonSerializer();
                mShapes = (List<Shape>)ser.Deserialize(File.ReadAllText(fileName));
            }
        }
    }
}
