using UnityEngine;

public class Sampler : MonoBehaviour
{
 [SerializeField] StepSequencer _stepSequencer;
 [SerializeField] AudioClip _audioClip;
 [SerializeField] SamplerVoice _samplerVoicePrefab;

 [SerializeField] [Range(0f, 2f)] double _attackTime;
 [SerializeField] [Range(0f, 2f)] double _sustainTime;
 [SerializeField] [Range(0f, 2f)] double _releaseTime;
 [SerializeField] [Range(1, 8)] int _numVoices = 2;

 SamplerVoice[] _samplerVoices;
 int _nextVoiceIndex;

 void Awake()
 {
  _samplerVoices = new SamplerVoice[_numVoices];

  if (_samplerVoicePrefab != null)
  {
   for (int i = 0; i < _numVoices; i++)
   {
    SamplerVoice samplerVoice = (Instantiate(_samplerVoicePrefab));
    samplerVoice.transform.name = $"SamplerVoice_{i}";
    samplerVoice.transform.parent = transform;
    samplerVoice.transform.localPosition = Vector3.zero;
    _samplerVoices[i] = samplerVoice;
   }
  }
 }

 void OnEnable()
 {
  if (_stepSequencer != null) _stepSequencer.TickedEvent += HandleTicked;
 }

 void OnDisable()
 {
  if (_stepSequencer != null) _stepSequencer.TickedEvent -= HandleTicked;
 }

 void HandleTicked(double tickTime, int midiNotePitch, double duration)
 {
  float pitch = MusicMathUtils.MidiNoteToPitch(midiNotePitch, MusicMathUtils.MidiNoteC4);
  _samplerVoices[_nextVoiceIndex].PlayScheduled(_audioClip, pitch, tickTime, _attackTime, duration, _releaseTime);

  _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
 }
}

