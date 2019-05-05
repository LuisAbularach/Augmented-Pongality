//-----------------------------------------------------------------------
// <copyright file="CloudAnchorsExampleController.cs" company="Google">
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
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
    #endif

    /// <summary>
    /// Controller for the Cloud Anchors Example. Handles the ARCore lifecycle.
    /// </summary>
    public class CloudAnchorsExampleController : MonoBehaviour
    {
        [Header("ARCore")]

        //Button that will appear once the ball is spawned
        public GameObject StartButton;
    
        //Size data
        bool P2ready;
        //These are references to the back and fronts of the player fields
        float P2front, P2back;
        bool canPlaceWall;
        public GameObject Player2Zone;
        public GameObject SinglePlayerField;

        public GameObject LeftWall,RightWall;

        public GameObject SpawnedBall;
        /// <summary>
        /// The UI Controller.
        /// </summary>
        public NetworkManagerUIController UIController;

        /// <summary>
        /// The root for ARCore-specific GameObjects in the scene.
        /// </summary>
        public GameObject ARCoreRoot;

        /// <summary>
        /// The helper that will calculate the World Origin offset when performing a raycast or
        /// generating planes.
        /// </summary>
        public ARCoreWorldOriginHelper ARCoreWorldOriginHelper;

        [Header("ARKit")]

        /// <summary>
        /// The root for ARKit-specific GameObjects in the scene.
        /// </summary>
        public GameObject ARKitRoot;

        /// <summary>
        /// The first-person camera used to render the AR background texture for ARKit.
        /// </summary>
        public Camera ARKitFirstPersonCamera;

        /// <summary>
        /// A helper object to ARKit functionality.
        /// </summary>
        private ARKitHelper m_ARKit = new ARKitHelper();

        /// <summary>
        /// Indicates whether the Origin of the new World Coordinate System, i.e. the Cloud Anchor,
        /// was placed.
        /// </summary>
        private bool m_IsOriginPlaced = false;

        /// <summary>
        /// Indicates whether the Anchor was already instantiated.
        /// </summary>
        private bool m_AnchorAlreadyInstantiated = false;

        /// <summary>
        /// Indicates whether the Cloud Anchor finished hosting.
        /// </summary>
        private bool m_AnchorFinishedHosting = false;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The anchor component that defines the shared world origin.
        /// </summary>
        private Component m_WorldOriginAnchor = null;

        /// <summary>
        /// The last pose of the hit point from AR hit test.
        /// </summary>
        private Pose? m_LastHitPose = null;

        /// <summary>
        /// The current cloud anchor mode.
        /// </summary>
        private ApplicationMode m_CurrentMode = ApplicationMode.Ready;

        //Number of walls
        private int wall_Count;

        /// <summary>
        /// The Network Manager.
        /// </summary>
#pragma warning disable 618
        private NetworkManager m_NetworkManager;
#pragma warning restore 618

        private bool Started;

        private bool m_MatchStarted = false;

        /// <summary>
        /// Enumerates modes the example application can be in.
        /// </summary>
        public enum ApplicationMode
        {
            Ready,
            Hosting,
            Resolving,
        }

        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            P2back = 0;
#pragma warning disable 618
            m_NetworkManager = UIController.GetComponent<NetworkManager>();
#pragma warning restore 618

            wall_Count = 0;

            // A Name is provided to the Game Object so it can be found by other Scripts
            // instantiated as prefabs in the scene.
            gameObject.name = "CloudAnchorsExampleController";
            ARCoreRoot.SetActive(false);
            ARKitRoot.SetActive(false);
            _ResetStatus();
        }
        //***********************************************************************************************//
        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        //***********************************************************************************************//
        public void Update()
        {
            _UpdateApplicationLifecycle();

            //Activete button that will start the game if two walls are placed and it has not been activated
            if(wall_Count == 2 && StartButton.activeSelf == false && m_CurrentMode == ApplicationMode.Hosting && !Started)
            {
                Started = true;
                StartButton.SetActive(true);
            }

            //If not host search for objects
            if(m_CurrentMode != ApplicationMode.Hosting && wall_Count <=2)
            {
                Player2Zone = GameObject.Find("Player2Zone(Clone)");
                if(Player2Zone!=null)
                {
                    P2back = Player2Zone.transform.position.z + 0.5f;
                    Debug.Log("GOT P2Back distance: " + P2back);
                    //Destroy single player objects
                }


                GameObject wall = GameObject.Find("Wall(Clone)");
                if(wall!=null){
                    if(wall.transform.position.x < 0 && LeftWall == null)
                    {
                        Debug.Log("LEFT WALL FOUND");
                        LeftWall = wall;
                        LeftWall.name = "LeftWall";
                        float x = LeftWall.transform.localScale.x;
                        float y = LeftWall.transform.localScale.y;
                        LeftWall.transform.localScale = new Vector3(x,y,P2back);
                        wall_Count += 1;
                    }
                    else if(RightWall == null)
                    {
                        Debug.Log("RIGHT WALL FOUND");
                        RightWall = wall;
                        RightWall.name = "RightWall";
                        float x = RightWall.transform.localScale.x;
                        float y = RightWall.transform.localScale.y;
                        RightWall.transform.localScale = new Vector3(x,y,P2back);
                        wall_Count += 1;
                    }
                }
            }

            // If we are neither in hosting nor resolving mode then the update is complete.
            if (m_CurrentMode != ApplicationMode.Hosting &&
                m_CurrentMode != ApplicationMode.Resolving)
            {
                return;
            }
        
            // If the origin anchor has not been placed yet, then update in resolving mode is
            // complete.
            if (m_CurrentMode == ApplicationMode.Resolving && !m_IsOriginPlaced)
            {
                return;
            }

            // If the player has not touched the screen then the update is complete.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

        //touch
            // if(wall_Count == 2 && SpawnedBall!=null)
            // {
            //     SpawnedBall.GetComponent<Ball>().StartBallMovement();
            //     SpawnedBall.GetComponent<Ball>().CmdsetP2Back(P2back);
            //     if(m_CurrentMode != ApplicationMode.Hosting)
            //         SpawnedBall.GetComponent<Ball>().isP2 = true;
                    
            // }
            TrackableHit arcoreHitResult = new TrackableHit();
            m_LastHitPose = null;

            // Raycast against the location the player touched to search for planes.
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                if (ARCoreWorldOriginHelper.Raycast(touch.position.x, touch.position.y,
                        TrackableHitFlags.PlaneWithinPolygon, out arcoreHitResult))
                {
                    m_LastHitPose = arcoreHitResult.Pose;
                }
            }
            else
            {
                Pose hitPose;
                if (m_ARKit.RaycastPlane(
                    ARKitFirstPersonCamera, touch.position.x, touch.position.y, out hitPose))
                {
                    m_LastHitPose = hitPose;
                }
            }

            // If there was an anchor placed, then instantiate the corresponding object.
            if (m_LastHitPose != null && m_CurrentMode == ApplicationMode.Hosting)
            {                
                // The first touch on the Hosting mode will instantiate the origin anchor (p1 field). Any
                // subsequent touch will instantiate a wall, both in Hosting and Resolving modes.
                if (_CanPlaceStars() && wall_Count!=2)
                {
                    //touch eveyone not host
                    //if(m_CurrentMode != ApplicationMode.Hosting)
                    if(!canPlaceWall)
                    {
                        Debug.Log("Place the field for P2");
                        _InstantiatePlayer2Zone();  
                    }
                    else{
                        _InstantiateWall();
                        wall_Count += 1;
                        Debug.Log("Wall Count" + wall_Count);
                    }
                }
                else if (!m_IsOriginPlaced && m_CurrentMode == ApplicationMode.Hosting)
                {
                    
                    if (Application.platform != RuntimePlatform.IPhonePlayer)
                    {
                        m_WorldOriginAnchor =
                            arcoreHitResult.Trackable.CreateAnchor(arcoreHitResult.Pose);
                    }
                    else
                    {
                        m_WorldOriginAnchor = m_ARKit.CreateAnchor(m_LastHitPose.Value);
                    }

                    SetWorldOrigin(m_WorldOriginAnchor.transform);
                    _InstantiateAnchor();
                    OnAnchorInstantiated(true);
                }
            }
        }

        /// <summary>
        /// Sets the apparent world origin so that the Origin of Unity's World Coordinate System
        /// coincides with the Anchor. This function needs to be called once the Cloud Anchor is
        /// either hosted or resolved.
        /// </summary>
        /// <param name="anchorTransform">Transform of the Cloud Anchor.</param>
        public void SetWorldOrigin(Transform anchorTransform)
        {
            if (m_IsOriginPlaced)
            {
                Debug.LogWarning("The World Origin can be set only once.");
                return;
            }

            m_IsOriginPlaced = true;

            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                ARCoreWorldOriginHelper.SetWorldOrigin(anchorTransform);
            }
            else
            {
                m_ARKit.SetWorldOrigin(anchorTransform);
            }
        }

        /// <summary>
        /// Handles user intent to enter a mode where they can place an anchor to host or to exit
        /// this mode if already in it.
        /// </summary>
        public void OnEnterHostingModeClick()
        {
            if (m_CurrentMode == ApplicationMode.Hosting)
            {
                m_CurrentMode = ApplicationMode.Ready;
                _ResetStatus();
                return;
            }

            m_CurrentMode = ApplicationMode.Hosting;
            _SetPlatformActive();
        }

        /// <summary>
        /// Handles a user intent to enter a mode where they can input an anchor to be resolved or
        /// exit this mode if already in it.
        /// </summary>
        public void OnEnterResolvingModeClick()
        {
            if (m_CurrentMode == ApplicationMode.Resolving)
            {
                m_CurrentMode = ApplicationMode.Ready;
                _ResetStatus();
                return;
            }

            m_CurrentMode = ApplicationMode.Resolving;
            _SetPlatformActive();
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was instantiated and the host request was
        /// made.
        /// </summary>
        /// <param name="isHost">Indicates whether this player is the host.</param>
        public void OnAnchorInstantiated(bool isHost)
        {
            if (m_AnchorAlreadyInstantiated)
            {
                return;
            }

            m_AnchorAlreadyInstantiated = true;
            UIController.OnAnchorInstantiated(isHost);
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was hosted.
        /// </summary>
        /// <param name="success">If set to <c>true</c> indicates the Cloud Anchor was hosted
        /// successfully.</param>
        /// <param name="response">The response string received.</param>
        public void OnAnchorHosted(bool success, string response)
        {
            m_AnchorFinishedHosting = success;
            UIController.OnAnchorHosted(success, response);
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was resolved.
        /// </summary>
        /// <param name="success">If set to <c>true</c> indicates the Cloud Anchor was resolved
        /// successfully.</param>
        /// <param name="response">The response string received.</param>
        public void OnAnchorResolved(bool success, string response)
        {
            UIController.OnAnchorResolved(success, response);
        }

        /// <summary>
        /// Instantiates the anchor object at the pose of the m_LastPlacedAnchor Anchor. This will
        /// host the Cloud Anchor.
        /// </summary>
        private void _InstantiateAnchor()
        {
            // The anchor will be spawned by the host, so no networking Command is needed.
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                .SpawnAnchor(Vector3.zero, Quaternion.identity, m_WorldOriginAnchor);

            //Spawn the field for single player
            //Instantiate(SinglePlayerField, new Vector3(0,0,0.5f),Quaternion.Euler(0,0,0));
        }

        /// <summary>
        /// Instantiates a star object that will be synchronized over the network to other clients.
        /// </summary>
        private void _InstantiateWall()
        {
            // Wall is spawned in the server so a networking Command is used.
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                .CmdSpawnWall(m_LastHitPose.Value.position, Quaternion.Euler(0,0,0), 1 + P2back);

            Debug.Log("Spawning Ball");
            if(wall_Count==1){
            //Ball
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                .CmdSpawnBall(m_LastHitPose.Value.position, Quaternion.Euler(0,0,0),P2back);
            SpawnedBall = GameObject.Find("Ball(Clone)");
            }
        }

        private void _InstantiatePlayer2Zone()
        {
            Vector3 P2 = m_LastHitPose.Value.position;
            //Check to see if z position is at least half a meter away from p1
            //if(P2.z - 0.5f >= 0.0f){
                GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                    .CmdSpawnSecondPlayerZone( P2.z, Quaternion.Euler(0,0,0));
                P2back = P2.z + 0.5f;
                P2front = P2.z - 0.5f;

                canPlaceWall = true;
                Debug.Log("Zone Placed!");  
            //}
            // else
            // {
            //     //Not far enough give error 
            //     P2back = 0;
            // }
        }

        /// <summary>
        /// Sets the corresponding platform active.
        /// </summary>
        private void _SetPlatformActive()
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                ARCoreRoot.SetActive(true);
                ARKitRoot.SetActive(false);
            }
            else
            {
                ARCoreRoot.SetActive(false);
                ARKitRoot.SetActive(true);
            }
        }

        /// <summary>
        /// Indicates whether a star can be placed.
        /// </summary>
        /// <returns><c>true</c>, if stars can be placed, <c>false</c> otherwise.</returns>
        private bool _CanPlaceStars()
        {
            if (m_CurrentMode == ApplicationMode.Resolving)
            {
                return m_IsOriginPlaced;
            }

            if (m_CurrentMode == ApplicationMode.Hosting)
            {
                return m_IsOriginPlaced && m_AnchorFinishedHosting;
            }

            return false;
        }

        /// <summary>
        /// Resets the internal status.
        /// </summary>
        private void _ResetStatus()
        {
            // Reset internal status.
            m_CurrentMode = ApplicationMode.Ready;
            if (m_WorldOriginAnchor != null)
            {
                Destroy(m_WorldOriginAnchor.gameObject);
            }

            m_WorldOriginAnchor = null;
        }

        /// <summary>
        /// Begin the Game and set button inactive again
        /// </summary>
        public void _StartGame()
        {
            Destroy(StartButton);
            SpawnedBall.GetComponent<Ball>().StartBallMovement();
            SpawnedBall.GetComponent<Ball>().CmdsetP2Back(P2back);
            SpawnedBall.GetComponent<Ball>().isP2 = true;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            if (!m_MatchStarted && m_NetworkManager.IsClientConnected())
            {
                m_MatchStarted = true;
            }

            // Restart scene when the 'back' button is pressed if not on lobby screen.
            if (Input.GetKey(KeyCode.Escape))
            {
                // GameObject.Destroy(GameObject.Find("NetworkManagerObject"));
                Application.Quit();
                //  SceneManager.LoadScene("Augmented-Pongality", LoadSceneMode.Single);
            }

            var sleepTimeout = SleepTimeout.NeverSleep;

#if !UNITY_IOS
            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                sleepTimeout = lostTrackingSleepTimeout;
            }
#endif

            Screen.sleepTimeout = sleepTimeout;

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                UIController.ShowErrorMessage(
                    "Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 5.0f);
            }
            else if (Session.Status.IsError())
            {
                UIController.ShowErrorMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 5.0f);
            }
            else if (m_MatchStarted && !m_NetworkManager.IsClientConnected())
            {
                UIController.ShowErrorMessage(
                    "Network session disconnected!  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 5.0f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }
    }
}
