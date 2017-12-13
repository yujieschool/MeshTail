using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrailSection

{

   

    public Vector3 upDir;

    public float timer;



    public Vector3 position;


    //  mesh

    public Vector3[] vertex;

    public Color[] colors;

    public Vector2[] uv;


   // int[] trangles;


    public void ShowColor()
    {
        Debug.Log(colors[0]);

        Debug.Log(colors[1]);
    }


    public   int cellIndex;

    public TrailSection()
    {

       // Debug.Log("Section  initial  coming");


        upDir = Vector3.up;

        timer = 0;

        cellIndex = 0;



        vertex = new Vector3[2];
        for (int i = 0; i < vertex.Length; i++)
        {
            vertex[i] = Vector3.zero;
            
        }


        uv = new Vector2[2];

        for (int i = 0; i < uv.Length; i++)
        {

            uv[i] = Vector2.zero;
        
        }


        colors = new Color[2];



        //for (int i = 0; i < colors.Length; i++)
        //{

        //    colors[i] =  Color.white;

        //}

    }


    public void Initial(int index ,float  total ,Color start, Color  end)
    {


        cellIndex = index;


        float  tmpU = (float)index  / total ;

        uv[0].x = tmpU;

        uv[0].y = 0;


        uv[1].x = tmpU;

        uv[1].y = 1;



        Color interpolatedColor = Color.Lerp(start, end, tmpU);
        colors[ 0] = interpolatedColor;
        colors[ 1] = interpolatedColor;

       // Debug.Log("index ===" + index + "tmpu====" + tmpU + "==interpolatedColor==" + interpolatedColor);



 
    }



    public void InitialVertex( Vector3 one, Vector3 two)
    {

        vertex[0] = one;

        vertex[1] = two;


    }



    public void FollowFront(TrailSection front, float total,Color start, Color  end)
    {


      

        if (front.timer < 0.00001f)
        {

            CopyFromSetion(front);


        }
        else
        {

            Initial(this.cellIndex,total,start,end);

 
        }

        this.cellIndex = front.cellIndex + 1;

    }


    public void CopyFromSetion(TrailSection  tmpFrom)
    {

        for (int i = 0; i <   this.colors.Length; i++)
			{
			      this.colors[i] = tmpFrom.colors[i];
			}


        for (int i = 0; i < this.uv.Length; i++)
        {
              this.uv[i] = tmpFrom.uv[i];
        }



        for (int i = 0; i < this.vertex.Length; i++)
        {
            this.vertex[i] = tmpFrom.vertex[i];
        }



        this.upDir = tmpFrom.upDir;

        this.position = tmpFrom.position;

        this.timer = tmpFrom.timer;




    }


    public bool ReduceTimer(float  delTimer)
    {


        timer -= delTimer;


        if (timer < 0.0001f)
        {

            timer = 0.0f;

            return true;
          
        }


        return false;


    }




}


public class CycleQueue<T>
{


    List<T> myQueue;


    public CycleQueue()
    {

        myQueue = new List<T>();
    }


    public CycleQueue(int  tmpCount)
    {

        myQueue = new List<T>();


        for (int i = 0; i < tmpCount; i++)
        {
            

             T  tmpT = default(T);
             myQueue.Add(tmpT);
        }
    }

    public void Debug()
    {

        for (int i = 0; i < myQueue.Count; i++)
        {

            UnityEngine.Debug.Log(myQueue[i]);
            
        }
    }

    public T GetMember(int index)
    {
         return   myQueue[index] ;
    }

    public void AddMember(T tmpT)
    {

        myQueue.Add(tmpT);
    }


    public List<T> GetMembers()
    {

        return myQueue;
    }



    public T GetFirst()
    {
        return myQueue[0];


    }

    public int GetMaxCout()
    {
        return myQueue.Count;
    }


    public T GetLast()
    {
        return myQueue[myQueue.Count-1];


    }





