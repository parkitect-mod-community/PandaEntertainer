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
    private BoxExtruder AngledCrossBeamSupport;

    private BoxExtruder collisionMeshExtruder;

    private BoxExtruder CrossBeamRailSupportLeft;
    private BoxExtruder CrossBeamRailSupportRight;

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
        leftWoodenTrack.setUV (15, 15);
        rightWoodenTrack.setUV (15, 15);


        leftMinorWoodenTrack = new BoxExtruder (.04421f, .0250f);
        rightMinorWoodenTrack = new BoxExtruder (.04421f, .0250f);
        rightMinorWoodenTrack.setUV (14, 15);
        leftMinorWoodenTrack.setUV (14, 15);


        leftSideWoodenTrack = new BoxExtruder (.0170f,.07714f);
        rightSideWoodenTrack = new BoxExtruder (.0170f,.07714f);
        rightSideWoodenTrack.setUV (15, 14);
        leftSideWoodenTrack.setUV (15, 14);


        crossBeamSupport = new BoxExtruder (.0550f, .0550f);
        crossBeamSupport.closeEnds = true;
        crossBeamSupport.setUV (15, 15);

        CrossBeamRailSupportLeft = new BoxExtruder (.0560f, .0560f);
        CrossBeamRailSupportRight = new BoxExtruder (.0560f, .0560f);
        CrossBeamRailSupportLeft.setUV (15, 15);
        CrossBeamRailSupportRight.setUV (15, 15);

        AngledCrossBeamSupport = new BoxExtruder (.0110f, .06967f);
        AngledCrossBeamSupport.setUV (15, 15);

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

        Vector3 binormalFlat = Vector3.Cross(Vector3.up, tangentPoint).normalized;


        Vector3 midPoint = trackPivot + normal * this.getCenterPointOffsetY();


        this.leftWoodenTrack.extrude(trackPivot + binormal * base.trackWidth / 2f, tangentPoint, normal);
        this.rightWoodenTrack.extrude(trackPivot - binormal * base.trackWidth / 2f, tangentPoint, normal);


        this.leftMinorWoodenTrack.extrude(trackPivot + binormal * .07304f , tangentPoint,normal);
        this.rightMinorWoodenTrack.extrude(trackPivot - binormal * .07304f, tangentPoint,normal);

        this.leftSideWoodenTrack.extrude(trackPivot - normal * .1323f  + binormal * (-(leftSideWoodenTrack.width/2.0f) + (base.trackWidth / 2f) + (this.leftWoodenTrack.width /2.0f)),tangentPoint,normal);
        this.rightSideWoodenTrack.extrude(trackPivot - normal * .1323f - binormal * (-(leftSideWoodenTrack.width/2.0f) + (base.trackWidth / 2f) + (this.rightWoodenTrack.width /2.0f)),tangentPoint,normal);

        CrossBeamRailSupportLeft.extrude (trackPivot + binormalFlat * .5f - Vector3.up * (trackOffsetY()), tangentPoint, binormalFlat);
        CrossBeamRailSupportRight.extrude (trackPivot - binormalFlat * .5f - Vector3.up * (trackOffsetY()), tangentPoint, binormalFlat);

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
            Vector3 tangentPoint = trackSegment.getTangentPoint (tForDistance);
            Vector3 binormal = Vector3.Cross (normal, tangentPoint).normalized;
            Vector3 trackPivot = base.getTrackPivot (trackSegment.getPoint (tForDistance), normal);
            Vector3 binormalFlat = Vector3.Cross(Vector3.up, tangentPoint).normalized;

            //vertical segment
            crossBeamSupport.extrude (trackPivot + normal * crossBeamSupport.width + (binormal * (base.trackWidth / 2f + leftWoodenTrack.width/2.0f  + crossBeamSupport.width/2.0f)), normal* -1f, binormal);
            crossBeamSupport.extrude (trackPivot - normal * .2841f + normal * crossBeamSupport.width + (binormal * (base.trackWidth / 2f + rightWoodenTrack.width/2.0f + crossBeamSupport.width/2.0f)), normal* -1f, binormal);
            crossBeamSupport.end ();

            crossBeamSupport.extrude (trackPivot + normal * crossBeamSupport.width - (binormal * (base.trackWidth / 2f + leftWoodenTrack.width/2.0f  + crossBeamSupport.width/2.0f)), normal * -1f, binormal);
            crossBeamSupport.extrude (trackPivot - normal * .2841f + normal * crossBeamSupport.width - (binormal * (base.trackWidth / 2f + rightWoodenTrack.width/2.0f + crossBeamSupport.width/2.0f)),normal* -1f, binormal);
            crossBeamSupport.end ();

            //cross beams
            crossBeamSupport.extrude (trackPivot + normal * crossBeamSupport.width + binormal * .5f, -1f * binormal, normal);
            crossBeamSupport.extrude (trackPivot + normal * crossBeamSupport.width - binormal * .5f, -1f * binormal, normal);
            crossBeamSupport.end ();

            if (!(trackSegment is Station)) {
                
                //angled segments
                AngledCrossBeamSupport.extrude (trackPivot - normal * (.2841f - AngledCrossBeamSupport.height / 2.0f) + normal * (crossBeamSupport.width) + (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) - (binormal * (base.trackWidth / 2f + leftWoodenTrack.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), binormal * -1f, normal);
                Vector3 tangentAngleVectorLeft = rotateNormalAxis (tangentPoint, binormal, -Mathf.Deg2Rad * 25f);
                AngledCrossBeamSupport.extrude (trackPivot + normal * (crossBeamSupport.width + .1f) + (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) - (binormal * (.5f + .05f)), tangentAngleVectorLeft.normalized * -1f, Vector3.Cross (tangentAngleVectorLeft, tangentPoint).normalized * -1f);
                AngledCrossBeamSupport.end ();

                AngledCrossBeamSupport.extrude (trackPivot - normal * (.2841f - AngledCrossBeamSupport.height / 2.0f) + normal * (crossBeamSupport.width) - (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) - (binormal * (base.trackWidth / 2f + leftWoodenTrack.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), binormal * -1f, normal);
                AngledCrossBeamSupport.extrude (trackPivot + normal * (crossBeamSupport.width + .1f) - (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) - (binormal * (.5f + .05f)), tangentAngleVectorLeft.normalized * -1f, Vector3.Cross (tangentAngleVectorLeft, tangentPoint).normalized * -1f);
                AngledCrossBeamSupport.end ();

                AngledCrossBeamSupport.extrude (trackPivot - normal * (.2841f - AngledCrossBeamSupport.height / 2.0f) + normal * (crossBeamSupport.width) + (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) + (binormal * (base.trackWidth / 2f + leftWoodenTrack.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), binormal, normal);
                Vector3 tangentAngleVectorright = rotateNormalAxis (tangentPoint, binormal, Mathf.Deg2Rad * 25f);
                AngledCrossBeamSupport.extrude (trackPivot + normal * (crossBeamSupport.width + .1f) + (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) + (binormal * (.5f + .05f)), tangentAngleVectorright.normalized, Vector3.Cross (tangentAngleVectorright, tangentPoint).normalized * -1f);
                AngledCrossBeamSupport.end ();

                AngledCrossBeamSupport.extrude (trackPivot - normal * (.2841f - AngledCrossBeamSupport.height / 2.0f) + normal * (crossBeamSupport.width) - (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) + (binormal * (base.trackWidth / 2f + leftWoodenTrack.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), binormal, normal);
                AngledCrossBeamSupport.extrude (trackPivot + normal * (crossBeamSupport.width + .1f) - (tangentPoint * (crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)) + (binormal * (.5f + .05f)), tangentAngleVectorright.normalized, Vector3.Cross (tangentAngleVectorright, tangentPoint).normalized * -1f);
                AngledCrossBeamSupport.end ();
        
                if (index % 2 == 0) {
                    //secondary vertical

                    float angle = Mathf.Deg2Rad * AngleSigned (normal, Vector3.down, tangentPoint);

                    crossBeamSupport.extrude (trackPivot + binormalFlat * .5f + rotateNormalAxis (tangentPoint, normal, angle) * trackOffsetY () - (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.extrude (trackPivot + binormalFlat * .5f + normal * (-.4015f + trackOffsetY ()) - (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.end ();

                    crossBeamSupport.extrude (trackPivot + binormalFlat * .5f + rotateNormalAxis (tangentPoint, normal, angle) * trackOffsetY () + (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.extrude (trackPivot + binormalFlat * .5f + normal * (-.4015f + trackOffsetY ()) + (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.end ();


                    crossBeamSupport.extrude (trackPivot - binormalFlat * .5f + rotateNormalAxis (tangentPoint, normal, angle) * trackOffsetY () - (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.extrude (trackPivot - binormalFlat * .5f + normal * (-.4015f + trackOffsetY ()) - (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.end ();

                    crossBeamSupport.extrude (trackPivot - binormalFlat * .5f + rotateNormalAxis (tangentPoint, normal, angle) * trackOffsetY () + (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.extrude (trackPivot - binormalFlat * .5f + normal * (-.4015f + trackOffsetY ()) + (tangentPoint * (crossBeamSupport.width / 2.0f + crossBeamSupport.width / 2.0f + AngledCrossBeamSupport.width / 2.0f)), normal * -1f, binormalFlat);
                    crossBeamSupport.end ();

                }
            }
        }

    }
    private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    private Vector3 rotateNormalAxis(Vector3 n, Vector3 dir,float axis)
    {
        return dir * Mathf.Cos(axis) + (Vector3.Cross(n,dir))* Mathf.Sin(axis) + n*(Vector3.Dot(n,dir)*(1-Mathf.Cos(axis)));
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
                rightSideWoodenTrack,
                crossBeamSupport,
                AngledCrossBeamSupport,
                CrossBeamRailSupportLeft,
                CrossBeamRailSupportRight
               
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

