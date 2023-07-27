using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace SpiningCube 
{

    class cuabeRot
    {

        static int width = 100, height = 44;
        static float A, B, C;
        static float cubeWidth = 20;
        static float[] zBuffer = new float[160 * 44];
        static char[] buffer = new char[160 * 44];
        static int backgroundASCIICode = ' ';
        static int distanceFromCam = 100;
        static float horizontalOffset;
        static float K1 = 40f;
        static float incrementalSpeed = 0.6f;
        static float x, y, z;
        static float ozz;
        static int xp, yp;
        static int idx;

        static void Main(string[] args)
        {
            Console.WriteLine("\x1b[2J");
            while (true)
            {


                Array.Fill(buffer, (char)backgroundASCIICode);
                Array.Clear(zBuffer, 0, zBuffer.Length);

                horizontalOffset = -6 * cubeWidth + 15;

                //First cube
                for (float CubeX = -cubeWidth;
                    CubeX < cubeWidth;
                    CubeX += incrementalSpeed)
                {
                    for (float CubeY = -cubeWidth;
                        CubeY < cubeWidth;
                        CubeY += incrementalSpeed)
                    {
                        CalculateSurface(CubeX, CubeY, -cubeWidth, '#');
                        CalculateSurface(cubeWidth, CubeY, CubeX, '*');
                        CalculateSurface(-cubeWidth, CubeY, -CubeX, '$');
                        CalculateSurface(-CubeX, CubeY, cubeWidth, '+');
                        CalculateSurface(CubeX, -cubeWidth, CubeY, '@');
                        CalculateSurface(CubeX, cubeWidth, CubeY, '~');
                    }
                }

                Console.SetCursorPosition(0, 0);
                for (int k = 0; k < width * height; k++)
                {
                    Console.Write(buffer[k]);
                    if ((k + 1) % width == 0)
                        Console.WriteLine();
                }

                A += 0.05f;
                B += 0.05f;
                C += 0.01f;
                Thread.Sleep(8 * 2);

            }

        }


        static void CalculateSurface(float CubeX, float CubeY, float CubeZ, char ch)
        {
            x = calculateRotX((int)CubeX, (int)CubeY, (int)CubeZ);
            y = calculateRotY((int)CubeX, (int)CubeY, (int)CubeZ);
            z = calculateRotZ((int)CubeX, (int)CubeY, (int)CubeZ) + distanceFromCam;

            ozz = 1 / z;
            xp = (int)(width / 2 + horizontalOffset + K1 * ozz * x * 2);
            yp = (int)(height / 2 + K1 * ozz * y);

            idx = xp + yp * width;
            if (idx >= 0 && idx < width * height)
            {
                buffer[idx] = ch;
                zBuffer[idx] = ozz;
            }

        }
        static float calculateRotX(int i, int j, int k)
        {
            float rotX =
                j * MathF.Sin(A) * MathF.Sin(B) * MathF.Cos(C)
                - k * MathF.Cos(A) * MathF.Sin(B) * MathF.Cos(C)
                + j * MathF.Cos(A) * MathF.Sin(C)
                + k * MathF.Sin(A) * MathF.Sin(C)
                //+k *MathF.Cos(A) * MathF.Sin(C)
                //-j *MathF.Sin(A) * MathF.Sin(C)
                + i * MathF.Cos(B) * MathF.Cos(C);
            return rotX;
        }

        static float calculateRotY(int i, int j, int k)
        {
            float rotY =
                +j * MathF.Cos(A) * MathF.Cos(C)
                + k * MathF.Sin(A) * MathF.Cos(C)
                //+ k * MathF.Cos(A) * MathF.Cos(C)
                // - j * MathF.Sin(A) * MathF.Cos(C)
                - j * MathF.Sin(A) * MathF.Sin(B) * MathF.Sin(C)
                + k * MathF.Cos(A) * MathF.Sin(B) * MathF.Sin(C)
                - i * MathF.Cos(B) * MathF.Sin(C);
            return rotY;

        }

        static float calculateRotZ(int i, int j, int k)
        {
            float rotZ =
                +k * MathF.Cos(A) * MathF.Sin(B)
                - j * MathF.Sin(A) * MathF.Cos(B)
                + i * MathF.Sin(B);

            return rotZ;

        }

    }
}



