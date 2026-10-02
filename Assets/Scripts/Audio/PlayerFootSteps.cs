using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFootSteps : MonoBehaviour
{
	[SerializeField] private AudioClip footstepClip;
	[SerializeField] private AudioSource audioSource;
	[SerializeField, Min(0.1f)] private float distanceBetweenSteps = 0.65f;
	[SerializeField, Range(0f, 1f)] private float volume = 1f;

	private Rigidbody2D body;
	private Vector2 lastPosition;
	private float distanceSinceLastStep;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		if (audioSource == null)
			audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = GetComponentInChildren<AudioSource>();

		lastPosition = body.position;

		if (footstepClip == null)
		{
			if (audioSource != null)
				footstepClip = audioSource.clip;
		}

		if (audioSource == null)
		{
			Debug.LogError("PlayerFootSteps needs an AudioSource on this object or a child object.", this);
			enabled = false;
			return;
		}

		if (footstepClip == null)
		{
			Debug.LogError("PlayerFootSteps needs a footstep clip assigned, or assigned as the AudioSource clip.", this);
			enabled = false;
			return;
		}

		audioSource.spatialBlend = 0f;
	}

	private void Update()
	{
		Vector2 currentPosition = body.position;
		float distanceMoved = Vector2.Distance(lastPosition, currentPosition);
		lastPosition = currentPosition;

		if (distanceMoved <= 0f)
			return;

		distanceSinceLastStep += distanceMoved;
		if (distanceSinceLastStep >= distanceBetweenSteps)
		{
			distanceSinceLastStep %= distanceBetweenSteps;

			audioSource.PlayOneShot(footstepClip, volume * VolumeManager.GetInstance().SFXVolume);
		}
	}
}
