using UnityEngine;

namespace MiniJameGam9.Character.Player
{
	public class CameraManager : MonoBehaviour
	{
		private Transform _follow;
		public Transform ToFollow
		{
			set
			{
				_follow = value;
				_offset = transform.position - _follow.position;
			}
		}

		private Vector3 _offset;

		private float _duration, _maxDuration;
		private float _shakeAmount;

		void Update()
		{
			if (_follow == null) // We didn't set an object to follow yet
			{
				return;
			}
			transform.position = _follow.position + _offset;
			if (_duration > 0)
			{
				transform.localPosition = transform.position + _shakeAmount * Mathf.Lerp(0f, 1f, _duration / _maxDuration) * Random.insideUnitSphere;
				_duration -= Time.deltaTime;
			}
		}

		public void Launch(float duration, float shakeAmount)
		{
			_duration = duration;
			_maxDuration = duration;
			_shakeAmount = shakeAmount;
		}
	}

}