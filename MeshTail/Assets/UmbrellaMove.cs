using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaMove : MonoBehaviour {


    public Transform desTrans;

    public float durationTime;







    private MeshTail meshTrail;

    //private float deltaAdd = 0;
    ////bool isAdd = false;


    Vector3 orginPos;

    Vector3 desPos;





    float srcRadiu;

    float desRadius;

	// Use this for initialization
	void Start () {







        Vector3 myVector = transform.position - transform.parent.position;

        srcRadiu = Vector3.Magnitude(myVector);

        orginPos = transform.position;

        desPos = desTrans.position;

       // Debug.Log("srcRadiu radius==" + srcRadiu);

        Vector3 destVect = desTrans.position - transform.parent.position;

        desRadius = Vector3.Magnitude(destVect);


       // Debug.Log("des radius=="+  desRadius);


      //  Debug.Log("des radius222==" + Vector3.Distance(desTrans.position, transform.parent.position));

      //   deltaAdd  =(desRadius - srcRadiu) / durationTime;


        meshTrail  = GetComponent<MeshTail>();
        
	}


    float timeCount = 0;

    float rate  = 0;


	// Update is called once per frame
	void Update () {


        timeCount += Time.deltaTime;

        if (timeCount > durationTime)
        {
            timeCount = 0;


            transform.position = orginPos;

            if (meshTrail != null)
            {

               // Debug.Log("reset ");
                meshTrail.Rest();
            }

        }
        else
        {
            rate = timeCount / durationTime;
            Vector3 tmpPos = Vector3.Lerp(orginPos, desPos, rate) - transform.parent.position;


            Vector3 tmpDesPos = tmpPos.normalized * Mathf.Lerp(srcRadiu, desRadius, rate);

            // Debug.Log(radius);


            transform.position = tmpDesPos + transform.parent.position; //Vector3.Lerp(transform.position, tmpDesPos, Time.deltaTime*10);


        }



        
		
	}
}
