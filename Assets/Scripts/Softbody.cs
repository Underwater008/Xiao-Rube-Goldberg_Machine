using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Softbody : MonoBehaviour
{
    #region Constants
    private const float splineOffset = 0.5f;
    #endregion

    #region Fields
    [SerializeField]
    public SpriteShapeController spriteShape;
    [SerializeField]
    public Transform[] points;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        UpdatVerticies();
    }
    private void Update()
    {
        UpdatVerticies();
    }
    #endregion

    #region privateMethods
    private void UpdatVerticies()
    {
        for(int i = 0; i< points.Length - 1; i++)
        {
            Vector2 _vertex = points[i].localPosition;

            Vector2 _towardsCenter = (Vector2.zero - _vertex).normalized;

            float _colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try
            {
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * _colliderRadius));
            }
            catch
            {
                Debug.Log("Spline points are too close to each other.. recalculate");
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_colliderRadius + splineOffset)));
            }

            Vector2 _1t = spriteShape.spline.GetLeftTangent(i);

            Vector2 _newRt = Vector2.Perpendicular(_towardsCenter) * _1t.magnitude;
            Vector2 _newLt = Vector2.zero - (_newRt);

            spriteShape.spline.SetRightTangent(i, _newRt);
            spriteShape.spline.SetLeftTangent(i, _newLt);
        }
    }
    #endregion
}
