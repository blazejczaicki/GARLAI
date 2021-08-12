using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    private DT_IGameTreeNode root;

    public void CreateShooterModeTree()
    {
        
    }

    public void CreateWalkModeTree()
    {
        DT_Decision isSouroundedDec = new IsSouroundedDecision();
        DT_Decision isEnemiesTooCloseDec = new IsEnemiesTooCloseDecision();
        DT_Decision isSafetyAroundDec = new IsSafetyAroundDecision();
        DT_Decision isWallDec = new IsWallOnRoadDecision();
        DT_Decision isCornerDecision = new IsCornerDecision();
        DT_Action makeGoBackAct = new MakeGoBackAction();
        DT_Action makeBreakThroughAct = new MakeBreakThroughSouroundingAction();
        DT_Action makeGoToSafetyPositionAct = new MakeGoToSafetyPositionAction();
        DT_Action makeStayHereAct = new MakeStayHereAction();
        DT_Action makeGoBackWallAct = new MakeGoBackWallAction();
        DT_Action makeGoCenterAction = new MakeGoCenterAction();

        isEnemiesTooCloseDec.TrueNode = isSouroundedDec;
        isEnemiesTooCloseDec.FalseNode = isSafetyAroundDec;
        root = isEnemiesTooCloseDec;
        isSafetyAroundDec.TrueNode = makeStayHereAct;
        isSafetyAroundDec.FalseNode = makeGoToSafetyPositionAct;
        isSouroundedDec.TrueNode = makeBreakThroughAct;
        isSouroundedDec.FalseNode = isWallDec;
        isWallDec.TrueNode = isCornerDecision;
        isWallDec.FalseNode = makeGoBackAct;
        isCornerDecision.TrueNode =makeGoCenterAction;
        isCornerDecision.FalseNode =makeGoBackWallAct;
    }

    public Queue<Vector3> MakeDecision(PlayerAI player, ref string actName)
    {
        DT_IGameTreeNode actionNode = root.MakeDecision(player);
        return actionNode.MakeAction(player, ref actName);
    }
}
