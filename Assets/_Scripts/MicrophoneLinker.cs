using UnityEngine;
using System.Collections;
using System;


namespace VRKite.Interaction.VoiceControl
{
    public class MicrophoneLinker : MonoBehaviour
    {
        const float delay = 0.030f;
        const float freq = 256f;
        string microphoneName;

        public event Action OnMicrophoneFound;
        public bool MicrophoneFound = false;

        public bool continuousDetection = false;
        
        void Update()
        {
            if (!MicrophoneFound && Microphone.devices.Length > 0)
            {
                MicrophoneFound = true;
                StartMic();
                if (OnMicrophoneFound != null)
                {
                    OnMicrophoneFound();
                }
            }
            else if (MicrophoneFound && Microphone.devices.Length == 0)
            {
                MicrophoneFound = false;
                StopMic();
            }
            else if (MicrophoneFound)
            {
                UpdateMicLink();
            }
        }


        void OnDisable()
        {
            StopMic();
        }

        void StartMic()
        {
            if (Microphone.devices != null && Microphone.devices.Length > 0)
            {
                microphoneName = Microphone.devices[0];
                GetComponent<AudioSource>().clip = Microphone.Start(microphoneName, true, 5, (int)freq);
            }
        }

        void StopMic()
        {
            if (Microphone.devices.Length > 0)
            {
                microphoneName = Microphone.devices[0];
                Microphone.End(microphoneName);
            }
        }

        void UpdateMicLink()
        {
            int microphoneSamples = Microphone.GetPosition(microphoneName);

            if (microphoneSamples / freq > delay)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    //jump over the delayed samples
                    GetComponent<AudioSource>().timeSamples = (int)(microphoneSamples - delay * freq);
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}