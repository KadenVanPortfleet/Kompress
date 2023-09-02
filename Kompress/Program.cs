using System.Collections;
//Another test change
namespace Kompress
{


    class Kompress
    {



        static void Main()
        {

            Kompress komp = new Kompress();
            string[] source = Environment.GetCommandLineArgs();

            string data = File.ReadAllText(source[2]);
            data = data;


            if (source[1] == "decompress")
            {
                Decompress decomp = new Decompress();
                decomp.Decompressor();
            }
            else if (source[1] == "compress")
            {


                int[] arrFreq = new int[256];

                arrFreq = komp.Count(data);

                Hashtable hashCodes = new Hashtable();



                Heap heap = new Heap();
                heap.genNodes(arrFreq);

                Node root = new Node();
                root = heap.getRoot();
                //Console.WriteLine("Root Frequency: " + root.Right.Right.Left.Left.Val);

                //Console.WriteLine("" + root.Freq);
                //Console.WriteLine("" + root.Left.Right.Val);

                for (int i = 0; i < 255; i++)
                {
                    if (data.Contains((char)i) == true)
                    {
                        heap.getCode((char)i, root);
                        //Console.WriteLine(heap.foundNode.Code);
                        Console.WriteLine("Code for " + (char)i + ": " + heap.genCode(heap.foundNode));
                        hashCodes.Add((char)i, heap.genCode(heap.foundNode));
                    }

                }

                komp.Compress(data, hashCodes, arrFreq);
                //Console.WriteLine(root.Right.Right.Left.Val);

            }
        }

        public void Compress(string data, Hashtable hashCodes, int[] arrFreq)
        {
            string finalBits = null;
            for (int i = 0; i < data.Length; i++)
            {
                finalBits = finalBits + hashCodes[data[i]].ToString();
                //Console.WriteLine(finalBits);
            }
            Console.WriteLine("FINAL BITS: \n" + finalBits);
            int extra = finalBits.Length % 8;
            for (int i = 0; i < extra; i++)
            {
                finalBits += 0;
            }
            int numberofBytes = finalBits.Length / 8;
            Byte[] bytes = new Byte[numberofBytes];
            for (int i = 0; i < numberofBytes; i++)
            {
                bytes[i] = Convert.ToByte(finalBits.Substring(8 * i, 8), 2);
            }
            string ENCODED = null;
            for (int i = 0; i < bytes.Length; i++)
            {
                ENCODED = ENCODED + (Convert.ToChar(Convert.ToInt32(bytes[i])));
            }

            Console.WriteLine("DEBUG: " + ENCODED);
            

            File.WriteAllText("test.txt", Convert.ToString(ENCODED));


            File.WriteAllText("key.txt", null);
            for (int i = 0; i < 255; i++)
            {
                if (arrFreq[i] != 0)
                {
                    File.AppendAllText("key.txt",i + "." + arrFreq[i].ToString() + (char)45);

                }

            }


        }




        public int[] Count(string data)
        {
            int[] arrFreq = new int[256];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < 255; j++)
                {
                    if (data[i] == (char)j)
                    {
                        arrFreq[j] = arrFreq[j] + 1;
                    }
                }


            }

