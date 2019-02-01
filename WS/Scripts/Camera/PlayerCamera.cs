using UnityEngine;

namespace WS
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        public Transform playerTrans;

        private Vector3 lastpos;

        private Camera camera;

        private float z;

        [SerializeField]
        private float MinView = 200;
        [SerializeField]
        private float MaxView = 500;
        [SerializeField]
        private float DefaultView = 350;
        [SerializeField] 
        private float Sensitive = 50;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("multi player camera, destroy old");
                Destroy(Instance.gameObject);
            }
            Instance = this;
            this.camera = this.GetComponent<Camera>();
            this.camera.orthographicSize = DefaultView;
        }

        public void InitPlayer(Transform p)
        {
            playerTrans = p;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                Zoom(50f);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                Zoom(-50f);
            }
            var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheel != 0)
            {
                Zoom(-mouseWheel * Sensitive);
            }
            //
            if (playerTrans == null) return;
            if (playerTrans.position == lastpos) return;
            var p = playerTrans.position;
            this.transform.position = new Vector3(p.x, p.y);

   
        }

        public void Zoom(float v)
        {
            var newv = Mathf.Clamp(this.camera.orthographicSize + v, MinView, MaxView);
            this.camera.orthographicSize = newv;
        }

    }
}
