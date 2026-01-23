using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(SplineContainer))]
public class PathGenerator : MonoBehaviour
{
    public Transform trackParent;
    private SplineContainer splineContainer;

    void Start()
    {
        splineContainer = GetComponent<SplineContainer>();
        GenerateSpline();
    }

    [ContextMenu("Generate Spline")]
    public void GenerateSpline()
    {
        var spline = splineContainer.Spline;
        spline.Clear();

        foreach (Transform child in trackParent)
        {
            BezierKnot knot = new BezierKnot();
            knot.Position = transform.InverseTransformPoint(child.position);


            spline.Add(knot, TangentMode.AutoSmooth);
        }

        TightenCurve(0.3f);
        spline.Closed = true;
    }

    void TightenCurve(float tension = 0.5f)
    {
        var spline = splineContainer.Spline;
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];

            knot.TangentIn *= tension;
            knot.TangentOut *= tension;

            spline[i] = knot;
        }
    }
}