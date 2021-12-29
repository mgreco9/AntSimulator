using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class BehaviourTreeBase : MonoBehaviour
    {
        protected Node root = null;
        protected Dictionary<String, object> sharedParameters = new Dictionary<string, object>();

        protected void Start()
        {
            root = SetupTree();
            root.Initialize();
        }

        private void Update()
        {
            sharedParameters.Clear();

            if (root == null)
                return;

            NodeState aiState = root.Evaluate();

            if (aiState == NodeState.RUNNING)
                return;

            if (aiState == NodeState.SUCCESS || aiState == NodeState.FAILURE)
                root.Reset();
        }

        protected abstract Node SetupTree();

    }
}
