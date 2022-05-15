using System;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    private int[] m_scores;

    public Score()
    {
        m_scores = new int[Enum.GetValues(typeof(PlayerTeam)).Length];
        Reset();
    }

    public void Reset()
    {
        for(int i = 0; i < m_scores.Length; ++i)
        {
            m_scores[i] = 0;
        }
    }

    public int GetScore(PlayerTeam team)
    {
        return m_scores[(int)team];
    }

    public void AddScore(PlayerTeam team, int score)
    {
        m_scores[(int)team] += score;
    }

    public PlayerTeam GetLeadingTeam(PlayerTeam biddingTeam)
    {
        int team1Score = GetScore(PlayerTeam.Team1);
        int team2Score = GetScore(PlayerTeam.Team2);

        if(team2Score == team1Score)
        {
              if(biddingTeam == PlayerTeam.Team1)  
              {
                  return PlayerTeam.Team2;
              }
              else
              {
                  return PlayerTeam.Team1;
              }
        }
        else if(team1Score > team2Score)
        {
            return PlayerTeam.Team1;
        }
        return PlayerTeam.Team2;
    }
}