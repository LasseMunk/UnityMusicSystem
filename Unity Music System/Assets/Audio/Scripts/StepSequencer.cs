using System.Collections.Generic;
using UnityEngine;
using System;

public class StepSequencer : MonoBehaviour
{

  [Serializable]
  public class Step
  {
    public bool Active;
    public int MidiNotePitch;
    public double Duration;
  }

  public delegate void HandleTick(double tickTime, int midiNotePitch, double duration);

  public event HandleTick TickedEvent;

  [SerializeField] Metronome _metronome;
  [SerializeField, HideInInspector] List<Step> _steps;
  public bool muteSequence = false;

  int _currentTick = 0;

#if UNITY_EDITOR
  public List<Step> GetSteps() { return _steps; }
#endif

  void OnEnable()
  {
    if (_metronome != null) _metronome.TickedAction += HandleTicked;
  }

  void OnDisable()
  {
    if (_metronome != null) _metronome.TickedAction -= HandleTicked;
  }

  public void HandleTicked(double tickTime)
  {
    int numSteps = _steps.Count;

    if (numSteps == 0) return;

    Step step = _steps[_currentTick];

    if (step.Active)
    {
      if (!muteSequence) TickedEvent?.Invoke(tickTime, step.MidiNotePitch, step.Duration);
    }

    _currentTick = (_currentTick + 1) % numSteps;

  }
}
