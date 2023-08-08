using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
namespace Tennis.Orthographic
{
    public class ScoreZone : NetworkBehaviour
    {
        public int MasterScore, ClientScore;

        public ScoreTexts Instance;

        [Networked(OnChanged = nameof(SetscoreChanged))]
        public int SetMasterScore { get; set; }
        public int SetClientScore { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Changing the score here directly does not work since this code is only executed on the client pressing the button and not on every client.
           /* if (TennisMovement.instance.HasInputAuthority)
            {
                SetClientScore = ClientScore++;
                Instance.scoreClient.text = ClientScore.ToString();
                Debug.Log("Client Score added");

            }
            else
            {
                SetMasterScore = MasterScore++;
                Instance.scoreMaster.text = MasterScore.ToString();
                Debug.Log("Master Score added");
            }*/
        }

        private static void SetscoreChanged(Changed<ScoreZone> changed)
        {
            changed.Behaviour.MasterScore = changed.Behaviour.SetMasterScore;
            changed.Behaviour.MasterScore = changed.Behaviour.SetClientScore;

        }



    }
}
