using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame.Board
{
    public class PositionSpace : MonoBehaviour
    {
        private int qty = 0;
        public enum PositionActions
        {
            Start,
            End,
            GoForward,
            MissTurn,
            GoBackwards,
            None
        }

        [SerializeField] private Material exampleMaterial;

        [SerializeField] private PositionActions _thisPosition = PositionActions.None;

        public void SetPositionAction(bool start, bool end)
        {
            if (start)
            {
                _thisPosition = PositionActions.Start;
                return;
            }
            if (end)
            {
                _thisPosition = PositionActions.End;
                return;
            }

            _thisPosition = GetRandomAction();

            SetNewQty();
        }

        private void SetNewQty()
        {
            
            switch (_thisPosition)
            {
                case PositionActions.GoBackwards:
                    qty = Random.Range(1, FindObjectOfType<RandomPositions>().GetDesiredPositions() / 8);
                    ChangeColour(Color.red);
                    break;
                case PositionActions.GoForward:
                    qty = Random.Range(1, FindObjectOfType<RandomPositions>().GetDesiredPositions() / 8);
                    ChangeColour(Color.green);
                    break;
                case PositionActions.MissTurn:
                    ChangeColour(Color.gray);
                    qty = Random.Range(0, 2);
                    if (qty == 0)
                    {
                        ChangeColour(Color.cyan);
                        _thisPosition = PositionActions.None;
                    }
                    break;
                case PositionActions.None:
                    ChangeColour(Color.cyan);
                    break;
                default:
                    qty = 0;
                    break;
            }
        }

        void ChangeColour(Color colour)
        {
            
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = colour;
            }

            if (TryGetComponent(out Renderer rend))
            {
                rend.material.color = colour;
            }
            
        }

        public (int, PositionActions) GetAction()
        {
            SetNewQty();
            return (qty, _thisPosition);
        }

        private void Awake()
        {
            Material material = new Material(exampleMaterial);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = material;
            }

            if (TryGetComponent(out Renderer rend))
            {
                rend.material = material;
            }
        }

        private PositionActions GetRandomAction()
        {
            return (PositionActions)Random.Range(2, (int)(Enum.GetValues(typeof(PositionActions)).Cast<PositionActions>().Max()) + 1);
        }
    }
}
