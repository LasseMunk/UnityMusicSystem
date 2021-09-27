using UnityEngine;
using System;

public class IntensityMute : MonoBehaviour
{
  [Serializable]
  public class SequencerIntensityPair
  {
    public StepSequencer Sequencer;
    public float Intensity;
  }

  [SerializeField] SequencerIntensityPair[] _sequencerIntensityPairs;

  public void OnInstensitySliderUpdate(float sliderValue)
  {
    for (int i = 0; i < _sequencerIntensityPairs.Length; i++)
    {
      _sequencerIntensityPairs[i].Sequencer.muteSequence = (sliderValue < _sequencerIntensityPairs[i].Intensity);
    }
  }

  void Start()
  {
    OnInstensitySliderUpdate(0f);
  }
}
