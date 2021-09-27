using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StepSequencer))]
public class StepSequencerEditor : Editor
{
  public override void OnInspectorGUI()
  {

    // get the StepSequencer instance
    StepSequencer sequencer = (StepSequencer)target;

    // start listening for changes
    EditorGUI.BeginChangeCheck();

    // Draw the controls we are not hiding with [HideInInspector]
    DrawDefaultInspector();

    List<StepSequencer.Step> steps = sequencer.GetSteps();

    // Set number of steps in sequence
    int numSteps = EditorGUILayout.IntSlider("# steps", steps.Count, 1, 32);
    // Debug.Log(_numSteps);


    // Add or remove steps based on the above slider's value

    while (numSteps > steps.Count)
    {
      steps.Add(new StepSequencer.Step());
    }
    while (numSteps < steps.Count)
    {
      steps.RemoveAt(steps.Count - 1);
    }

    // Draw the steps
    for (int i = 0; i < steps.Count; ++i)
    {
      StepSequencer.Step step = steps[i];

      // Draw all the step field on one line
      EditorGUILayout.BeginHorizontal();
      EditorGUIUtility.labelWidth = 60;
      EditorGUILayout.LabelField("Step " + (i + 1), GUILayout.Width(60));
      step.Active = EditorGUILayout.Toggle("Active", step.Active, GUILayout.Width(80));
      step.MidiNotePitch = EditorGUILayout.IntField("Note", step.MidiNotePitch);
      step.Duration = EditorGUILayout.FloatField("Duration", (float)step.Duration);
      EditorGUIUtility.labelWidth = 0;
      EditorGUILayout.EndHorizontal();
    }

    // if there were changes, mark the StepSequencer dirty,
    // that is, let Unity know it should be re-saved when saving the scene/project.
    if (EditorGUI.EndChangeCheck())
    {
      EditorUtility.SetDirty(target);
    }
  }
}