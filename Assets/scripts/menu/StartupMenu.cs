﻿using UnityEngine;

public class StartupMenu : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		LocalizedStrings.LoadLanguage(SystemLanguage.English);
		// LocalizedStrings.LoadLanguage(Application.systemLanguage);	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
