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
    public double[,] fullMatrixMult(double[] vec1, double[] vec2) { }
    // Sum two matrices element-wise
    public double[,] matrixSum(double[,] mat1, double[,] mat2) { }
    public double[] vectorMult(double[] vec1, double[] vec2) { }
    public double[,] transpose(double[,] inputMat) { }
    // Multiply element-wise a matrix by a scaling factor
    public double[,] matrixScale(double[,] inputMat, double scale) { }
}