using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<GameEvent> gameEvents = new List<GameEvent>();
    [SerializeField] ParticleSystem drumParticles;

    private int count = 0;
    private int gameEventIndex = 0;

    public void AudioTriggered()
    {
        // Awaken new spirit tree on every 5th bang of the drum
        if (count % 5 == 0)
        {
            gameEvents[gameEventIndex].Raise();
        }

        if (drumParticles != null)
        {
            drumParticles.Play();
        }
    }
}
