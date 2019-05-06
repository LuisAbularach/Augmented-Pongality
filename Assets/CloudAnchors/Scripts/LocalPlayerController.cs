//-----------------------------------------------------------------------
// <copyright file="LocalPlayerController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.CloudAnchors
{
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// Local player controller. Handles the spawning of the networked Game Objects.
    /// </summary>
#pragma warning disable 618
    public class LocalPlayerController : NetworkBehaviour
#pragma warning restore 618
    {

        
        public GameObject BallPrefab;
        public GameObject SecondPlayerZone;
        GameObject BallInPlay;
        /// <summary>
        /// The Star model that will represent networked objects in the scene.
        /// </summary>
        public GameObject StarPrefab;

        /// <summary>
        /// The Anchor model that will represent the anchor in the scene.
        /// </summary>
        public GameObject AnchorPrefab;

        public GameObject SinglePlayerField;

        private int countWalls = 0;
        public delegate void ObjectPlaced(string s);
        public static event ObjectPlaced OnObjectPlaced;

        public delegate void SetUpComplete();
        public static event SetUpComplete OnSetUpComplete;
        /// <summary>
        /// The Unity OnStartLocalPlayer() method.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // A Name is provided to the Game Object so it can be found by other Scripts, since this
            // is instantiated as a prefab in the scene.
            gameObject.name = "LocalPlayer";
        }

        /// <summary>
        /// Will spawn the origin anchor and host the Cloud Anchor. Must be called by the host.
        /// </summary>
        /// <param name="position">Position of the object to be instantiated.</param>
        /// <param name="rotation">Rotation of the object to be instantiated.</param>
        /// <param name="anchor">The ARCore Anchor to be hosted.</param>
        public void SpawnAnchor(Vector3 position, Quaternion rotation, Component anchor)
        {
            // Instantiate Anchor model at the hit pose.
            var anchorObject = Instantiate(AnchorPrefab, position, rotation);

            // Anchor must be hosted in the device.
            anchorObject.GetComponent<AnchorController>().HostLastPlacedAnchor(anchor);

            // Host can spawn directly without using a Command because the server is running in this
            // instance.
#pragma warning disable 618
            NetworkServer.Spawn(anchorObject);
#pragma warning restore 618

        
        }

 #pragma warning disable 618
        [Command]
#pragma warning restore 618
        public void CmdSetProperties(Vector3 NewDirection, float speed)
        {
            BallInPlay = GameObject.Find("Ball(Clone)");
            Debug.Log("LocalPlayer - setting properties");
            BallInPlay.GetComponent<Ball>().direction = NewDirection;
            BallInPlay.GetComponent<Ball>().movementSpeed = speed;
        }   

        /// <summary>
        /// A command run on the server that will spawn the Star prefab in all clients.
        /// </summary>
        /// <param name="position">Position of the object to be instantiated.</param>
        /// <param name="rotation">Rotation of the object to be instantiated.</param>
#pragma warning disable 618
        [Command]
#pragma warning restore 618
        public void CmdSpawnWall(Vector3 position, Quaternion rotation, float size)
        {
            GameObject Wall = Instantiate(StarPrefab , new Vector3(position.x,position.y,-0.5f), rotation);
            //Wall.transform.position += new Vector3(0,0,-1);
           
#pragma warning disable 618
            NetworkServer.Spawn(Wall);
#pragma warning restore 618

            //Set wall size (cannot send dynamically sized wall through server)
             if(position.x < 0){
                Wall.name = "LeftWall";
                if(OnObjectPlaced != null){
                    if (countWalls==0){
                        OnObjectPlaced("Left Wall Placed. Place right side.");
                    }  
                    else
                        OnObjectPlaced("Left Wall Placed. Find the ball, and tap to start");
                }
            }
            else
            {
                Wall.name = "RightWall";
                if(OnObjectPlaced != null){
                    if (countWalls==0){
                        OnObjectPlaced("Right Wall Placed. Place left side.");
                    }  
                    else
                        OnObjectPlaced("Right Wall Placed. Find the ball, and tap to start");
                }
            }
            countWalls++; //to check if walls are set
            if(countWalls == 2){
                if(OnSetUpComplete!=null)
                    OnSetUpComplete();
            }
            float x = Wall.transform.localScale.x;
            float y = Wall.transform.localScale.y;
            Wall.transform.localScale = new Vector3(x,y,size);
        }
        
#pragma warning disable 618
        [Command]
#pragma warning restore 618
        public void CmdSpawnBall(Vector3 position, Quaternion rotation, float P2backOfField
        )
        {
            // Instantiate Star model at the hit pose.
            BallInPlay = Instantiate(BallPrefab, new Vector3(0,1.0f,0), rotation);

#pragma warning disable 618
            NetworkServer.Spawn(BallInPlay);
#pragma warning restore 618

            BallInPlay.GetComponent<Ball>().P2Back = P2backOfField;
        }

#pragma warning disable 618
        [Command]
#pragma warning restore 618
        public void CmdSpawnSecondPlayerZone(float distanceTocenter, Quaternion rotation)
        {
            //We want the new position aligned with the anchor
            Vector3 position = new Vector3(0,0,distanceTocenter);

            GameObject PlayerFieldObject = Instantiate(SecondPlayerZone, position, rotation);

#pragma warning disable 618
            NetworkServer.Spawn(PlayerFieldObject);
#pragma warning restore 618
            if(OnObjectPlaced != null)
                    OnObjectPlaced("Opponent Placed. Set walls to the left and right");
        }
   }
    
}

