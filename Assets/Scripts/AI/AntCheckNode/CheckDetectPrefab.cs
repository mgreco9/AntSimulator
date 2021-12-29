using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourTree;

class CheckDetectPrefab : Node
{
    private DetectorManager dinput;
    private DetectorType detectorType;

    public CheckDetectPrefab(DetectorManager dinput, DetectorType detectorType)
    {
        this.dinput = dinput;
        this.detectorType = detectorType;
    }

    public override NodeState Evaluate()
    {
        if (dinput.GetDetectedPrefab(detectorType) is null)
            return NodeState.FAILURE;

        return NodeState.SUCCESS;
    }
}
