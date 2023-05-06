using System;
using Chimera.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Chimera
{
    public class ActorSelectionHUD : MonoBehaviour
    {
        public Image selectable;

        private Actor _actor;

        private void Awake()
        {
            _actor = transform.parent.GetComponent<Actor>();
        }
        
        private void Start()
        {
            Color color;
            switch (_actor.Faction)
            {
                case Faction.Red: 
                    color = Color.red;
                    break;
                case Faction.Green: 
                    color = Color.green;
                    break;
                case Faction.Blue: 
                    color = Color.blue;
                    break;
                default:
                    throw new NotImplementedException();
            }

            selectable.color = color;
            selectable.enabled = false;
        }

        private void Update()
        {
            if (selectable.enabled != _actor.Selected)
            {
                selectable.enabled = _actor.Selected;
            }
        }
    }
}