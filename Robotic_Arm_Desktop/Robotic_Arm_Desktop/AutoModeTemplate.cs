﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Robotic_Arm_Desktop
{
    static class AutoModeTemplate
    {
        const int sleepTime = 15;
        const int incrementation = 1;

        public static async Task StartTemplateAsync(List<string> commands,Movemend movemend, _3Dmodel model,TextBox textBox)
        {
            int reps;
            if (textBox.Text == "inf")
            {
                reps = int.MaxValue;
            }
            else
            {
                reps = Convert.ToInt32(textBox.Text);
            }

            for (int i = 0; i < reps; i++)
            {
                foreach (var command in commands)
                {
                    if (Global.stop == false)
                    {
                        List<double> instructions = Deserialization(command);

                        await Task.Run(() => {
                            bool allMotorsOnPositions = false;
                            do
                            {
                                if (Global.stop == false)
                                {
                                    allMotorsOnPositions = Moving(instructions, movemend);
                                            Thread.Sleep(sleepTime / Convert.ToInt16(instructions[8]));
                                }
                                else
                                {
                                    break;
                                }

                            } while (allMotorsOnPositions == false);

                            if (instructions[6] == 1) //wait for trigger
                            {
                                while (true)
                                {
                                    if (Global.stop == false)
                                    {
                                        if (Global.triggered == false)
                                        {
                                            Thread.Sleep(300);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            Thread.Sleep(Convert.ToInt16(instructions[7]));
                        });
                    }
                    else
                    {
                        break;
                    }
                }
                if (Global.stop == true)
                {
                    break;
                }

                textBox.Text = (reps-i).ToString();
            }
            
            Global.autoModeRunning = false;
        }

        static bool Moving(List<double> instruction, Movemend movemend)
        {
            int onPosition = 0;

            if (instruction[1] > movemend.elbow0.AngleInPWM)
            {
                if (movemend.elbow0.AngleInPWM+incrementation > instruction[1])
                {
                    movemend.elbow0.Update(instruction[1],1);
                }
                else
                {
                    movemend.elbow0.Update(movemend.elbow0.AngleInPWM + incrementation, 1);
                }
            }
            else if (instruction[1] < movemend.elbow0.AngleInPWM)
            {
                if (movemend.elbow0.AngleInPWM - incrementation < instruction[1])
                {
                    movemend.elbow0.Update(instruction[1], 1);
                }
                else
                {
                    movemend.elbow0.Update(movemend.elbow0.AngleInPWM - incrementation, 1);
                }
            }
            else
            {
                onPosition++;
            }


            if (instruction[2] > movemend.elbow1.AngleInPWM)
            {
                if (movemend.elbow1.AngleInPWM + incrementation > instruction[2])
                {
                    movemend.elbow1.Update(instruction[2], 1);
                }
                else
                {
                    movemend.elbow1.Update(movemend.elbow1.AngleInPWM + incrementation, 1);
                }

            }
            else if (instruction[2] < movemend.elbow1.AngleInPWM)
            {
                if (movemend.elbow1.AngleInPWM - incrementation < instruction[2])
                {
                    movemend.elbow1.Update(instruction[2], 1);
                }
                else
                {
                    movemend.elbow1.Update(movemend.elbow1.AngleInPWM - incrementation, 1);
                }
            }
            else
            {
                onPosition++;
            }

            if (instruction[3] > movemend.elbow2.AngleInPWM)
            {
                if (movemend.elbow2.AngleInPWM + incrementation > instruction[3])
                {
                    movemend.elbow2.Update(instruction[3], 1);
                }
                else
                {
                    movemend.elbow2.Update(movemend.elbow2.AngleInPWM + incrementation, 1);
                }

            }
            else if (instruction[3] < movemend.elbow2.AngleInPWM)
            {
                if (movemend.elbow2.AngleInPWM - incrementation < instruction[3])
                {
                    movemend.elbow2.AngleInPWM = instruction[3];
                }
                else
                {
                    movemend.elbow2.Update(movemend.elbow2.AngleInPWM - incrementation, 1);
                }
            }
            else
            {
                onPosition++;
            }

            if (instruction[4] > movemend.griperRotation.AngleInPWM)
            {
                if (movemend.griperRotation.AngleInPWM + incrementation > instruction[4])
                {
                    movemend.griperRotation.AngleInPWM = instruction[4];
                }
                else
                {
                    movemend.griperRotation.Update(movemend.griperRotation.AngleInPWM + incrementation, 1);
                }

            }
            else if (instruction[4] < movemend.griperRotation.AngleInPWM)
            {
                if (movemend.griperRotation.AngleInPWM - incrementation < instruction[4])
                {
                    movemend.griperRotation.AngleInPWM = instruction[4];
                }
                else
                {
                    movemend.griperRotation.Update(movemend.griperRotation.AngleInPWM - incrementation, 1);
                }
            }
            else
            {
                onPosition++;
            }

            if (instruction[5] > movemend.griper.AngleInPWM)
            {
                if (movemend.griper.AngleInPWM + incrementation > instruction[5])
                {
                    movemend.griper.AngleInPWM = instruction[5];
                }
                else
                {
                    movemend.griper.Update(movemend.griper.AngleInPWM + incrementation, 1);

                }

            }
            else if (instruction[5] < movemend.griper.AngleInPWM)
            {
                if (movemend.griper.AngleInPWM - incrementation < instruction[5])
                {
                    movemend.griper.AngleInPWM = instruction[5];
                }
                else
                {
                    movemend.griper.Update(movemend.griper.AngleInPWM - incrementation, 1);
                }
            }
            else
            {
                onPosition++;
            }

            if (instruction[0] > movemend.baseMovemend.AngleInPWM)
            {
                if (movemend.baseMovemend.AngleInPWM + incrementation > instruction[0])
                {
                    movemend.baseMovemend.AngleInPWM = instruction[0];
                }
                else
                {
                    movemend.baseMovemend.Update(movemend.griper.AngleInPWM + incrementation, 1);
                }

            }
            else if (instruction[0] < movemend.baseMovemend.AngleInPWM)
            {
                if (movemend.baseMovemend.AngleInPWM - incrementation < instruction[0])
                {
                    movemend.baseMovemend.AngleInPWM = instruction[0];
                }
                else
                {
                    movemend.baseMovemend.Update(movemend.baseMovemend.AngleInPWM - incrementation, 1);

                }
            }
            else
            {
                onPosition++;
            }

            if (onPosition == 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static List<double> Deserialization(string command)
        {
            List<string> instructionsRaw = command.Split('*').ToList();
            List<double> instructions = new List<double>();

            foreach (var item in instructionsRaw)
            {
                instructions.Add(Convert.ToDouble(item));
            }

            return instructions;
        }

    }
}
