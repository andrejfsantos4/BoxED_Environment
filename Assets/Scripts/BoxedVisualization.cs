using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Contains a few basic examples for visualization grasp poses and object trajectories in BoxED. Attach it to a
/// game object to deploy it.
/// </summary>
public class BoxedVisualization : MonoBehaviour
{
    // Debugging variables
    public GameObject drawOnObject;  // Object on which the gripper should be drawn, when visualizing a grasp pose
    public GameObject drawThisObject;  // Gripper object to draw when visualizing grasp pose
    public GameObject trajectoryObject;  // Object to spawn when visualizing a trajectory

    private class TrajectoryPoseElement 
    {
        public long timeStamp;
        public float[,] rotation;
        public float[] translation;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // You can use Newtonsoft.Json to load the data in the JSON files.
        
        // If visualizing the object trajectories with DrawTrajectory(), load them into the a list of objects of the
        // class TrajectoryPoseElement
    }
    
    // Example visualization functions

    /// <summary>
    /// Visualization function that spans a new semi-transparent gripper with the grasp pose on the grasped object.
    /// The grasp pose is specified by graspPosition and graspRotation.
    /// The public variable drawThisObject is the object that is spawned, which is the gripper in this case. The public
    /// draOnObject variable is the object which was grasped. These objects should be set in the Editor.
    /// </summary>
    private void DrawGraspPose(Vector3 graspPosition, Quaternion graspRotation)
    {
        Instantiate(drawThisObject, drawOnObject.transform.TransformPoint(graspPosition), 
            drawOnObject.transform.rotation*graspRotation);
    }
    
    /// <summary>
    /// Visualization function that draws the trajectory of an object from a trajectory pose list.
    /// To do this, a copy of the object are spawned at each pose in the trajectory. The spawned object is
    /// trajectoryObject which should be set in the Editor.
    /// </summary>
    private void DrawTrajectory(List<TrajectoryPoseElement> trajectory)
    {
        foreach (var pose in trajectory)
        {
            Instantiate(trajectoryObject, new Vector3(pose.translation[0], pose.translation[1], pose.translation[2]),
                Matrix2Quat(pose.rotation));
        }
    }
    
    /// <summary>
    /// Converts a matrix of 3x3 floats to a quaternion. The method is explained here
    /// https://d3cw3dd2w32x2b.cloudfront.net/wp-content/uploads/2015/01/matrix-to-quat.pdf
    /// </summary>
    private Quaternion Matrix2Quat(float[,] matrix)
    {
        float t;
        Quaternion q;
        if (matrix[2,2] < 0) {
            if (matrix[0,0] >matrix[1,1]) {
                t = 1 + matrix[0,0] -matrix[1,1] -matrix[2,2];
                q = new Quaternion( t, matrix[0,1]+matrix[1,0], matrix[2,0]+matrix[0,2], matrix[1,2]-matrix[2,1] );
            }
            else {
                t = 1 -matrix[0,0] + matrix[1,1] -matrix[2,2];
                q = new Quaternion( matrix[0,1]+matrix[1,0], t, matrix[1,2]+matrix[2,1], matrix[2,0]-matrix[0,2] );
            }
        }
        else {
            if (matrix[0,0] < -matrix[1,1]) {
                t = 1 -matrix[0,0] -matrix[1,1] + matrix[2,2];
                q = new Quaternion( matrix[2,0]+matrix[0,2], matrix[1,2]+matrix[2,1], t, matrix[0,1]-matrix[1,0] );
            }
            else {
                t = 1 + matrix[0,0] + matrix[1,1] + matrix[2,2];
                q = new Quaternion( matrix[1,2]-matrix[2,1], matrix[2,0]-matrix[0,2], matrix[0,1]-matrix[1,0], t );
            }
        }
        return new Quaternion(q.x * 0.5f / Mathf.Sqrt(t), q.y * 0.5f / Mathf.Sqrt(t), q.z * 0.5f / Mathf.Sqrt(t), -q.w * 0.5f / Mathf.Sqrt(t));
    }
}
