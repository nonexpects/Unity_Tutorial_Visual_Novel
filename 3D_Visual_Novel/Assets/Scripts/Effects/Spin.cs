using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] Transform tf_Target;
    //[SerializeField] float spinSpeed;
    //[SerializeField] Vector3 spinDir;
    
    void Update()
    {
        //transform.Rotate(spinDir * spinSpeed * Time.deltaTime);

        if(tf_Target != null)
        {
            Vector3 relativePos = tf_Target.position - transform.position; 
            Quaternion t_Rotation = Quaternion.LookRotation(relativePos);
            Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0);
            transform.eulerAngles = t_Euler;
        }
    }
}
