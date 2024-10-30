using SestiKupi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SestiKupi.Core
{
    public class RandomNumGenerator : Singleton<RandomNumGenerator>
    {
        private System.Random randomNumGen;
        protected override void Awake()
        {
            base.Awake();
            randomNumGen = new System.Random();
        }
        public List<int> GenerateNums(int count, int minVal, int maxVal)
        {
            List<int> newList = new List<int>();
            int counter = 0;
            while (counter < count)
            {
                var randNum = randomNumGen.Next(minVal, maxVal);
                newList.Add(randNum);
                
                counter++;
            }

            return newList;
        }
        public List<int> GenerateUniqueNums(int count, int minVal, int maxVal)
        {
            if(count > (maxVal-minVal))
            {
                Debug.LogError("Invalid input for random num generator");
                return null;
            }

            List<int> newList = new List<int>();
            int counter = 0;
            while(counter < count)
            {
                var randNum = randomNumGen.Next(minVal, maxVal);
                if(!newList.Contains(randNum))
                {
                    newList.Add(randNum);
                    counter++;
                }
            }

            return newList;
        }
    }
}
