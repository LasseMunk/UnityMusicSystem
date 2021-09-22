using UnityEngine;

public class Sampler : MonoBehaviour
{
  [SerializeField] Metronome _metronome;
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
    if (_metronome != null)
    {
      _metronome.TickedAction += HandleTicked;
    }
  }

  void OnDisable()
  {
    if (_metronome != null)
    {
      _metronome.TickedAction -= HandleTicked;
    }
  }

  void HandleTicked(double tickTime)
  {
    _samplerVoices[_nextVoiceIndex].PlayScheduled(_audioClip, tickTime, _attackTime, _sustainTime, _releaseTime);
    _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
  }
}

