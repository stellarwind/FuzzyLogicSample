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

        //Aux min function for three values
        public static double Min(double x, double y, double z)
        {
            return Math.Min(x, Math.Min(y, z));
        }

        //Aux min function for three values
        public static double Max(double x, double y, double z)
        {
            return Math.Max(x, Math.Max(y, z));
        }

        //Aux clamp function
        public static double Clamp (double val, double min, double max) 
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }

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

        private struct steerParam
        {
            //trapezoidal
            public double a1, b1, c1, d1,
            //triangular
            a2, b2, c2,
            //trapezoidal
            a3, b3, c3, d3;
        };

        distanceParam distStruct;
        deltaParam deltaStruct;
        steerParam steerStruct;

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

            //trapezoidal
            steerStruct.a1 = -1;
            steerStruct.b1 = -1;
            steerStruct.c1 = -0.8;
            steerStruct.d1 = -0.1;

            //triangular
            steerStruct.a2 = -0.6;
            steerStruct.b2 = 0;
            steerStruct.c2 = 0.6;

            //trapezoidal
            steerStruct.a3 = 0.1;
            steerStruct.b3 = 0.8;
            steerStruct.c3 = 1;
            steerStruct.d3 = 1;


        }

        ~mainLogic(){}

        public bool requestData()
        {
            Console.WriteLine("To exit the application use 'Q' ");
            Console.Write("Please enter car distance from the racing line (-10 -- 10)");
            charInput = Console.ReadLine();

            if (charInput == "Q")
                return false;
            else
                distanceX = Double.Parse(charInput);

            Console.Write("Please enter car distance change rate (-5 -- 5)");
            delta = Double.Parse(Console.ReadLine());

            distanceX = Clamp(distanceX, distStruct.a1, distStruct.d3);
            delta = Clamp(delta, deltaStruct.a1, deltaStruct.d3);
            return true;
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

            double steerRight1 = 0, steerRight2 = 0, steerRight3 = 0, steerZero1 = 0, steerZero2 = 0, 
            steerZero3 = 0, steerLeft1 = 0, steerLeft2 = 0, steerLeft3 = 0;

            double steerLeft = 0, steerRight = 0, steerZero = 0;

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

            //////////Aggregation step

            //9 (3x3) Fuzzy rule set
            //If distance is Left and delta is Left then Steer is Right
            //If distance is Left and delta is Zero then Steer is Right
            //If distance is Left and delta is Right then Steer is Zero 

            if (distLeft > 0 && deltaLeft > 0)
                steerRight1 = Math.Min(distLeft, deltaLeft);
            if (distLeft > 0 && deltaZero > 0)
                steerRight1 = Math.Min(distLeft, deltaZero);
            if (distLeft > 0 && deltaRight > 0)
                steerZero1 = Math.Min(distLeft, deltaRight);

            //If distance is Right and delta is Left then Steer is Zero 
            //If distance is Right and delta is Zero then Steer is Left 
            //If distance is Right and delta is Right then Steer is Left 
            if (distRight > 0 && deltaLeft > 0)
                steerZero2 = Math.Min(distRight, deltaLeft);
            if (distRight > 0 && deltaZero > 0)
                steerLeft2 = Math.Min(distRight, deltaZero);
            if (distRight > 0 && deltaRight > 0)
                steerLeft2 = Math.Min(distRight, deltaRight);

            //If distance is Zero and delta is Left then Steer is Right
            //If distance is Zero and delta is Zero then Steer is Zero
            //If distance is Zero and delta is Right then Steer is Left 
            if (distZero > 0 && deltaLeft > 0)
                steerRight3 = Math.Min(distZero, deltaLeft);
            if (distZero > 0 && deltaZero > 0)
                steerZero3 = Math.Min(distZero, deltaZero);
            if (distZero > 0 && deltaRight > 0)
                steerLeft3 = Math.Min(distZero, deltaRight);

            steerLeft = Max(steerLeft1, steerLeft2, steerLeft3);
            steerZero = Max(steerZero1, steerZero2, steerZero3);
            steerRight = Max(steerRight1, steerRight2, steerRight3);

            //////////Defuzzification into crisp values
            //Getting centroid out of steer member function and max points values. Uses RSS statistic method.
            centroid = ((steerStruct.c1 * steerLeft) + (steerStruct.b2 * steerZero) + (steerStruct.b3 * steerRight)) / (steerLeft + steerZero + steerRight);

            Console.WriteLine("Distance left membership {0} Distance zero membership {1} Distance right membership {2}", distLeft, distZero, distRight);
            Console.WriteLine("Delta left membership {0} Delta zero membership {1} Delta right membership {2}", deltaLeft, deltaZero, deltaRight);
            Console.WriteLine("Steer left membership {0} Steer zero membership {1} Steer right membership {2}", steerLeft, steerZero, steerRight);
            Console.WriteLine("Steer crisp value {0}", centroid);
        }     
    }

}
