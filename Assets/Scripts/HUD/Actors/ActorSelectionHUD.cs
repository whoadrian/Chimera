using System;
using Chimera.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Chimera
{
    /// <summary>
    /// Manages the selection circle under actors. Needs to be a scene child of an actor component.
    /// </summary>
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
            // Set selection circle colour based on actor's faction
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
            selectable.gameObject.SetActive(false);
        }

        private void Update()
        {
            // Activate the selection circle when actor is selected.
            if (selectable.gameObject.activeSelf != _actor.Selected)
            {
                selectable.gameObject.SetActive(_actor.Selected);
            }
        }
    }
}