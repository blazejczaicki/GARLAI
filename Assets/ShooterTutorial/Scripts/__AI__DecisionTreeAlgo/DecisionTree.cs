using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    private DT_IGameTreeNode root;

    public void CreateTmpTree()
    {
        DT_Decision isSouroundedDec = new IsSouroundedDecision();
        DT_Decision isEnemiesTooCloseDec = new IsEnemiesTooCloseDecision();
        DT_Action makeGoBackAct = new MakeGoBackAction();
        DT_Action makeBreakThroughAct = new MakeBreakThroughSouroundingAction();
        DT_Action makeGoToSafetyPositionAct = new MakeGoToSafetyPositionAction();


        isEnemiesTooCloseDec.TrueNode = isSouroundedDec;
        isEnemiesTooCloseDec.FalseNode = makeGoToSafetyPositionAct;
        root = isEnemiesTooCloseDec;
        isSouroundedDec.TrueNode = makeBreakThroughAct;
        isSouroundedDec.FalseNode = makeGoBackAct;
    }

    public Queue<Vector3> MakeDecision(PlayerAI player)
    {
        DT_IGameTreeNode actionNode = root.MakeDecision(player);
        return actionNode.MakeAction(player);
    }
}
