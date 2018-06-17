using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    /// <summary>
    /// 
    /// </summary>
    public class BTN
    {
        int val;
        BTN left;
        BTN right;

        /// <summary>
        /// Compare two Binary trees 
        /// </summary>
        /// <param name="p">First tree</param>
        /// <param name="q">Second tree</param>
        /// <returns>true is equal and false is not equal</returns>
        public bool isSameTree(BTN p, BTN q)
        {
            if (p == null && q == null)
            {
                return true;
            }
            else if (p == null || q == null)
            {
                return false;
            }

            bool isLeftSame = isSameTree(p.left, q.left);
            bool isRightSame = isSameTree(p.right, q.right);

            return p.val == q.val && isLeftSame && isRightSame;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Element
    {
        int num;
        String name;
        int age;
        const int SearchAge = 20;

        /// <summary>
        /// Filters List collection by given SearchAge
        /// </summary>
        /// <param name="inputElements">List<Element> </param>
        /// <returns>List<Element></returns>
        public List<Element> Filter(List<Element> inputElements)
        {
            if (inputElements == null) return null;

            var target = new List<Element>();

            foreach (var element in inputElements)
            {
                if (element.age > SearchAge)
                {
                    if (target.Count == 0)
                    {
                        target.Add(element);
                    }
                    else
                    {
                        bool IsExists = false;

                        foreach(var subElement in target)
                        {
                            if (element.num == subElement.num)
                            {
                                IsExists = true;
                                break;
                            }
                        }

                        if (!IsExists)
                        {
                            target.Add(element);
                        }
                    }
                }
            }

            return target;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ssss = CalculateMD5Hash("buxoro@kasaba.uz");
        }

        public static string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }
    }
}