    public void Replace(int from, int to)
    {
       T  tmpT = myQueue[from];


        myQueue[from] = myQueue[to];


        myQueue[to] = tmpT;

       
    }

    public void ReplaceLastToFirst()
    {

      //  Replace(myQueue.Count-1,0);

        T   tmpT =  myQueue[myQueue.Count -1];

        myQueue.RemoveAt(myQueue.Count -1);

        myQueue.Insert(0,tmpT);


    }


    public void PutLast(T tmpT)
    {


        myQueue.Add(tmpT);
    }

    





}





public class TrainSetionManager
{

    CycleQueue<TrailSection> setionManager;



  //  public float minDistance = 0.1f;

    public float sqrMinDistance = 0.0001F;





    public float lastTimer = 0.2f;

    public float height = 1.1f;


     public Color endColor =  Color.white;

     public Color startColor = Color.red;



    private Transform ower;
    private int curCount = 0;

    public TrainSetionManager(int  tmpCount ,Transform  tmpTrans,Color  tmpStart , Color  tmpEnd)
    {

        ower = tmpTrans;


        setionManager = new CycleQueue<TrailSection>();


        startColor = tmpStart;

        endColor = tmpEnd;

        curCount = tmpCount;

        for (int i = 0; i < tmpCount; i++)
        {

            float tmpFloat = (float)i / tmpCount;

          
            TrailSection tmpSection = new TrailSection();

            setionManager.AddMember(tmpSection);


            tmpSection.Initial(i, tmpCount, startColor, endColor);
            Initial(tmpSection, ower.position);
            
        }



    }


    public TrailSection GetMembers(int index)
    {
        TrailSection tmpSection = (TrailSection)setionManager.GetMember(index);

        return tmpSection;
    }


     void ReduceTimer(float deltimer)
    {

        List<TrailSection> tmpSections = setionManager.GetMembers();


         int  tmpCount = tmpSections.Count ;


        for (int i = 0; i < tmpSections.Count; i++)
        {


            if (tmpSections[i].ReduceTimer(deltimer))
            {
                if (tmpCount > i)
                {
                    tmpCount = i;
                }
            }


            curCount = tmpCount;




            UpdateSetionVertex(tmpSections[i]);

        }


    }


     void FollowFront()
    {


        List<TrailSection> tmpSections = setionManager.GetMembers();



        for (int i = 1; i < tmpSections.Count; i++)
        {


            tmpSections[i].FollowFront(tmpSections[i - 1], curCount, startColor, endColor);

        }
    }



     public void Initial(TrailSection tmpsetioin, Vector3 postion )
     {


         tmpsetioin.position = postion;

         tmpsetioin.timer = lastTimer;



         tmpsetioin.Initial(0, curCount, startColor, endColor);



     }


     public void UpdateSetionVertex(TrailSection tmpsetioin)
     {
         tmpsetioin.upDir =  ower.TransformDirection(Vector3.up);


         Vector3 first = ower.worldToLocalMatrix.MultiplyPoint(tmpsetioin.position);

         Vector3 two = ower.worldToLocalMatrix.MultiplyPoint(tmpsetioin.position + tmpsetioin.upDir * height);


         tmpsetioin.InitialVertex(first, two);

     }

     void  IsAddPoint(Vector3  postion)
    {



       
        TrailSection   tmpFirst = setionManager.GetFirst();


    

         float  tmpfloat = (tmpFirst.position - postion).sqrMagnitude  ;


        // Debug.Log("curCount===========" + curCount);

         if (tmpfloat > sqrMinDistance)
        {



          TrailSection tmpLast=  setionManager.GetLast();

         // Debug.Log("tmplast=========" + tmpLast.cellIndex);
        

          Initial(tmpLast,postion);


      


           setionManager.ReplaceLastToFirst();



         

        }

         FollowFront();

       

    }

