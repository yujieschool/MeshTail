using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutiUmbrellaPath : MonoBehaviour {


    public Transform[] desTrans;

    public float[] durationTime;

    private MeshTail meshTrail;


    public Transform center;


    Vector3 orginPos;

  //  Vector3 desPos;

   // Vector3 srcPos;



    float srcRadiu;

    float desRadius;

    // Use this for initialization
    void Start()
    {
        meshTrail = GetComponent<MeshTail>();
       // orginPos = desTrans[0].position;

        ResetPath();


    }


    float timeCount = 0;

    float rate = 0;



    byte curIndex;


    public void ResetPath()
    {
        timeCount = 0;

        curIndex = 0;
      


      Vector3 myVector = desTrans[curIndex].position - center.position;

        srcRadiu = Vector3.Magnitude(myVector);


      

       // desPos = desTrans[curIndex].position;

        

        Vector3 destVect = desTrans[curIndex+1].position - center.position;

        desRadius = Vector3.Magnitude(destVect);



        isPlay = false;

        Invoke("DelayExcuse", 1.0f);


    }



    public void NextNode()
    {
       // srcPos = desTrans[curIndex ].position;
        Vector3 myVector = desTrans[curIndex].position - center.position;



        srcRadiu = Vector3.Magnitude(myVector);

      //  Debug.Log("srcRadiu radius==" + srcRadiu +"==index=="+curIndex);



      //  desPos = desTrans[curIndex+1].position;
        Vector3 destVect = desTrans[curIndex + 1].position - center.position;

        desRadius = Vector3.Magnitude(destVect);
       // Debug.Log("desRadius radius==" + desRadius);



    }


    bool isPlay = false;
    public void DelayExcuse()
    {

      //  Debug.Log("coming!!");
        isPlay = true;

        transform.position = desTrans[curIndex].position;
        if (meshTrail != null)
        {

            //Debug.Log("reset ");
            meshTrail.Rest();
        }

    }
    // Update is called once per frame
    void Update()
    {


        if (!isPlay)
            return;

        timeCount += Time.deltaTime;

        if (timeCount > durationTime[curIndex])
        {

           

            timeCount = 0;

            curIndex++;

          //  Debug.Log("curindex =="+curIndex);

            if (curIndex >= desTrans.Length-1)
            {
                ResetPath();
            }
            else
            {

               
                NextNode();
            }
        

        }
        else
        {
            rate = timeCount / durationTime[curIndex];

            Vector3 finalPos = Vector3.Lerp(desTrans[curIndex].position, desTrans[curIndex + 1].position, rate);
            Vector3 tmpPos = finalPos - center.position;



           
            Vector3 tmpDesPos = tmpPos.normalized * Mathf.Lerp(srcRadiu, desRadius, rate);


           // Debug.Log("src radius ==" + srcRadiu + "==des radis==" + desRadius + "==final==" + Mathf.Lerp(srcRadiu, desRadius, rate));

          //  Debug.Log("src ==" + desTrans[curIndex].position + "==des ==" + desTrans[curIndex + 1].position + "==rate ==" + rate + " ==pos==" + finalPos);
            // Debug.Log(radius);


            transform.position = tmpDesPos + center.position; //  Vector3.Lerp(transform.position, tmpDesPos, Time.deltaTime * 10);


        }





    }
}
