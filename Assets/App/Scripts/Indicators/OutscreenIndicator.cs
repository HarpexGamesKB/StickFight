using UnityEngine;
using UnityEngine.Events;

public class OutscreenIndicator : MonoBehaviour
{
    public enum Side
    {
        None,
        Top,
        Left,
        Right,
        Bottom,
        In
    }

    public event UnityAction<GameObject> OnBecomeInside;

    [SerializeField] private Sprite IndicatorSprite;

    [Space]
    [SerializeField] private ObjectMover ObjectMover;

    [SerializeField] private Transform Object;
    private OutscreenIndicatorPresenter _indicator;

    [Range(0f, 200f)]
    [SerializeField] private float Offset;

    [Range(0f, 200f)]
    [SerializeField] private float OffsetTop;

    private Camera Camera;
    private Vector2 Center;
    private Vector2 LeftUpCorner;
    private Vector2 RightUpCorner;
    private Vector2 LeftDownCorner;
    private Vector2 RightDownCorner;

    private void Start()
    {
        Camera = Camera.main;
        Center = Camera.WorldToScreenPoint(Vector3.zero);
        LeftDownCorner = new Vector2(Offset, Offset);
        LeftUpCorner = new Vector2(Offset, Screen.height - Offset - OffsetTop);
        RightDownCorner = new Vector2(Screen.width - Offset, Offset);
        RightUpCorner = new Vector2(Screen.width - Offset, Screen.height - Offset - OffsetTop);
    }

    private void FixedUpdate()
    {
        Do();
    }

    private void Do()
    {
        Vector2 position = Camera.WorldToScreenPoint(Object.position);

        Side side = CheckSide(position);

        if (side != Side.In && _indicator == null)
        {
            if (ObjectMover.InView)
            {
                ObjectMover.Destroy();
                return;
            }
            _indicator = OutscreenIndicatorManager.Instance.GetIndicator();
            _indicator.SetIcon(IndicatorSprite);
        }
        else if (side == Side.In && _indicator != null)
        {
            _indicator.gameObject.SetActive(false);
            _indicator = null;
            ObjectMover.StepInView();
            OnBecomeInside?.Invoke(gameObject);
            return;
        }
        if (_indicator == null) return;

        if (side == Side.Left && Tools.Cross(LeftDownCorner, LeftUpCorner, position, Center, out Vector2 result1)) // Left
        {
            _indicator.position = result1 - new Vector2(Screen.width / 2, Screen.height / 2);
        }
        else if (side == Side.Top && Tools.Cross(LeftUpCorner, RightUpCorner, position, Center, out Vector2 result2)) // Top
        {
            _indicator.position = result2 - new Vector2(Screen.width / 2, Screen.height / 2);
        }
        else if (side == Side.Right && Tools.Cross(RightUpCorner, RightDownCorner, position, Center, out Vector2 result3)) // Right
        {
            _indicator.position = result3 - new Vector2(Screen.width / 2, Screen.height / 2);
        }
        else if (side == Side.Bottom && Tools.Cross(LeftDownCorner, RightDownCorner, position, Center, out Vector2 result4)) // Bottom
        {
            _indicator.position = result4 - new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }

    private Side CheckSide(Vector3 position)
    {
        position.Split(out float x, out float Y, out float z);

        float y = Y;

        float x1 = LeftUpCorner.x;
        float y1 = LeftUpCorner.y;
        float x2 = RightDownCorner.x;
        float y2 = RightDownCorner.y;

        float xc = (x1 + x2) / 2;
        float yc = (y1 + y2) / 2;

        float m = Mathf.Abs((y2 - y1) / (x2 - x1));
        if (x > 0 && x < Screen.width && y > 0 && y < Screen.height)
        {
            return Side.In;
        }
        if (Mathf.Abs(y - yc) < m * (x - xc))
        {
            return Side.Right;
        }
        else if (Mathf.Abs(y - yc) < -m * (x - xc))
        {
            return Side.Left;
        }
        else if (y - yc > m * Mathf.Abs(x - xc))
        {
            return Side.Top;
        }
        else if (-(y - yc) > m * Mathf.Abs(x - xc))
        {
            return Side.Bottom;
        }
        return default;
    }

    private void OnDestroy()
    {
        OnBecomeInside = null;
    }
}