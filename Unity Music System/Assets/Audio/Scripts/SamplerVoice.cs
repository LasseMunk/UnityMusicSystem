using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SamplerVoice : MonoBehaviour
{
 readonly ASREnvelope _envelope = new ASREnvelope();

 AudioSource _audioSource;
 uint _samplesUntilEnvelopeTrigger;

 public void PlayScheduled(AudioClip audioClip, float pitch, double startTime, double attackTime, double sustainTime, double releaseTime)
 {
  sustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
  _envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);

  double timeUntilTrigger = (startTime > AudioSettings.dspTime) ? (startTime - AudioSettings.dspTime) : 0.0;
  _samplesUntilEnvelopeTrigger = (uint)(timeUntilTrigger * AudioSettings.outputSampleRate);

  _audioSource.clip = audioClip;
  _audioSource.pitch = pitch;
  _audioSource.PlayScheduled(startTime);
 }

 void Awake()
 {
  _audioSource = GetComponent<AudioSource>();
 }

 void OnAudioFilterRead(float[] buffer, int numChannels)
 {
  double volume = 0;

  for (int sampleIndex = 0; sampleIndex < buffer.Length; sampleIndex += numChannels)
  {
   if (_samplesUntilEnvelopeTrigger == 0)
   {
    volume = _envelope.GetLevel();
   }
   else
   {
    --_samplesUntilEnvelopeTrigger;
   }

   for (int channelIndex = 0; channelIndex < numChannels; channelIndex++)
   {
    buffer[sampleIndex + channelIndex] *= (float)volume;
   }
  }
 }
}
