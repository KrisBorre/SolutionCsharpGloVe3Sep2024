namespace LibraryDimensionalityReducer1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class Converter
    {
        public static double[][] ToJaggedArray(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[][] jaggedArray = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = matrix[i, j];
                }
            }
            return jaggedArray;
        }
    }
}
