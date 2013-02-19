using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic 
{
    class FuzzyLogicClass 
    {
        static void Main(string[] args) 
        {
            mainLogic carLogic = new mainLogic();
            while (carLogic.requestData())
            {
                carLogic.checkRules();
            }   
        }
    }

    class mainLogic {
        private double centroid;
        private double distanceX;
        private double delta;

        private string charInput;

        //Distance value parameters
        private struct distanceParam
        {
            //trapezoidal
            public double a1, b1, c1, d1, 
            //triangular
            a2, b2, c2,
            //trapezoidal
            a3, b3, c3, d3;
        };

        //Delta value parameters
        private struct deltaParam
        {
            // trapezoidal
            public double a1, b1, c1, d1,
            //triangular
            a2, b2, c2,
            //trapezoidal
            a3, b3, c3, d3;
        };

        distanceParam distStruct;
        deltaParam deltaStruct;

        public mainLogic()
        {
            //trapezoidal
            distStruct.a1 = -10;
            distStruct.b1 = -10;
            distStruct.c1 = -8;
            distStruct.d1 = -2;

            //triangular
            distStruct.a2 = -6;
            distStruct.b2 = 0;
            distStruct.c2 = 6;

            //trapezoidal
            distStruct.a3 = 2;
            distStruct.b3 = 8;
            distStruct.c3 = 10;
            distStruct.d3 = 10;

            //trapezoidal
            deltaStruct.a1 = -5;
            deltaStruct.b1 = -5;
            deltaStruct.c1 = -4;
            deltaStruct.d1 = -0.5;

            //triangular
            deltaStruct.a2 = -3;
            deltaStruct.b2 = 0;
            deltaStruct.c2 = 3;

            //trapezoidal
            deltaStruct.a3 = 0.5;
            deltaStruct.b3 = 4;
            deltaStruct.c3 = 5;
            deltaStruct.d3 = 5;

        }

        ~mainLogic(){}

        public bool requestData()
        {
            Console.WriteLine("To exit the application use 'Q' ");
            Console.Write("Please enter car distance from the racing line ");
            charInput = Console.ReadLine();

            if (charInput == "Q")
                return false;
            else
                distanceX = Double.Parse(charInput);

            Console.Write("Please enter car distance change rate ");
            delta = Double.Parse(Console.ReadLine());
            return true;
        }

        //Aux min function for three values
        public static double Min(double x, double y, double z)
        {
            return Math.Min(x, Math.Min(y, z));
        }

        //trapezoidal MF = max(min((x-a)/(b-a),1,(d-x)/(d-c)),0)
        public double trapezoidalMF(double x, double a, double b, double c, double d)
        {
            return Math.Max( Min( (x-a)/(b-a), 1, (d-x)/(d-c) ), 0 );
        }

        //triangular MF = max(min((x-a)/(b-a),(c-x)/(c-b)),0)
        public double triangularMF(double x, double a, double b, double c)
        {
            return Math.Max( Math.Min( (x-a)/(b-a), (c-x)/(c-b) ), 0 );
        }

        public void checkRules()
        {
            //Linguistic values
            double distLeft = 0, distZero = 0, distRight = 0, deltaLeft = 0, deltaZero = 0, deltaRight = 0;

            //////////Fuzzification of input values

            //If distance is left
            if (distanceX >= distStruct.a1 && distanceX <= distStruct.d1)
            {
                distLeft = trapezoidalMF(distanceX, distStruct.a1, distStruct.b1, distStruct.c1, distStruct.d1);
            }

            //if distance is zero
            if (distanceX >= distStruct.a2 && distanceX <= distStruct.c2)
            {
                distZero = triangularMF(distanceX, distStruct.a2, distStruct.b2, distStruct.c2);
            }

            //if distance is right
            if (distanceX >= distStruct.a3 && distanceX <= distStruct.d3)
            {
                distRight = trapezoidalMF(distanceX, distStruct.a3, distStruct.b3, distStruct.c3, distStruct.d3);
            }

            //If delta is left
            if (delta >= deltaStruct.a1 && delta <= deltaStruct.d1)
            {
                deltaLeft = trapezoidalMF(delta, deltaStruct.a1, deltaStruct.b1, deltaStruct.c1, deltaStruct.d1);
            }

            //if delta is zero
            if (delta >= deltaStruct.a2 && delta <= deltaStruct.c2)
            {
                deltaZero = triangularMF(delta, deltaStruct.a2, deltaStruct.b2, deltaStruct.c2);
            }

            //if delta is right
            if (delta >= deltaStruct.a3 && delta <= deltaStruct.d3)
            {
                deltaRight = trapezoidalMF(delta, deltaStruct.a3, deltaStruct.b3, deltaStruct.c3, deltaStruct.d3);
            }

            Console.WriteLine("Distance left membership {0} Distance zero membership {1} Distance right membership {2}", distLeft, distZero, distRight);
            Console.WriteLine("Delta left membership {0} Delta zero membership {1} Delta right membership {2}", deltaLeft, deltaZero, deltaRight);

            //Fuzzy rule set
            //If distance is Left and delta is Left then Steer is Right 
            //If distance is Left and delta is Right then Steer is Zero 
            //If distance is Left and delta is Zero then Steer is Right

            //If distance is Right and delta is Left then Steer is Zero 
            //If distance is Right and delta is Right then Steer is Left 
            //If distance is Right and delta is Zero then Steer is Left 

            //If distance is Zero and delta is Left then Steer is Right 
            //If distance is Zero and delta is Right then Steer is Left 
            //If distance is Zero and delta is Zero then Steer is Zero 

            //////////Aggregation step



            //////////Defuzzification into crisp values

        }     
    }

}
