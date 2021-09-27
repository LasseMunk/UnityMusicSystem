using UnityEngine;
using System;

public class Metronome : MonoBehaviour
{
 public event Action<double> TickedAction;

 [SerializeField, Tooltip("The tempo in beats per minute"), Range(15f, 200f)] private double _tempo = 120.0;
 [SerializeField, Tooltip("The number of ticks per beat"), Range(1, 8)] private int _subdivisions = 4;

 double _tickLength_s;
 double _nextTickTime; // relative to AudioSettings.dspTime;

 private void Reset()
 {
  {
   Recalculate();
   _nextTickTime = AudioSettings.dspTime + _tickLength_s; // avoid double trigger
  }
 }

 void Recalculate()
 {
  double beatsPrSecond = _tempo / 60.0;
  double ticksPerSecond = beatsPrSecond * _subdivisions;
  _tickLength_s = 1.0 / ticksPerSecond;
 }

 void Awake()
 {
  Reset();
 }

 void OnValidate()
 {
  if (Application.isPlaying) Recalculate();
 }

 void Update()
 {
  double currentTime = AudioSettings.dspTime;
  currentTime += Time.deltaTime; // lookahead ~ 1 frame

  // catch all ticks withing one frame
  while (currentTime > _nextTickTime)
  {
   TickedAction?.Invoke(_nextTickTime);
   _nextTickTime += _tickLength_s;
  }
 }

 public void SetTempo(float newTempo)
 {
  _tempo = newTempo;

  if (Application.isPlaying) Recalculate();
 }
}
