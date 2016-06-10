 using System;
using UnityEngine;

public class MinetrainTrackGenerator : MeshGenerator
{
    public static readonly float[] BeamSizeVariation = {
            .318f,
            .436f,
            .292f,
            .35f,
            .243f,
            .415f,
            .428f,
            .340f,
            .39f,
            .355f,
            .393f,
            .28f,
            .247f,
            .414f,
            .328f,
            .495f,
            .298f,
            .492f,
            .429f,
            .232f,
            .356f,
            .374f,
            .472f,
            .285f,
            .472f
    };

    private const float railSize = 0.022835f;

    private const int sideTubesVertCount = 8;

    private const float buildVolumeHeight = 0.7f;

    public const float SupoortBeamSize = .0728281f;

    private const float SupportBoxArea = .7f;

    private BoxExtruder leftRailExtruder;

    private BoxExtruder rightRailExtruder;

    private BoxExtruder collisionMeshExtruder;

    private BoxExtruder[] TrackBeamExtruder;

    private BoxExtruder SupportBeamExtruder;

    private BoxExtruder SupportBottomBeamExtruder;

    protected override void Initialize()
    {
        base.Initialize();
        base.trackWidth = 0.25f;
        this.crossBeamSpacing = 0.5f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = this.material;
        this.leftRailExtruder = new BoxExtruder(railSize, railSize);
        this.leftRailExtruder.setUV(15, 14);
        this.rightRailExtruder = new BoxExtruder(railSize, railSize);
        this.rightRailExtruder.setUV(15, 14);

        SupportBeamExtruder = new BoxExtruder (SupoortBeamSize, SupoortBeamSize);
        SupportBeamExtruder.setUV (14, 15);
        SupportBeamExtruder.closeEnds = true;

        SupportBottomBeamExtruder = new BoxExtruder (SupoortBeamSize * 2.0f, SupoortBeamSize);
        SupportBottomBeamExtruder.setUV (14, 15);
        SupportBottomBeamExtruder.closeEnds = true;


        TrackBeamExtruder = new BoxExtruder[4];
        TrackBeamExtruder [0] = new BoxExtruder (.145123f, railSize);
        TrackBeamExtruder [1] = new BoxExtruder (0.0448f, railSize);
        TrackBeamExtruder [2] = new BoxExtruder (0.125123f,railSize);
        TrackBeamExtruder [3] = new BoxExtruder (0.09f, railSize);

        for (int x = 0; x < 4; x++) {
            TrackBeamExtruder [x].setUV (15, 15);
            TrackBeamExtruder [x].closeEnds = true;
        }
       
        this.collisionMeshExtruder = new BoxExtruder(base.trackWidth, 0.022835f);
        this.buildVolumeMeshExtruder = new BoxExtruder(base.trackWidth, 0.7f);
        this.buildVolumeMeshExtruder.closeEnds = true;
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(t), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;

        Vector3 leftRail = trackPivot + binormal * base.trackWidth / 2f;
        Vector3 rightRail = trackPivot - binormal * base.trackWidth / 2f;
        Vector3 midPoint = trackPivot + normal * this.getCenterPointOffsetY();

        this.leftRailExtruder.extrude(leftRail, tangentPoint, normal);
        this.rightRailExtruder.extrude(rightRail, tangentPoint, normal);
        this.collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (this.liftExtruder != null)
        {
            this.liftExtruder.extrude(midPoint, tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
        int crossBeamIndex = 0;
        float pos = 0.0f;
        //adds random wood planks as supports
        while (pos < trackSegment.getLength ()) {
            float tForDistance = trackSegment.getTForDistance(pos);

            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangetPoint = trackSegment.getTangentPoint (tForDistance);
            Vector3 binormal = Vector3.Cross (normal, tangetPoint).normalized;
            Vector3 pivot = base.getTrackPivot (trackSegment.getPoint (tForDistance), normal);

            float crossBeamOffset = BeamSizeVariation [crossBeamIndex % BeamSizeVariation.Length];
            BoxExtruder selectedCrossBeamExtruder = TrackBeamExtruder [(crossBeamIndex + 10) % TrackBeamExtruder.Length];

            Vector3 left =  tangetPoint.normalized * (selectedCrossBeamExtruder.width / 2.0f) + Vector3.down * railSize + pivot + binormal * (base.trackWidth + crossBeamOffset) / 2f;
            Vector3 right = tangetPoint.normalized * (selectedCrossBeamExtruder.width / 2.0f) + Vector3.down * railSize + pivot - binormal * (base.trackWidth + crossBeamOffset) / 2f;

            pos += TrackBeamExtruder[(crossBeamIndex + 10) % TrackBeamExtruder.Length].width + .03f;
            if (pos > trackSegment.getLength ())
                break;

            selectedCrossBeamExtruder.extrude (left  , binormal * -1f, normal);
            selectedCrossBeamExtruder.extrude (right , binormal* -1f, normal);
            selectedCrossBeamExtruder.end ();

            crossBeamIndex++;
        }

        pos = 0.0f;
        float segments = trackSegment.getLength () / (float)Mathf.RoundToInt (trackSegment.getLength () / this.crossBeamSpacing);
        //inbetween supports for edge of track
        while (pos < trackSegment.getLength ()) {
            float tForDistance = trackSegment.getTForDistance(pos);

            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangetPoint = trackSegment.getTangentPoint (tForDistance);
            Vector3 binormal = Vector3.Cross (normal, tangetPoint).normalized;
            Vector3 pivot = base.getTrackPivot (trackSegment.getPoint (tForDistance), normal);


            float crossBeamOffset = BeamSizeVariation [crossBeamIndex % BeamSizeVariation.Length] * .3f;

            Vector3 left = pivot + binormal * (SupportBoxArea / 2.0f) + Vector3.down * (railSize +railSize + SupoortBeamSize/2.0f);
            Vector3 right = pivot - binormal * (SupportBoxArea/ 2.0f) + Vector3.down * (railSize +railSize  + SupoortBeamSize/2.0f);
  
            SupportBeamExtruder.extrude (left + binormal * crossBeamOffset , binormal * -1f, normal);
            SupportBeamExtruder.extrude (right - binormal * crossBeamOffset , binormal* -1f, normal);
            SupportBeamExtruder.end ();

            SupportBeamExtruder.extrude (left, normal, binormal);
            SupportBeamExtruder.extrude (new Vector3(left.x,Mathf.FloorToInt(left.y),left.z) , normal, binormal);
            SupportBeamExtruder.end ();

            SupportBeamExtruder.extrude (right, normal, binormal);
            SupportBeamExtruder.extrude (new Vector3(right.x,Mathf.FloorToInt(right.y),right.z) , normal, binormal);
            SupportBeamExtruder.end ();

            SupportBottomBeamExtruder.extrude (new Vector3(left.x,Mathf.FloorToInt(left.y),left.z)  + binormal * (.5f / 2.0f) , binormal * -1f, Vector3.down);
            SupportBottomBeamExtruder.extrude (new Vector3(right.x,Mathf.FloorToInt(right.y),right.z) - binormal * (.5f / 2.0f)  , binormal * -1f, Vector3.down);
            SupportBottomBeamExtruder.end ();
            

            pos += segments;
            crossBeamIndex++;
        }

    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return default(MeshCombiner).start().add(new Extruder[]
            {
                this.leftRailExtruder,
                this.rightRailExtruder,
                TrackBeamExtruder [0],
                TrackBeamExtruder [1],
                TrackBeamExtruder [2],
                TrackBeamExtruder [3],
                SupportBeamExtruder,
                SupportBottomBeamExtruder
            }).add(TrackBeamExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return this.collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return this.buildVolumeMeshExtruder;
    }

    public override float trackOffsetY()
    {
        return 0.2225f;
    }

    public override float getSupportOffsetY()
    {
        return 0.05f;
    }

    protected override float getTunnelOffsetY()
    {
        return 0.15f;
    }

    public override float getTunnelWidth()
    {
        return 0.7f;
    }

    public override float getTunnelHeight()
    {
        return 0.95f;
    }

    protected override float railHalfHeight()
    {
        return 0.022835f;
    }
}

