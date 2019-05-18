using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace VRKite.Interaction.VoiceControl
{
    namespace VRKite.PetSystem.Voice
    {
        public class ShoutDetector : MonoBehaviour
        {
            const int OUTPUT_SIZE = 1 << 6;
            const float outputFactor = 100000f;

            public float frequency = 0.2f;

            public int startingAnalysisPoint = 20;
            public int endingAnalysisPoint = 60;

            public float volumeThresold = 0.9f;
            public float shoutWaitingTime = 1f;
            
            private float currentVolume;
            
            [HideInInspector]
            public float[] spectrumData = new float[OUTPUT_SIZE];

            private MicrophoneLinker microphone;
            private AudioSource audioSource;


            public delegate bool ShoutDetectorDelegate(ShoutDetector detector);
            public ShoutDetectorDelegate OnShout;

            #region calibration
            void Awake()
            {
                microphone = this.GetComponent<MicrophoneLinker>();
                if (microphone == null)
                {
                    Debug.LogError("No microphone linker found");
                }
            }

            void OnEnable()
            {
                audioSource = GetComponent<AudioSource>();
                StartCoroutine(NoiseDetectionService());
            }

            void OnDisable()
            {
                StopAllCoroutines();
            }

#if UNITY_EDITOR
            void Update()
            {
                DrawInformation();
            }
#endif

#endregion

            IEnumerator NoiseDetectionService()
            {
                while (true)
                {
                    bool consumed = false;
                    if (microphone.MicrophoneFound)
                    {
                        consumed = ExamineNoise();
                    }

                    if (frequency <= 0f && !consumed)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        yield return new WaitForSeconds(consumed ? shoutWaitingTime : frequency);
                    }
                }
            }

            bool ExamineNoise()
            {
                audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

                currentVolume = HowLoud(spectrumData);
                if (currentVolume > volumeThresold)
                {
                    Debug.Log("SHOUT!!");
                    if(OnShout != null)
                    {
                        return OnShout(this);
                    }
                }
                return false;
            }

            float HowLoud(float[] sample)
            {
                int samples = endingAnalysisPoint - startingAnalysisPoint;

                float frequencySum = 0;

                for (int i = startingAnalysisPoint; i < endingAnalysisPoint; i++)
                {
                    frequencySum += spectrumData[i] / samples;
                }

                return frequencySum;

            }

            void DrawInformation()
            {
                int samples = endingAnalysisPoint - startingAnalysisPoint;
                //draw spectrum
                for (int i = startingAnalysisPoint + 1; i < endingAnalysisPoint; i++)
                {
                    Debug.DrawLine(new Vector3(100 + i - 1, spectrumData[i - 1] * outputFactor, 0),
                                   new Vector3(100 + i, spectrumData[i] * outputFactor, 0),
                                   Color.red);
                }

                Debug.DrawLine(new Vector3(100 + startingAnalysisPoint, currentVolume * outputFactor, 0), new Vector3(100 + endingAnalysisPoint, currentVolume * outputFactor, 0), Color.cyan);
                Debug.DrawLine(new Vector3(100 + startingAnalysisPoint, volumeThresold * outputFactor, 0), new Vector3(100 + endingAnalysisPoint, volumeThresold * outputFactor, 0), Color.green);
            }


        }
    }
}