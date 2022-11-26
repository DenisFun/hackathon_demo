using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomMenuTriggers : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] public GameObject WorldCanvas;

	// Update is called once per frame
	void Update()
	{


		if (Input.GetKeyDown(KeyCode.E))
		{
			SceneManager.LoadScene("City", LoadSceneMode.Single);
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			Application.Quit();
		}
		
	}

	public void OnTriggerStay(Collider collision)
	{


	
		if (collision.tag == "Trigger")
		{

			WorldCanvas.SetActive(true);
		} else WorldCanvas.SetActive(false);
		

	}


}
