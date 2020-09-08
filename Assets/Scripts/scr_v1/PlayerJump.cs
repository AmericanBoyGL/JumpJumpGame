using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    //направление наведения шарика
    Vector3 delta = Vector3.zero;
    Vector3 lastPos = Vector3.zero;

    //направление силы натягивания шарика
    Vector3 startPos;
    Vector3 endPos;
    Camera camera;
    LineRenderer lr;

    Vector3 camOffset = new Vector3(0, 0, 10);

    [SerializeField] AnimationCurve ac;

    public PlayerStart SelectedPlayerHelper { get; set; }

    //linerender
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    int dotsCount = 10;
    [SerializeField]
    float speed = 10;
    float gravity;

    public TrajectoryController Trajectory;
    //
    private readonly Vector2 LAUNCH_VELOCITY = new Vector2(20f, 80f);
    private Vector2 INITIAL_POSITION = Vector2.zero;
    private readonly Vector2 GRAVITY = new Vector2(0f, -240f);
    private const float DELAY_UNTIL_LAUNCH = 1f;
    private readonly int NUM_DOTS_TO_SHOW = 5;

    [SerializeField] float Pushforce = 10f;
    public Vector2 force;
    public Vector2 direction;
    public float distance;
    public GameObject trajectoryDotPrefab;
    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        gravity = Physics2D.gravity.y;
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        
    }

    void Update()
    {
       // Trajectory.test();
        //вызов траектории(доделать)
        //Trajectory.ShowTrajectory(transform.position, Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            
            //линия натяжки
            if (lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }

            lr.enabled = true;
            lr.positionCount = 2;
            startPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            //startPos = camera.ScreenToWorldPoint(gameObject.transform.localPosition) + camOffset;

            lr.SetPosition(0, startPos);
            lr.useWorldSpace = true;
            lr.widthCurve = ac;
            lr.numCapVertices = 10;

           
        }
        else if (Input.GetMouseButton(0))
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            delta = Input.mousePosition - lastPos;

            //Debug.Log("Delta Distance From Starting Position : " + delta.magnitude);
            Debug.DrawLine(transform.position, delta * -1, Color.red, 1f);
            var objs = GameObject.FindGameObjectsWithTag("Dot"); // возвращает МАССИВ!
            for (int i = 0; i < objs.Length; i++)
                Destroy(objs[i]);

            endPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            distance = Vector2.Distance(startPos, endPos);
            direction = (startPos - endPos).normalized;
            force = direction * distance * Pushforce;
            Debug.Log("force = " + force * -1);

            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                if(GameObject.FindGameObjectsWithTag("Dot").Length < 20)
                {
                    GameObject trajectoryDot = Instantiate(trajectoryDotPrefab);
                    trajectoryDot.transform.position = CalculatePosition(0.05f * i, gameObject.transform.position , force);
                    //Debug.Log("Calc = " + CalculatePosition(0.05f * i, gameObject.transform.position, delta * -1));
                }
            }
            //multiply delta.magnitude times the force you want to apply to the ball and bam! magic!
            //Trajectory.ShowTrajectory(gameObject.transform.position, gameObject.transform.up);
            
            lr.SetPosition(1, endPos);

           

        }

        if (Input.GetMouseButtonUp(0))
        {
            
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            //gameObject.GetComponent<Rigidbody2D>().AddForce(delta * -1);
            gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            //Debug.Log("delta = " + delta * -1 * 0.05f * 10);
            //gameObject.GetComponent<Rigidbody2D>().velocity = delta * -1; //Чем долше зажимаешь тем сильнее летить
            lr.enabled = false;
        }
    }

    private Vector2 CalculatePosition(float elapsedTime, Vector2 INITIAL_POSITION, Vector2 LAUNCH_VELOCITY)
    {
        return GRAVITY * elapsedTime * elapsedTime * 0.5f +
                   LAUNCH_VELOCITY * elapsedTime + INITIAL_POSITION;
        //return INITIAL_POSITION + LAUNCH_VELOCITY * elapsedTime + GRAVITY * elapsedTime * elapsedTime / 2;
    }
}
