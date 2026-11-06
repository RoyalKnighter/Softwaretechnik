using System;
using System.Linq;

namespace NeuralNetworks_1;

class nnMath
{
    public double[] matrixMult(double[,] mat1, double[] mat2)
    {
        double[] result = new double[mat2.Length];

        for (int y = 0; y < result.Length; y++)
        {
            for (int x = 0; x < mat2.Length; x++)
            {
                result[y] += mat1[x, y] * mat2[x];
            }
        }

        return result;
    }

    public double[] activationFunc(double[] inputs) => inputs.Select(x => 1.0 / (1.0 + Math.Exp(-x))).ToArray();


    // Multiply a column vector and a row vector, return a matrix
    public double[,] fullMatrixMult(double[] vec1, double[] vec2)
    {
        double[,] result = new double[vec1.Length, vec2.Length];
        for (int i = 0; i < vec1.Length; i++)
            for (int j = 0; j < vec2.Length; j++)
                result[i, j] = vec1[i] * vec2[j];
        return result;
    }
    // Sum two matrices element-wise
    public double[,] matrixSum(double[,] mat1, double[,] mat2)
    {
        int rows = mat1.GetLength(0), cols = mat1.GetLength(1);
        double[,] result = new double[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = mat1[i, j] + mat2[i, j];
        return result;
    }
    public double[] vectorMult(double[] vec1, double[] vec2)
    {
        double[] result = new double[vec1.Length];
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] * vec2[i];
        return result;
    }
    public double[,] transpose(double[,] inputMat)
    {
        int rows = inputMat.GetLength(0), cols = inputMat.GetLength(1);
        double[,] result = new double[cols, rows];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[j, i] = inputMat[i, j];
        return result;
    }
    // Multiply element-wise a matrix by a scaling factor
    public double[,] matrixScale(double[,] inputMat, double scale)
    {
        int rows = inputMat.GetLength(0), cols = inputMat.GetLength(1);
        double[,] result = new double[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = inputMat[i, j] * scale;
        return result;
    }
}