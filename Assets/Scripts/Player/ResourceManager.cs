﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public delegate void ParamChanging(Param p);
    public event ParamChanging OnParamChanging;

    public PathGame pathGame;
    private List<Param> parameters = new List<Param>();
    private ChainPlayer chainPlayer;
    private ChainPlayer ChainPlayer
    {
        get
        {
            if (chainPlayer == null)
            {
                chainPlayer = FindObjectOfType<ChainPlayer>();
            }
            return chainPlayer;
        }
    }

    public void SetParam(string name, float value)
    {
        parameters.First(x => x.name == name).PValue = value;
        OnParamChanging.Invoke(parameters.First(x => x.name == name));
    }

    public void SetParam(string name, string evaluationString, List<Param> evaluationParameters = null)
    {

        float value = 0;
        value = ExpressionSolver.CalculateFloat(evaluationString, evaluationParameters);


        parameters.First(x => x.name == name).PValue = value;
        OnParamChanging.Invoke(parameters.First(x => x.name == name));
    }

    public float GetParam(string name)
    {
        return parameters.First(x => x.name == name).PValue;
    }

    private void Start()
    {
        if (pathGame)
        {
            parameters = pathGame.parameters;
            foreach (Param p in parameters)
            {
                p.pValue = 0;
                if (ChainPlayer)
                {
                    p.OnParamActivation += ChainPlayer.PlayChain;
                }
                OnParamChanging.Invoke(p);
            }
            GUIDManager.SetInspectedGame(pathGame);
        }
    }

    private void OnDestroy()
    {
        foreach (Param p in parameters)
        {
            if (ChainPlayer)
            {
                p.OnParamActivation -= ChainPlayer.PlayChain;
            }
        }
    }
}
