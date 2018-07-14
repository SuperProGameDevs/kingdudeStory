using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PluggableAI
{
    [CreateAssetMenu(menuName = "PluggableAI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;

        //public void UpdateState(StateController controller)
        //{
        //    this.DoActions(controller);
        //}

        //private void DoActions(StateController controller)
        //{
        //    foreach (Action action in actions) {
        //        action.Act(controller);
        //    }
        //}
    }
}
