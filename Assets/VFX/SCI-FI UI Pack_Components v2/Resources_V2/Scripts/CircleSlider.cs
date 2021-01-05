using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleSlider : MonoBehaviour
{
 
     public bool b=true;
	 public Image image;
	 public float speed=0.5f;

  float time =0f;
  
  public Text progress;
  
  void Start()
  {
	  
	image = GetComponent<Image>();
  }
  
    void Update()
    {
		if(b)
		{
			time+=Time.deltaTime*speed;
			image.fillAmount= time;
			if(progress)
			{
				progress.text = (int)(image.fillAmount*100)+"%";
			}
			
        if(time>1)
		{
						
			time=0;
		}
    }
	}
	
	
}
