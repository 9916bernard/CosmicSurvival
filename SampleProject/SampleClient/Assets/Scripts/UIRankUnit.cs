using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRankUnit : MonoBehaviour
{
    [SerializeField] private Text _Text_Rank = null;
    [SerializeField] private Text _Text_Username = null;
    [SerializeField] private Text _Text_Record = null;
    [SerializeField] private Image _Icon_Rank = null;

    public void SetRank(int rank, string username, int record)
    {
        _Text_Rank.text = rank.ToString();
        _Text_Username.text = username;
        _Text_Record.text = FormatRecord(record);

        if (rank > 3)
        {
            // Make icon transparent
            _Icon_Rank.color = new Color(_Icon_Rank.color.r, _Icon_Rank.color.g, _Icon_Rank.color.b, 0);
        }
        else
        {
            // Make icon visible
            _Icon_Rank.color = new Color(_Icon_Rank.color.r, _Icon_Rank.color.g, _Icon_Rank.color.b, 1);

            switch (rank)
            {
                case 1:
                    _Icon_Rank.color = new Color(1.0f, 0.843f, 0.0f); // Gold
                    break;
                case 2:
                    _Icon_Rank.color = new Color(0.753f, 0.753f, 0.753f); // Silver
                    break;
                case 3:
                    _Icon_Rank.color = new Color(0.803f, 0.498f, 0.196f); // Bronze
                    break;
            }
        }
    }

    private string FormatRecord(int record)
    {
        TimeSpan time = TimeSpan.FromSeconds(record);
        return time.ToString(@"mm\:ss");
    }
}
