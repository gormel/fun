using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Fun1.Model
{
    public class Field
    {
        private class RevertOption : FieldOption
        {
            public RevertOption(Field field) : base(field)
            {
            }

            public override bool Enabled => base.Enabled && Field.mStates.Count > 0;
            public override int Cost => Math.Max(Field.Score / 2 + 1, 10);
            public override string Name => "Revert";
            public override void Fire()
            {
                Field.Score -= Cost;

                var newState = Field.mStates.Pop();
                Field.FieldData = newState;
            }
        }

        private class RestartOption : FieldOption
        {
            public RestartOption(Field field) : base(field)
            {
            }

            public override int Cost => Field.Score;
            public override string Name => "Restart";
            public override void Fire()
            {
                Square[,] startData = null;
                while (Field.mStates.Count > 0)
                {
                    startData = Field.mStates.Pop();
                }

                if (startData == null)
                    return;

                Field.Score = 0;

                Field.FieldData = startData;
            }
        }

        private class RegenerateOption : FieldOption
        {
            public RegenerateOption(Field field) : base(field)
            {
            }

            public override int Cost => Field.Score;
            public override string Name => "Regenerate";
            public override void Fire()
            {
                Field.Score = 0;
                Field.Generate();
            }
        }

        private readonly int mWidth;
        private readonly int mHeight;

        public int Score { get; private set; } = 0;

        public List<FieldOption> Options { get; }  

        public Square[,] FieldData { get; private set; }

        private readonly Stack<Square[,]> mStates = new Stack<Square[,]>();

        public Field(int w, int h)
        {
            Options = new List<FieldOption>
            {
                new RevertOption(this),
                new RestartOption(this),
                new RegenerateOption(this)
            };
            mWidth = w;
            mHeight = h;
            Generate();
        }

        private void Generate()
        {
            FieldData = new Square[mWidth, mHeight];
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    FieldData[i, j] = new Square();
                }
            }
            
        }

        public void ProcessClick(int x, int y)
        {
            var copy = new Square[mWidth, mHeight];
            Array.Copy(FieldData, copy, FieldData.Length);

            var removed = RemoveSame(x, y, FieldData[x, y].Color);

            if (removed < 2)
                FieldData = copy;
            else
            {
                Score += (int) Math.Pow(2, removed - 1);
                mStates.Push(copy);
            }

            ShiftDown();

            ShiftRight();
        }

        private int RemoveSame(int x, int y, int lastColor)
        {
            if (x < 0 || x >= mWidth || y < 0 || y >= mHeight)
                return 0;
            
            if (FieldData[x, y] == null)
                return 0;

            if (FieldData[x, y].Color != lastColor)
                return 0;

            var savedColor = FieldData[x, y].Color;

            FieldData[x, y] = null;

            var result = 1;

            result += RemoveSame(x - 1, y, savedColor);
            result += RemoveSame(x, y - 1, savedColor);
            result += RemoveSame(x + 1, y, savedColor);
            result += RemoveSame(x, y + 1, savedColor);

            return result;
        }

        private void ShiftDown()
        {
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = mHeight - 1; j >= 0; j--)
                {
                    var newJ = j;
                    while (newJ < mHeight - 1 && FieldData[i, newJ + 1] == null)
                    {
                        newJ++;
                    }

                    if (j == newJ)
                        continue;

                    FieldData[i, newJ] = FieldData[i, j];
                    FieldData[i, j] = null;
                }
            }
        }

        private void ShiftRight()
        {
            for (int i = mWidth - 1; i >= 0; i--)
            {
                bool empty = FieldData[i, mHeight - 1] == null;

                if (!empty)
                    continue;

                var sourceI = i;
                while (sourceI >= 0 && FieldData[sourceI, mHeight - 1] == null)
                {
                    sourceI--;
                }

                if (sourceI < 0)
                    return;

                for (int j = 0; j < mHeight; j++)
                {
                    FieldData[i, j] = FieldData[sourceI, j];
                    FieldData[sourceI, j] = null;
                }
            }
        }
    }
}
