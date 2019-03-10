﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace SignalProcessingMethods
{
    public class SignalOperations
    {
        public List<double> AddSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i=0; i<signal1.Count; i++)
            {
                result[i] = signal1[i] + signal2[i];
            }

            return result;
            ;
        }

        public List<double> SubtractSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result[i] = signal1[i] - signal2[i];
            }

            return result;
            ;
        }

        public List<double> MultiplySignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result[i] = signal1[i] * signal2[i];
            }

            return result;
            ;
        }

        public List<double> DivideSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal1.Count; i++)
            {
                result[i] = signal1[i] / signal2[i];
            }

            return result;
            ;
        }
    }
}