#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks_1
{
    class nn3S
    {
        double[,] wih, who;
        int inodes, hnodes, onodes;
        double[] hidden_inputs;
        double[] hidden_outputs;
        double[] hidden_errors;
        double[] final_inputs;
        double[] final_outputs;
        double[] output_errors;

        nnMath nnMathO = new nnMath();

        double learningRate = 0;


        public double[,] WIH { get { return wih; } }
        public double[,] WHO { get { return who; } }
        public double[] Hidden_inputs { get { return hidden_inputs; } }
        public double[] Hidden_outputs { get { return hidden_outputs; } }
        public double[] Hidden_errors { get { return hidden_errors; } }
        public double[] Final_inputs { get { return final_inputs; } }
        public double[] Final_outputs { get { return final_outputs; } }
        public double[] Output_errors { get { return output_errors; } }

        public nn3S(int inodes, int hnodes, int onodes, double learningRate)
        {
            this.inodes = inodes;
            this.hnodes = hnodes;
            this.onodes = onodes;
            this.learningRate = learningRate;

            createWeightMatrizes();
        }

        private void createWeightMatrizes()
        {
            wih = new double[inodes, hnodes];
            who = new double[hnodes, onodes];

            wih[0, 0] = 0.9; //spalte, reihe
            wih[1, 0] = 0.3;
            wih[2, 0] = 0.4;
            wih[0, 1] = 0.2;
            wih[1, 1] = 0.8;
            wih[2, 1] = 0.2;
            wih[0, 2] = 0.1;
            wih[1, 2] = 0.5;
            wih[2, 2] = 0.6;

            who[0, 0] = 0.3;
            who[1, 0] = 0.7;
            who[2, 0] = 0.5;
            who[0, 1] = 0.6;
            who[1, 1] = 0.5;
            who[2, 1] = 0.2;
            who[0, 2] = 0.8;
            who[1, 2] = 0.1;
            who[2, 2] = 0.9;
        }

        public void queryNN(double[] inputs)
        {
            hidden_inputs = new double[hnodes];
            hidden_inputs = nnMathO.matrixMult(wih, inputs);

            hidden_outputs = new double[hnodes];
            hidden_outputs = nnMathO.activationFunc(hidden_inputs);

            final_inputs = new double[onodes];
            final_inputs = nnMathO.matrixMult(who, hidden_outputs);

            final_outputs = new double[hnodes];
            final_outputs = nnMathO.activationFunc(final_inputs);

        }

        public void setWHOMatrix() { }
        public void setWIHMatrix() { }
        public void train(double[] inputs, double[] targets)
        {
            queryNN(inputs);

            output_errors = targets.Zip(final_outputs, (x, y) => x - y).ToArray();
            hidden_errors = nnMathO.matrixMult(nnMathO.transpose(who), output_errors);

            double[] deltaOut = nnMathO.vectorMult(output_errors,
                                 nnMathO.vectorMult(final_outputs, final_outputs.Select(x => 1 - x).ToArray()));
            who = nnMathO.matrixSum(who, nnMathO.matrixScale(nnMathO.fullMatrixMult(hidden_outputs, deltaOut), learningRate));

            // 5: dasselbe für die versteckte Schicht
            double[] deltaHid = nnMathO.vectorMult(hidden_errors , nnMathO.vectorMult(hidden_outputs, hidden_outputs.Select(x => 1 - x).ToArray()));
            wih = nnMathO.matrixSum(wih, nnMathO.matrixScale(nnMathO.fullMatrixMult(inputs, deltaHid), learningRate));
        }
        public void updateWeights() { }
    }
}