     public void ResetTrail()
     {

         TrailSection tmpFirst = setionManager.GetFirst();




         Initial(tmpFirst, ower.position);

         UpdateSetionVertex(tmpFirst);



         List<TrailSection> tmpSections = setionManager.GetMembers();

       

         for (int i = 1; i < tmpSections.Count; i++)
         {


             tmpSections[i].CopyFromSetion(tmpFirst);

         }



     }

    public void Update(float deltime)
    {


      


        IsAddPoint(ower.position);


        ReduceTimer(deltime);



    }






 

}


public class MeshManager
{
        int maxCount;



    Vector3[] meshVertices;


    Color[] meshColor;

    Vector2[] meshUV;


    int[] meshTriangles;


    Mesh mesh;

    public MeshManager(int tmpCout,Mesh  tmpMesh)
    {


        maxCount = tmpCout;



        mesh = tmpMesh;


        meshVertices = new Vector3[maxCount*2];


        meshColor = new Color[maxCount *2];

        meshUV = new Vector2[maxCount*2];


        meshTriangles = new int[(maxCount-1) *6];

        for (int i = 0; i < maxCount-1 ; i++)
        {

            meshTriangles[i * 6 + 0] = i * 2;
            meshTriangles[i * 6 + 1] = i * 2 + 1;
            meshTriangles[i * 6 + 2] = i * 2 + 2;

            meshTriangles[i * 6 + 3] = i * 2 + 2;
            meshTriangles[i * 6 + 4] = i * 2 + 1;
            meshTriangles[i * 6 + 5] = i * 2 + 3;

        }


        FreshMesh();



    }


    public void FreshMesh()
    {

        mesh.Clear();

        mesh.vertices = meshVertices;

        mesh.uv = meshUV;

        mesh.colors = meshColor;


        mesh.triangles = meshTriangles;

     //   mesh.RecalculateNormals();

        mesh.RecalculateBounds();

     

    }



    public void Update(TrainSetionManager manager)
    {

      //  Debug.Log("Vertex  begin  ======");



        for (int i = 0; i < maxCount; i++)
        {

            TrailSection  section = manager.GetMembers(i);

            meshVertices[i*2] = section.vertex[0];

            meshVertices[i*2 +1] = section.vertex[1];


          //  Debug.Log("section.vertex[0]==" + section.vertex[0]);

         //   Debug.Log("section.vertex[1]==" + section.vertex[1]);

            meshColor[i * 2] = section.colors[0];

            meshColor[i * 2+1] = section.colors[1];


         





            meshUV[i * 2] = section.uv[0];

            meshUV[i * 2 + 1] = section.uv[1];




            
        }


        FreshMesh();

      

    }


}




public class MeshTail : MonoBehaviour
{


    TrainSetionManager trainSetion;


    MeshManager meshTrail;





    public float lastTimer = 2.0f;

    public float height = 1.1f;


    public Color endColor = Color.white;

    public Color startColor = Color.white;

    public int cellCount =100;


    public float middleTime = 0.0001f;



    Mesh myMesh;



	// Use this for initialization
	void Awake () 
    {

        myMesh = transform.GetComponent<MeshFilter>().mesh;


        myMesh.Clear();


        //myMesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        //myMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        //myMesh.triangles = new int[] { 0, 1, 2 };



        trainSetion = new TrainSetionManager(cellCount, transform,startColor,endColor);


        //trainSetion.startColor = startColor;

        //trainSetion.endColor = endColor;

        trainSetion.lastTimer = lastTimer;



        trainSetion.height = height;


        trainSetion.sqrMinDistance = middleTime;


        meshTrail = new MeshManager(cellCount, myMesh);


       // meshTrail.Update(trainSetion);
        
	}

    public void Rest()
    {


        trainSetion.ResetTrail();

        meshTrail.Update(trainSetion);

    }
	
    //// Update is called once per frame
    void Update()
    {


        trainSetion.Update(Time.deltaTime);

        meshTrail.Update(trainSetion);



    }
}