            return arrFreq;
        }


        public class Node
        {
            public Node Left
            { get; set; }
            public Node Right
            { get; set; }
            public Node Parent { get; set; }

            public char Val { get; set; }
            public int Freq { get; set; }
            public int Code { get; set; }

        }

        public class Heap
        {
            public Node Root { get; set; }
            public ArrayList arrNode = new ArrayList();
            public Node foundNode = new Node();
            public string[] arrCodes = new string[256];

            public void genNodes(int[] arrFreq)
            {
                //Root.Freq = Enumerable.Sum(arrFreq);
                for (int i = 0; i < arrFreq.Length; i++)
                {
                    Node newNode = new Node();
                    newNode.Val = (char)i;
                    newNode.Freq = arrFreq[i];
                    arrNode.Add(newNode);
                }
                //Node n = new Node();
                //n = (Node)arrNode[97];
                //Console.WriteLine("TEST: " + n.Val + n.Freq);



                Branches(sortNodes(arrNode));




            }

            public Queue sortNodes(ArrayList arrNode)
            {
                Node temp;
                //Console.WriteLine("TEST: " + n.Val + n.Freq);
                for (int i = 0; i < arrNode.Count; i++)
                {
                    for (int j = i + 1; j < arrNode.Count; j++)
                    {
                        Node n1 = new Node();
                        Node n2 = new Node();
                        n1 = (Node)arrNode[i];
                        n2 = (Node)arrNode[j];

                        if (n1.Freq > n2.Freq)
                        {


                            arrNode[j] = n1;
                            arrNode[i] = n2;

                            //Console.WriteLine("TEST" + n1.Val + n1.Freq);

                        }
                        else if (n2.Val == null)
                        {
                            arrNode[j] = n1;
                            arrNode[i] = n2;
                        }
                    }
                }
                //Cleans the array of 0 frequency Nodes and Sends to Queue
                Queue nodeQueue = new Queue();
                for (int i = 0; i < arrNode.Count; i++)
                {
                    Node tempNode = (Node)arrNode[i];
                    if (tempNode.Freq != 0)
                    {
                        //Console.WriteLine(tempNode.Val + " " + tempNode.Freq);
                        nodeQueue.Enqueue(tempNode);
                    }
                }
                //while (nodeQueue.Count != 0)
                //{

                //    Node n3 = new Node();
                //    n3 = (Node)nodeQueue.Dequeue();
                //    Console.WriteLine(n3.Val + " " + n3.Freq);


                //}

                return nodeQueue;
            }

            public void Branches(Queue nodeQueue)
            {
                //Console.WriteLine(nodeQueue.Count);
                while (nodeQueue.Count > 1)
                {
                    Node n1 = (Node)nodeQueue.Dequeue();
                    Node n2 = (Node)nodeQueue.Dequeue();

                    Node newNode = new Node();
                    newNode.Freq = n1.Freq + n2.Freq;
                    newNode.Left = n1;
                    n1.Code = 0;
                    newNode.Right = n2;
                    n2.Code = 1;
                    n1.Parent = newNode;
                    n2.Parent = newNode;
                    nodeQueue.Enqueue(newNode);
                    ArrayList arrList = new ArrayList();

                    while (nodeQueue.Count != 0)
                    {
                        arrList.Add(nodeQueue.Dequeue());
                    }
                    
                    Branches(sortNodes(arrList));
                }
                if (nodeQueue.Count == 1)
                {
                    Node n3 = new Node();
                    n3 = (Node)nodeQueue.Dequeue();


                    Root = n3;

                }
                else
                {

                }



            }

            public Node getRoot()
            {
                return Root;
            }

            public void getCode(char ch, Node current)
            {

                //Console.WriteLine("Searching:" + current.Val);
                if (current.Val == ch)
                {
                    foundNode = current;
                    //Console.WriteLine("FOUND: " + current.Val);



                }
                if (current.Left != null)
                {
                    getCode(ch, current.Left);

                }

                if (current.Right != null)
                {
                    getCode(ch, current.Right);

                }

            }

            public string genCode(Node find)
            {
                string code = null;
                Node current = new Node();
                current = find;
                while (current.Parent != null)
                {
                    code = code + current.Code;
                    current = current.Parent;
                }
                Stack inverString = new Stack();

                for (int i = 0; i < code.Length; i++)
                {
                    inverString.Push(code[i]);
                }
                code = null;
                while (inverString.Count != 0)
                {
                    code = code + inverString.Pop();
                }

                //code = code.PadLeft(8, (char)48);
                //Console.WriteLine("CODE: " + code);
                return code;
            }




        }






    }
}






