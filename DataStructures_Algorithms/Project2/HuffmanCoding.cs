using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures_Algorithms.Project1;
using System.IO;

namespace DataStructures_Algorithms.Project2
{
    public class Node
    {
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }
        public char Symbol { get; set; }
        public int Frequency { get; set; }
    }

    public class HuffmanCoding
    {
        private List<Node> listOfNodes = new List<Node>();
        private Node Root { get; set; }


        Node Add(Node left, Node right)
        {
            // Combines the two nodes together into one with a symbol of '₸', indicating that it's a parent node
            left = new Node
            {
                Symbol = '₸',
                Frequency = left.Frequency + right.Frequency,
                LeftChild = left,
                RightChild = right
            };

            return left;
        }
        private void Build(Vector<char> input)
        {
            // Creates a list of nodes that holds the values of Char and Int as symbol and frequency
            foreach (var i in input)
            {

                foreach (var node in listOfNodes)
                {
                    // If it's an empty line, break
                    if (i == '\0')
                        break;

                    // If the node already exists append the frequency
                    if (node.Symbol == i)
                    {
                        node.Frequency++;
                        break;
                    }

                    // If were at the end of the list and the symbol doesn't already exist then create a new node
                    if (listOfNodes.IndexOf(node) == listOfNodes.Count() - 1)
                    {
                        listOfNodes.Add(new Node
                        {
                            Symbol = i,
                            Frequency = 1,
                            LeftChild = null,
                            RightChild = null
                        });
                        break;
                    }
                }

                // If we are at the beginning of the list, create a node
                if (listOfNodes.Count == 0)
                {
                    listOfNodes.Add(new Node
                    {
                        Symbol = i,
                        Frequency = 1,
                        LeftChild = null,
                        RightChild = null
                    });
                }
            }
        }

        public string binary = "";
        private void Traverse(Node node, TextWriter tw)
        {
            if (node == null)
                return;

            if (node.LeftChild != null)
            {
                binary += "0";

                // If the left child is not a parent node add to a list of binarys
                // along with its symbol and how many times it has been traversed
                if (node.LeftChild.Symbol != '₸')
                    binaryFreq.Add(node.LeftChild.Symbol, binary);

                Traverse(node.LeftChild, tw);
                binary = binary.Remove(binary.Length - 1);
            }


            if (node.RightChild != null)
            {
                binary += "1";

                if (node.RightChild.Symbol != '₸')
                    binaryFreq.Add(node.RightChild.Symbol, binary);

                Traverse(node.RightChild, tw);
                binary = binary.Remove(binary.Length - 1);
            }
        }

        public Vector<string> Encode(Vector<char> input)
        {
            Build(input);

            /* 
                1. Find the 2 lowest values and remove them from frequency
                2. Add() function is executed sending the nodes with the lowest value
                3. Add the newly created node to the list
            */


            while (listOfNodes.Count() != 1)
            {
                var lowest = listOfNodes.Aggregate((l, r) => l.Frequency < r.Frequency ? l : r);
                listOfNodes.Remove(lowest);

                var secondLowest = listOfNodes.Aggregate((l, r) => l.Frequency < r.Frequency ? l : r);
                listOfNodes.Remove(secondLowest);

                Root = Add(lowest, secondLowest);
                listOfNodes.Add(Root);    // Adds the current node to the list
            }

            Traverse(Root, Console.Out);


            /*
                Read every char in the input and if it matches the binary of the traversed node, add it a list
            */
            Vector<string> encoded = new Vector<string>(input.Count);
            foreach (var value in input)
            {
                foreach (var str in binaryFreq)
                {
                    if (str.Key == value)
                        encoded.Add(str.Value);
                }
            }

            return encoded;
        }

        public Vector<char> Decode(Vector<string> input)
        {
            var result = new Vector<char>();

            string inUsage = "";
            foreach (var inp in input)
            {
                inUsage += inp;

                foreach (var str in binaryFreq)
                {
                    if (str.Value == inUsage)
                    {
                        inUsage = "";
                        result.Add(str.Key);
                        continue;
                    }
                }
            }

            return result;
        }

        private Dictionary<char, string> _BinaryFreq = new Dictionary<char, string>();
        public Dictionary<char, string> binaryFreq
        {
            get { return _BinaryFreq; }
            set { _BinaryFreq = value; }
        }
    }
}
