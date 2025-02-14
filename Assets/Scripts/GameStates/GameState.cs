﻿using UnityEngine;
using System.Collections;

public class GameState : BaseState
{
	private float _nextHungerDecrease = 0f;
	private float _startDistance;

	public override void OnLoad(BaseState lastState)
	{
		GameManager.Instance.gameUI.SetActive(true);

		GameManager.Instance.CurrentPlayer.Reset();
		GameManager.Instance.CurrentPlayer.SetHidden(false);

		GameManager.Instance.CurrentTram.Reset();
		GameManager.Instance.CurrentTram.EngineOn(true);
		GameManager.Instance.CurrentTram.PlayerEnter(GameManager.Instance.CurrentPlayer);
		GameManager.Instance.CurrentTram.SetThrottle(0.3f);
		//GameManager.Instance.CurrentTram.SetSpeed(GameManager.Instance.CurrentTram.minMoveSpeed);
		GameManager.Instance.CurrentTram.ShowCanvasTips(true);

		StormManager.Instance.ResetStorm(GameManager.Instance.CurrentTram.transform.position);
		StormManager.Instance.SetStormActive(true);

		_nextHungerDecrease = Time.time + 3f;
		_startDistance = GameManager.Instance.CurrentTram.transform.position.z;
	}

	public override void OnUnload(BaseState nextState)
	{
		GameManager.Instance.gameUI.SetActive(false);
	}

	public override void Update()
	{
		var interfaceManager = GameManager.Instance.interfaceManager;
		var chunkDistance = GameManager.Instance.chunkDistance;
		var chunkSize = GameManager.Instance.chunkSize;
		var player = GameManager.Instance.CurrentPlayer;
		var tram = GameManager.Instance.CurrentTram;

		if (Time.time >= _nextHungerDecrease)
		{
			player.hunger = Mathf.Max(player.hunger - player.hungerDecrease, 0);

			_nextHungerDecrease = Time.time + 3f;

			if (player.hunger <= 0)
			{
				player.Hit(player.hungerDamage);
			}
		}

		interfaceManager.SetSpeed(tram.GetSpeedPerSecond());
		interfaceManager.SetDistance(tram.transform.position.z - _startDistance);
		interfaceManager.SetHealth(player.health / 100f);
		interfaceManager.SetHunger(player.hunger /100f);
		interfaceManager.SetFuelConsumption(tram.GetFuelConsumption());
		interfaceManager.SetFuel(tram.fuel / 100f);

		base.Update();
	}
}