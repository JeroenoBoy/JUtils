using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JUtils
{
    public enum ProductionStatus { Processing, Success, Failure }
    public delegate void ProductionStepCallback<T>(FactoryProcessor<T> processor, Action<bool> onDone);
    
    
    public class Factory<T>
    {
        private readonly Dictionary<string, ProductionStep<T>> _productionSteps = new();

        
        public void Add(string key, ProductionStep<T> step) => _productionSteps.Add(key, step);
        public void Remove(string key) => _productionSteps.Remove(key);

        
        public FactoryProcessor<T> Run()
        {
            return new FactoryProcessor<T>(_productionSteps.Values.OrderByDescending(it => it.priority).Select(it => it.processor).GetEnumerator());
        }
    }
    
    
    public class FactoryProcessor<T>
    {
        public T value;

        public ProductionStatus productionStatus { get; private set; }
        public bool isDone => productionStatus != ProductionStatus.Processing;


        public FactoryProcessor(IEnumerator<ProductionStepCallback<T>> callBacks)
        {
            productionStatus = ProductionStatus.Processing;
            RunNext(true);

            void RunNext(bool completed)
            {
                if (!completed) {
                    productionStatus = ProductionStatus.Failure;
                    return;
                }

                if (!callBacks.MoveNext()) {
                    productionStatus = ProductionStatus.Success;
                    return;
                }
                
                callBacks.Current!.Invoke(this, RunNext);
            }
        }

        
        public IEnumerator CompletionRoutine()
        {
            yield return new WaitUntil(() => isDone);
        }


        public void CallWhenDone(Action)
        {
            
        }
    }
    
    
    public class ProductionStep<T>
    {
        public readonly ProductionStepCallback<T> processor;
        public readonly int priority;

        
        public ProductionStep(int priority, ProductionStepCallback<T> processor)
        {
            this.processor = processor;
            this.priority = priority;
        }
    }
}