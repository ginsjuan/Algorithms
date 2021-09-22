using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LRUCache
{
    class Program
    {
        public class LRUCache
        {
            //Set the constrains first
            const short CAPACITY_MAX = 3000;

            /// <summary>
            /// Represents the capacity of this cache
            /// </summary>
            private readonly int _capacity;

            /// <summary>
            /// Represents a Memory HashMap data-structure n(1) access
            /// </summary>
            private Dictionary<int, LinkedListNode<KeyValuePair<int, int>>> _dict;
            /// <summary>
            /// Represents a double linked list
            /// </summary>
            private LinkedList<KeyValuePair<int, int>> _linkedList;



            public LRUCache(int capacity)
            {
                //Do validations for capacity constrain
                if (capacity >= CAPACITY_MAX)
                    throw new ArgumentException($"{nameof(capacity)} cannot be greater or equals than 3000");

                if (capacity < 1)
                    throw new ArgumentException($"{nameof(capacity)} cannot be less or equals than 1");

                //Initalize the capacity
                _capacity = capacity;
                //Initalize data-structures
                _dict = new Dictionary<int, LinkedListNode<KeyValuePair<int, int>>>();
                _linkedList = new LinkedList<KeyValuePair<int, int>>();
            }

            public int Get(int key)
            {

                if (_dict.ContainsKey(key))
                {
                    //Find the node by key, remove it from the list, and put it on the first node of the list
                    var node = _dict[key];

                    if (node != null)
                    {
                        var value = node.Value.Value;
                        _linkedList.Remove(node);
                        _linkedList.AddFirst(node);

                        return value;
                    }
                }

                //by default return -1, if node is not found
                return -1;

            }

            public void Put(int key, int value)
            {
                //Check if the linked-list is over capacity
                if (_linkedList.Count == _capacity)
                {
                    //get the last index and pop last item from list
                    var lastKey = _linkedList.Last.Value.Key;
                    _linkedList.RemoveLast();
                    //remove the popped key from dictionary
                    _dict.Remove(lastKey);

                    //then add the new value to the front
                    LinkedListNode<KeyValuePair<int, int>> node = CreateNode(key, value);
                    _dict.Add(key, node);
                }
                else
                {
                    //if the capacity is not reached, then add the value to the front 
                    LinkedListNode<KeyValuePair<int, int>> node = CreateNode(key, value);
                    _dict.Add(key, node);
                }

            }

            /// <summary>
            /// Create a linked-list node with a key-value pair reference
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private LinkedListNode<KeyValuePair<int, int>> CreateNode(int key, int value)
            {
                KeyValuePair<int, int> kvp = new KeyValuePair<int, int>(key, value);
                var node = new LinkedListNode<KeyValuePair<int, int>>(kvp);
                _linkedList.AddFirst(node);
                return node;
            }
        }
       
        /**
         * Your LRUCache object will be instantiated and called as such:
         * LRUCache obj = new LRUCache(capacity);
         * int param_1 = obj.Get(key);
         * obj.Put(key,value);
         */

        public static void Main(string[] args)
        {
            try
            {              

                LRUCache lRUCache = new LRUCache(2);
                lRUCache.Put(1, 0); // cache is {1=1}
                lRUCache.Put(2, 2); // cache is {1=1, 2=2}                                
                Assert.AreEqual(1, lRUCache.Get(1), "expected 1"); // return 1
                lRUCache.Put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
                Assert.AreEqual(-1, lRUCache.Get(2), "expected -1");    // returns -1 (not found)
                lRUCache.Put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
                Assert.AreEqual(-1, lRUCache.Get(1),  "expected -1");   // return -1 (not found)
                Assert.AreEqual(3, lRUCache.Get(3), "expected 3");    // return 3
                Assert.AreEqual(4, lRUCache.Get(4), "expected 4");   // return 4
            }
            catch(InvalidOperationException inv)
            {
                Console.WriteLine(inv.Message, inv.StackTrace);
            }
            catch (AssertFailedException asf)
            {
                Console.WriteLine($"LRUCache didn't return the rigth value {asf.Message}");
            }
        }
    }
}
