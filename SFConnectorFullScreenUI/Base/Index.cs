using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public sealed class Index : IEquatable<Index>
    {
        public int X;
        public int Y;

        public Index(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool IsIndexValid(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }
            return true;
        }

        //public static bool IsIndexValid(int x, int y)
        //{
        //    if (x < 0 || x >= MainWindow.NumberOfCellsHorizontal ||
        //        y < 0 || y >= MainWindow.NumberOfCellsVertical)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public static bool IsXOutOfUpperBound(Index index)
        //{
        //    if (index.X >= CellsController.NumberOfCellsHorizontal)
        //        return true;
        //    return false;
        //}

        //public static bool IsYOutOfUpperBound(Index index)
        //{
        //    if (index.Y >= CellsController.NumberOfCellsVertical)
        //        return true;
        //    return false;
        //}

        //public static bool IsOutOfUpperBound(Index index)
        //{
        //    if (IsXOutOfUpperBound(index) ||
        //        IsYOutOfUpperBound(index))
        //        return true;
        //    return false;
        //}

        public static Index GetIndexFromPosition(int x, int y, int caseWidth, int caseHeight)
        {
            var indexX = x / caseWidth;
            var indexY = y / caseHeight;
            //indexX = Math.Min(indexX, MainWindow.NumberOfCellsHorizontal - 1);
            //indexY = Math.Min(indexY, MainWindow.NumberOfCellsVertical - 1);
            indexX = Math.Max(indexX, 0);
            indexY = Math.Max(indexY, 0);
            return new Index(indexX, indexY);
        }

        public static (int x, int y) GetPositionFromIndex(Index index, int caseWidth, int caseHeight)
        {
            if (index == null) return (0, 0);
            return (index.X * caseWidth, index.Y * caseHeight);
        }

        public static Index GetCellCornerPoint(double x, double y, int caseWidth, int caseHeight)
        {
            return GetCaseCornerPoint((int)x, (int)y, caseWidth, caseHeight);
        }
        public static Index GetCaseCornerPoint(int x, int y, int caseWidth, int caseHeight)
        {
            x = (x / caseWidth) * caseWidth;
            y = (y / caseHeight) * caseHeight;
            return new Index(x, y);
        }

        public static (int x, int y) GetPositionInsideCaseFromGlobal(int x, int y, int caseWidth, int caseHeight)
        {
            var index = GetIndexFromPosition(x, y, caseWidth, caseHeight);
            x = x - (index.X * caseWidth);
            y = y - (index.Y * caseHeight);
            return (x, y);
        }

        public bool Equals(Index other)
        {
            if (other == null) return false;
            if (this.X == other.X && this.Y == other.Y) return true;
            return false;
        }

        public override string ToString()
        {
            return X.ToString()+Y.ToString();
        }
    }
}
