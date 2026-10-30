using System;
using System.Linq;

namespace NeuralNetworks_1;

class nnMath
{
    public double[] MatrixMult(double[,] mat1, int mat1Width, double[] mat2)
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

    public double[] ActivationFunc(double[] inputs) => inputs.Select(x => 1.0 / (1.0 + Math.Exp(x))).ToArray();
}