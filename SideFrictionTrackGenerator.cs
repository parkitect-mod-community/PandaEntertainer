 using System;
using UnityEngine;

public class SideFrictionTrackGenerator : MeshGenerator
{


    private BoxExtruder leftWoodenTrack;
    private BoxExtruder rightWoodenTrack;

    private BoxExtruder leftMinorWoodenTrack;
    private BoxExtruder rightMinorWoodenTrack;

    private BoxExtruder leftSideWoodenTrack;
    private BoxExtruder rightSideWoodenTrack;

    private BoxExtruder crossBeamSupport;

    private BoxExtruder collisionMeshExtruder;

    protected override void Initialize()
    {
        base.Initialize();
        base.trackWidth = 0.22263f * 2.0f;
        this.crossBeamSpacing = 0.5f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = this.material;
       
        leftWoodenTrack = new BoxExtruder (.09908f,.0250f);
        rightWoodenTrack = new BoxExtruder (.09908f,.0250f);

        leftMinorWoodenTrack = new BoxExtruder (.04421f, .0250f);
        rightMinorWoodenTrack = new BoxExtruder (.04421f, .0250f);

        leftSideWoodenTrack = new BoxExtruder (.0170f,.07714f);
        rightSideWoodenTrack = new BoxExtruder (.0170f,.07714f);

        crossBeamSupport = new BoxExtruder (.0550f, .0550f);


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

        Vector3 midPoint = trackPivot + normal * this.getCenterPointOffsetY();


        this.leftWoodenTrack.extrude(trackPivot + binormal * base.trackWidth / 2f, tangentPoint, normal);
        this.rightWoodenTrack.extrude(trackPivot - binormal * base.trackWidth / 2f, tangentPoint, normal);


        this.leftMinorWoodenTrack.extrude(trackPivot + binormal * .07304f , tangentPoint,normal);
        this.rightMinorWoodenTrack.extrude(trackPivot - binormal * .07304f, tangentPoint,normal);

        this.leftSideWoodenTrack.extrude(trackPivot - normal * .1023f  + binormal * (-(leftSideWoodenTrack.width/2.0f) + (base.trackWidth / 2f) + (this.leftWoodenTrack.width /2.0f)),tangentPoint,normal);
        this.rightSideWoodenTrack.extrude(trackPivot - normal * .1023f - binormal * (-(leftSideWoodenTrack.width/2.0f) + (base.trackWidth / 2f) + (this.rightWoodenTrack.width /2.0f)),tangentPoint,normal);


        this.collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (this.liftExtruder != null)
        {
            this.liftExtruder.extrude(midPoint, tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);

        float sample = trackSegment.getLength() / (float)Mathf.RoundToInt(trackSegment.getLength()/ this.crossBeamSpacing);
        float pos = 0.0f;
        int index = 0;
        while(pos < trackSegment.getLength())
        {
            index++;
            pos += sample;

            float tForDistance = trackSegment.getTForDistance (pos);
            Vector3 normal = trackSegment.getNormal (tForDistance);
            Vector3 tangetPoint = trackSegment.getTangentPoint (tForDistance);
            Vector3 binormal = Vector3.Cross (normal, tangetPoint).normalized;
            Vector3 pivot = base.getTrackPivot (trackSegment.getPoint (tForDistance), normal);


        }

    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        


        return default(MeshCombiner).start().add(new Extruder[]
            {
                leftWoodenTrack,
                rightWoodenTrack,
                leftMinorWoodenTrack,
                rightMinorWoodenTrack,
                leftSideWoodenTrack,
                rightSideWoodenTrack
               
            }).end(putMeshOnGO.transform.worldToLocalMatrix);
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

