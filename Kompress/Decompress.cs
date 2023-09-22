using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompress
{
    class Decompress
    {
        Kompress.Node root = new Kompress.Node();
        Kompress.Heap heap = new Kompress.Heap();
        public void Decompressor(string[] sourceD)
        {
            
            int[] arrFreq = new int[256];

            string key = File.ReadAllLines(sourceD[3])[0];
            string input = File.ReadAllText(sourceD[2]);

            string[] arrInput = key.Split((char)45);
            //Console.WriteLine(arrInput[16]);

            for (int i = 0; i < arrInput.Length - 1; i++)
            {
                string[] datasplit = arrInput[i].Split(".");
                Console.WriteLine(datasplit[0]);
                Console.WriteLine(datasplit[1]);
                
                arrFreq[(Int32.Parse(datasplit[0]))] = Int32.Parse(datasplit[1]);
            }


            Kompress komp = new Kompress();
            
            string[] source = Environment.GetCommandLineArgs();


            
            heap.genNodes(arrFreq);

            
            root = heap.getRoot();


            Console.WriteLine("Root Frequency: " + root.Freq);
            string bits;
            bits = parseEncryption(input);
            Console.WriteLine("DEBUG before shaving: " + bits);

            string[] shaveArr = arrInput[arrInput.Length - 1].Split("|");
            Console.WriteLine(shaveArr[1]);
            int shaveAmt = Int32.Parse(shaveArr[1]);
            Console.WriteLine(shaveAmt);
            bits = bits.Remove(bits.Length - shaveAmt);

            Console.WriteLine("DEBUG after shaving: " + bits);
            Console.WriteLine(bitInterpreter(bits));
            //Console.WriteLine(root.Right.Right.Right.Right.Val);

            File.WriteAllText("decrypt.txt", bitInterpreter(bits));




        }

        public string parseEncryption(string input)
        {
            string bits = null;
            string strTemp = null;
            string temp = null;
            temp = input;
            //Console.WriteLine("Debug2: " + input);
            for (int i = 0; i < temp.Length; i++)
            {
                //Console.WriteLine("Debug3: " + temp[i]);
                strTemp = Convert.ToString(Convert.ToInt32(temp[i]), 2);
                strTemp = strTemp.PadLeft(8, (char)(byte)48);

                //Console.WriteLine("DEBUG4: " + strTemp);

                bits = bits + strTemp;
                
            }

            return bits;
        }

        public string bitInterpreter(string bits)
        {
            string FINAL = null;
            Kompress.Node current = root;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == (char)49)
                {
                    current = current.Right;
                }
                else if (bits[i] == (char)48)
                { 
                    current = current.Left;
                }
                if (current.Left == null && current.Right == null)
                {
                    FINAL = FINAL + current.Val;
                    current = root;
                }
            }



            return FINAL;
        
        }


    }
}
