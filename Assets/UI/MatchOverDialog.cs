using System;
using JungleCore;
using UnityEngine;

namespace UI
{
    public class MatchOverDialog : MonoBehaviour
    {
        [SerializeField] private GameObject victory;
        [SerializeField] private GameObject defeat;
        private Master     _master;
        private void Awake()
        {
            _master = FindObjectOfType<Master>();
        }

        private void Start()
        {
            _master.game.matchOver += Show;
        }    
        private void OnDisable()
        {
            _master.game.matchOver -= Show;
        }

        private void Show(Side side)
        {
            switch (side)
            {
                case Side.Player:
                    victory.SetActive(true);
                    break; 
                case Side.Enemy:
                    defeat.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }
    }
}
